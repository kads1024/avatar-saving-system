using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSavingSystem
{
	/// <summary>
	/// Override retargeting information for attachments.
	/// </summary>
	public class PartAttachmentOverride : MonoBehaviour
    {
		/// <summary>
		/// Should this bone have its position retargeted.
		/// </summary>
		public bool RetargetPosition;

		/// <summary>
		/// Should this bone have its rotation retargeted.
		/// </summary>
		public bool RetargetRotation;

		/// <summary>
		/// Should this bone have its scale retargeted.
		/// </summary>
		public bool RetargetScale;
	}
}