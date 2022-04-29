using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarSavingSystem
{
	// Original parent bone information before any animating
	public class ParentBone : MonoBehaviour
	{
		public Quaternion ParentRotation;
		public Vector3 ParentPosition;
		public Vector3 ParentScale;
	}
}