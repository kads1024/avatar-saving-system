using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarSavingSystem
{
    public class AvatarModel : MonoBehaviour
    {
		/// <summary>
		/// All slots where parts are to be inserted.
		/// </summary>
		public List<AvatarPartSlot> Slots;

		/// <summary>
		/// Legacy animation to use (If Using Legacy Animation).
		/// </summary>
		public Animation LegacyAnimation;

		/// <summary>
		/// Animation controller to use
		/// </summary>
		public Animator Animator;

		/// <summary>
		/// All Part attachments currently  inserted.
		/// </summary>
		private PartAttachment[] PartAttachments;

		/// <summary>
		/// Find attached animation and animator on Awake
		/// </summary>
		private void Awake()
        {
			LegacyAnimation = GetComponent<Animation>();
			Animator = GetComponent<Animator>();
		}

        /// <summary>
        /// Attach default parts on all required slots.
        /// </summary>
        private void Start()
		{	
			for (int slot = 0; slot < Slots.Count; slot++)
				if(Slots[slot].Required) Attach(slot, "Base" + slot.ToString());	
		}

		/// <summary>
		/// Find attached animation and animator on Reset
		/// </summary>
		private void Reset()
		{
			LegacyAnimation = GetComponent<Animation>();
			Animator = GetComponent<Animator>();
		}

		/// <summary>
		/// Start animating with a new legacy animation.
		/// </summary>
		/// <param name="animation">Animation to use.</param>
		public void Rebind(Animation animation)
		{
			LegacyAnimation = animation;
			if (PartAttachments != null)
			{
				foreach (PartAttachment attachment in PartAttachments)
				{
					if (attachment != null) attachment.Rebind(animation);
				}
			}
		}

		/// <summary>
		/// Start animating with a new animation controller.
		/// </summary>
		/// <param name="animator">Animator to use.</param>
		public void Rebind(Animator animator)
		{
			Animator = animator;
			if (PartAttachments != null)
			{
				foreach (PartAttachment attachment in PartAttachments)
				{
					if (attachment != null) attachment.Rebind(animator);
				}
			}
		}

		/// <summary>
		/// Replace prefab on a specific slot.
		/// </summary>
		/// <param name="p_Slot">Slot to be inserted.</param>
		/// <param name="p_PartName">Part to be used.</param>
		public void Attach(int p_Slot, string p_PartName)
		{
			
			// Check if provided slot is valid
			if (p_Slot < 0 || p_Slot >= Slots.Count)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return;
			}

			// Check if provided part is valid
			AvatarPart avatarPart = Resources.Load<AvatarPart>("AvatarParts/" + p_PartName);
			if (avatarPart == null || avatarPart.PartPrefab == null)
			{
				Debug.LogWarning("Part " + p_PartName + " on slot " + p_Slot + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, Slots.Count, null);

			// If attachment already exists, destroy it
			if (PartAttachments[p_Slot] != null) Destroy(PartAttachments[p_Slot].gameObject);

			// Instantiate the new attachement and save it in both the attachment list
			Debug.Log("INSTANTIATING " + avatarPart.ToString());
			PartAttachments[p_Slot] = Instantiate(avatarPart.PartPrefab, transform);

			// Rebind animator in case any new
			if (Animator != null) PartAttachments[p_Slot].Rebind(Animator);
			if (LegacyAnimation != null) PartAttachments[p_Slot].Rebind(LegacyAnimation);

			// Detach any conflicts
			foreach (int conflict in Slots[p_Slot].Conflicts)
			{
				if (conflict < 0 || conflict >= Slots.Count) continue;
				if (conflict == p_Slot) continue;
				Detach(conflict);
			}

		}

		/// <summary>
		/// Detach any attachments from the slot.
		/// </summary>
		/// <param name="p_Slot">Slot to detach from.</param>
		public void Detach(int p_Slot)
		{

			// Check if provided slot is valid
			if (p_Slot < 0 || p_Slot >= Slots.Count)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, Slots.Count, null);

			// Destroy attachment if it exists
			if (PartAttachments[p_Slot] != null) Destroy(PartAttachments[p_Slot].gameObject);

			// Remove attachment
			PartAttachments[p_Slot] = null;

		}

		/// <summary>
		/// Resize array and fill the extra slots with the provided value (if any).
		/// </summary>
		private static void Resize<T>(ref T[] p_Array, int p_Length, T p_Value)
		{
			int size = p_Array == null ? 0 : p_Array.Length;
			System.Array.Resize(ref p_Array, p_Length);

			// Fill in the rest of the values if the array got bigger
			for (int i = size; i < p_Length; i++)
				p_Array[i] = p_Value;
			
		}
	}
}