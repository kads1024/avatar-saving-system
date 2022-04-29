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
		public AvatarPartSlot[] Slots;

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
		/// Part index currently animating on each slot or -1 for none.
		/// </summary>
		private int[] CurrentlyEquippedParts;

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
			for (int slot = 0; slot < Slots.Length; slot++)
			{
				if (Slots[slot].Required) Attach(slot, 0);
			}
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
		/// Replace prefab on a specific slot.
		/// </summary>
		/// <param name="p_Slot">Slot to be inserted.</param>
		/// <param name="p_PartIndex">Index of the part to be used.</param>
		public void Attach(int p_Slot, int p_PartIndex)
		{

			// Check if provided slot is valid
			if (p_Slot < 0 || p_Slot >= Slots.Length)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return;
			}

			// Check if provided part index is valid within the slot
			if (p_PartIndex < 0 || p_PartIndex >= Slots[p_Slot].PartAttachments.Length)
			{
				Debug.LogWarning("Part Index " + p_PartIndex + " on slot " + p_Slot + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, Slots.Length, null);
			Resize(ref CurrentlyEquippedParts, Slots.Length, -1);

			// If attachment already exists, destroy it
			if (PartAttachments[p_Slot] != null) Destroy(PartAttachments[p_Slot].gameObject);

			// Instantiate the new attachement and save it in both the attachment list and the index list
			PartAttachments[p_Slot] = Instantiate(Slots[p_Slot].PartAttachments[p_PartIndex], transform);
			CurrentlyEquippedParts[p_Slot] = p_PartIndex;

			// TODO: REBIND ANIMATIONS?

			// Detach any conflicts
			foreach (int conflict in Slots[p_Slot].Conflicts)
			{
				if (conflict < 0 || conflict >= Slots.Length) continue;
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
			if (p_Slot < 0 || p_Slot >= Slots.Length)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, Slots.Length, null);
			Resize(ref CurrentlyEquippedParts, Slots.Length, -1);

			// Destroy attachment if it exists
			if (PartAttachments[p_Slot] != null) Destroy(PartAttachments[p_Slot].gameObject);

			// Remove attachment
			PartAttachments[p_Slot] = null;
			CurrentlyEquippedParts[p_Slot] = -1;

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

		/// <summary>
		/// Get current index of the prefab used on a specific slot or -1 if slot is empty.
		/// </summary>
		/// <param name="p_Slot">Slot to check.</param>
		/// <returns>Prefab index on the slot.</returns>
		public int GetEquippedPartInSlot(int p_Slot)
		{

			// Check if provided slot is valid
			if (p_Slot < 0 || p_Slot >= Slots.Length)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return -1;
			}

			// Resize resources
			Resize(ref PartAttachments, Slots.Length, null);
			Resize(ref CurrentlyEquippedParts, Slots.Length, -1);

			// Return current prefab index
			return CurrentlyEquippedParts[p_Slot];

		}
	}
}