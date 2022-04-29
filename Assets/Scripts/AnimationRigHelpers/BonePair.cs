using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarSavingSystem
{
	// Bone information to update
	public struct BonePair
	{
		public PartAttachmentOverride ChildOverride;
		public Transform ChildBone;
		public Quaternion ChildRotation;
		public Vector3 ChildPosition;
		public Vector3 ChildScale;
		public ParentBone ParentBone;
	}
}
