using System;
using UnityEngine;

namespace AvatarSavingSystem
{
	// Reference counter on the rig that is being animated
	public class AttachedRig : MonoBehaviour
	{
		[NonSerialized] public int Count = 0;
		[NonSerialized] public bool Destroyed = false;
		private void OnDestroy() { Destroyed = true; }
	}
}
