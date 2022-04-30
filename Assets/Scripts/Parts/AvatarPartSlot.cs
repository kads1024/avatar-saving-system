using System;

namespace AvatarSavingSystem
{
    [Serializable]
    public class AvatarPartSlot 
    {
		/// <summary>
		/// Name of the Slot
		/// </summary>
		public string SlotName;

		/// <summary>
		/// Which slots are incompatible with this slot.
		/// </summary>
		public int[] Conflicts;

		/// <summary>
		/// True if slot is required and must not be empty.
		/// </summary>
		public bool Required;
	}
}
