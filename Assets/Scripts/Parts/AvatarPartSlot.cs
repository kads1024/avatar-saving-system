using System;

namespace AvatarSavingSystem
{
    [Serializable]
    public class AvatarPartSlot 
    {
		/// <summary>
		/// Which slots are incompatible with this slot.
		/// </summary>
		public int[] Conflicts;

		/// <summary>
		/// List of Prefab parts to choose from.
		/// </summary>
		public PartAttachment[] PartAttachments;

		/// <summary>
		/// Asset path to load prefabs from.
		/// </summary>
		public string AssetPath;

		/// <summary>
		/// True if slot is required and must not be empty.
		/// </summary>
		public bool Required;
	}
}
