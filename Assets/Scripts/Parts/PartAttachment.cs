using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AvatarSavingSystem
{
    /// <summary>
	/// Class that sets up this part and attaches itself to a parent gameobject
    /// </summary>
    public class PartAttachment : MonoBehaviour
    {
		/// <summary>
		/// Should bones have their position retargeted.
		/// </summary>
		public bool RetargetPosition;

		/// <summary>
		/// Should bones have their rotation retargeted.
		/// </summary>
		public bool RetargetRotation;

		/// <summary>
		/// Should bones have their scale retargeted.
		/// </summary>
		public bool RetargetScale;

		// Resources
		private AttachedRig[] AttachedRigs = null;
		private BonePair[] AttachedBones = null;
		private Transform AttachedParent = null;
		private Animation LegacyAnimation = null;
		private Animator Animator = null;

		private void Reset()
		{
			RetargetPosition = false;
			RetargetRotation = false;
			RetargetScale = false;
		}

		private void OnDestroy()
		{
			Detach();
		}

		private void LateUpdate()
		{

			// If not attached anywhere, do nothing
			if (AttachedBones == null)
			{
				return;
			}

			// Update each attached bone
			foreach (BonePair pair in AttachedBones)
			{

				// Make sure parent bone still exists
				if (pair.ParentBone == null)
				{
					Debug.LogError("Bone '" + pair.ChildBone.name + "' destroyed while animating.", pair.ChildBone);
					Detach();
					break;
				}

				// Update scale
				if (pair.ChildOverride?.RetargetScale ?? RetargetScale)
				{
					pair.ChildBone.localScale = new Vector3(
						pair.ChildScale.x * pair.ParentBone.ParentScale.x / pair.ParentBone.transform.localScale.x,
						pair.ChildScale.y * pair.ParentBone.ParentScale.y / pair.ParentBone.transform.localScale.y,
						pair.ChildScale.z * pair.ParentBone.ParentScale.z / pair.ParentBone.transform.localScale.z
					);
				}
				else
				{
					pair.ChildBone.localScale = pair.ParentBone.transform.localScale;
					Material x;
				}

				// Update rotation
				if (pair.ChildOverride?.RetargetRotation ?? RetargetRotation)
				{
					pair.ChildBone.localRotation = pair.ChildRotation * (pair.ParentBone.transform.localRotation * Quaternion.Inverse(pair.ParentBone.ParentRotation));
				}
				else
				{
					pair.ChildBone.localRotation = pair.ParentBone.transform.localRotation;
				}

				// Update position
				if (pair.ChildOverride?.RetargetPosition ?? RetargetPosition)
				{
					pair.ChildBone.localPosition = pair.ChildPosition + (pair.ParentBone.transform.localPosition - pair.ParentBone.ParentPosition);
				}
				else
				{
					pair.ChildBone.localPosition = pair.ParentBone.transform.localPosition;
				}

			}

		}

		/// <summary>
		/// Detach this attachment and stop it from animating.
		/// </summary>
		public void Detach()
		{

			// Remove rigs
			List<Transform> destroyed = null;
			AttachedRig[] rigs = Interlocked.Exchange(ref AttachedRigs, null);
			AttachedBones = null;
			AttachedParent = null;

			// Destroy rigs if no other attachment is using it
			if (rigs != null)
			{
				foreach (AttachedRig rig in rigs)
				{
					rig.Count--;
					if (rig.Count <= 0)
					{
						if (!rig.Destroyed)
						{
							rig.Destroyed = true;
							rig.transform.parent = null;
							Destroy(rig.gameObject);
						}
						if (destroyed == null) destroyed = new List<Transform>();
						destroyed.Add(rig.transform);
					}
				}
			}

			// If no rigs were destroyed, no need to rebind
			if (destroyed == null)
			{
				return;
			}

			// Remove destroyed rigs from legacy animation
			if (LegacyAnimation != null && AttachedRigs != null)
			{
				foreach (Transform transform in destroyed)
				{
					foreach (AnimationState state in LegacyAnimation)
					{
						state.RemoveMixingTransform(transform);
					}
				}
			}

			// Rebind animator if any rigs were destroyed
			if (Animator != null)
			{
				if (Animator.layerCount == 1)
				{
					AnimatorStateInfo info = Animator.GetCurrentAnimatorStateInfo(0);
					Animator.Rebind();
					Animator.Play(info.fullPathHash, 0, info.normalizedTime);
				}
				else if (Animator.layerCount > 1)
				{
					AnimatorStateInfo[] info = new AnimatorStateInfo[Animator.layerCount];
					for (int layer = 0; layer < Animator.layerCount; layer++)
					{
						info[layer] = Animator.GetCurrentAnimatorStateInfo(layer);
					}
					Animator.Rebind();
					for (int layer = 0; layer < Animator.layerCount; layer++)
					{
						Animator.Play(info[layer].fullPathHash, layer, info[layer].normalizedTime);
					}
				}
				else
				{
					Animator.Rebind();
				}
			}

		}

		/// <summary>
		/// Attach this attachment to a legacy animation.
		/// </summary>
		/// <param name="animation">Animation to attach to.</param>
		public void Rebind(Animation animation)
		{

			// If no animation provided, detach
			if (animation == null)
			{
				Detach();
				return;
			}

			// If no change, do nothing
			if (animation.transform == AttachedParent)
			{
				return;
			}

			// Rebind to the provided legacy animation
			bool rebind = Rebind(transform, animation.transform, out AttachedBones, out AttachedRigs);
			AttachedParent = animation.transform;
			LegacyAnimation = animation;
			Animator = null;

			// Add newly created rigs to the animation
			if (rebind)
			{
				foreach (AnimationState state in animation)
				{
					foreach (AttachedRig rig in AttachedRigs)
					{
						state.AddMixingTransform(rig.transform, true);
					}
				}
			}

			// Update the bones
			LateUpdate();

		}

		/// <summary>
		/// Attach this attachment to an animation controller.
		/// </summary>
		/// <param name="animator">Animator to attach to.</param>
		public void Rebind(Animator animator)
		{

			// If no animator provided, detach
			if (animator == null)
			{
				Detach();
				return;
			}

			// If no change, do nothing
			if (animator.transform == AttachedParent)
			{
				return;
			}

			// Rebind to the provided animator
			bool rebind = Rebind(transform, animator.transform, out AttachedBones, out AttachedRigs);
			AttachedParent = animator.transform;
			Animator = animator;
			LegacyAnimation = null;

			// Add newly created rigs to the animation
			if (rebind)
			{
				if (animator.layerCount == 1)
				{
					AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
					animator.Rebind();
					animator.Play(info.fullPathHash, 0, info.normalizedTime);
				}
				else if (animator.layerCount > 1)
				{
					AnimatorStateInfo[] info = new AnimatorStateInfo[animator.layerCount];
					for (int layer = 0; layer < animator.layerCount; layer++)
					{
						info[layer] = animator.GetCurrentAnimatorStateInfo(layer);
					}
					animator.Rebind();
					for (int layer = 0; layer < animator.layerCount; layer++)
					{
						animator.Play(info[layer].fullPathHash, layer, info[layer].normalizedTime);
					}
				}
				else
				{
					animator.Rebind();
				}
			}

			// Update the bones
			LateUpdate();

		}

		private static bool Rebind(Transform transform, Transform parent, out BonePair[] bones, out AttachedRig[] rigs)
		{
			bool rebind = false;
			List<BonePair> boneList = new List<BonePair>();
			List<AttachedRig> rigList = new List<AttachedRig>();
			foreach (Transform child in transform)
			{
				Renderer renderer = child.GetComponent<Renderer>();
				if (renderer == null)
				{
					AttachedRig rig = AttachRig(boneList, child, parent.transform, out bool rebindRig);
					if (rebindRig) rebind = true;
					rigList.Add(rig);
				}
			}
			bones = boneList.ToArray();
			rigs = rigList.ToArray();
			return rebind;
		}

		private static AttachedRig AttachRig(List<BonePair> bones, Transform child, Transform parent, out bool rebind)
		{
			rebind = false;
			AttachedRig rig = FindRigByName(parent, child.name);
			if (rig == null)
			{
				rig = Instantiate(child, parent).gameObject.AddComponent<AttachedRig>();
				rig.name = child.name;
				rebind = true;
			}
			rig.Count++;
			ExtractBones(bones, child, rig.transform, out bool rebindBones);
			if (rebindBones) rebind = true;
			return rig;

		}

		private static void ExtractBones(List<BonePair> bones, Transform local, Transform animated, out bool rebind)
		{
			rebind = false;
			Transform parent = FindByNameRecursive(animated, local.name);
			if (parent == null)
			{
				//Debug.LogWarning("Rig bone '" + local.name + "' not found.", local);
				parent = Instantiate(local, animated);
				parent.name = local.name;
				rebind = true;
			}
			ParentBone bone = parent.GetComponent<ParentBone>();
			if (bone == null)
			{
				bone = parent.gameObject.AddComponent<ParentBone>();
				bone.ParentPosition = parent.localPosition;
				bone.ParentRotation = parent.localRotation;
				bone.ParentScale = parent.localScale;
			}
			bones.Add(new BonePair()
			{
				ChildOverride = local.GetComponent<PartAttachmentOverride>(),
				ChildBone = local,
				ChildPosition = local.localPosition,
				ChildRotation = local.localRotation,
				ChildScale = local.localScale,
				ParentBone = bone,
			});
			foreach (Transform child in local)
			{
				ExtractBones(bones, child, parent, out bool rebindBones);
				if (rebindBones) rebind = true;
			}
		}

		private static AttachedRig FindRigByName(Transform parent, string name)
		{
			foreach (Transform child in parent)
			{
				if (child.name == name)
				{
					AttachedRig rig = child.GetComponent<AttachedRig>();
					if (rig == null)
					{
						//Debug.LogWarning("No rig on '" + name + "'.", child);
						rig = child.gameObject.AddComponent<AttachedRig>();
						rig.Count = 1;
					}
					return rig;
				}
			}
			return null;
		}

		private static Transform FindByNameRecursive(Transform skeleton, string name)
		{
			if (skeleton == null || skeleton.name == name) return skeleton;
			foreach (Transform bone in skeleton)
			{
				Transform found = FindByNameRecursive(bone, name);
				if (found != null) return found;
			}
			return null;
		}
	}
}