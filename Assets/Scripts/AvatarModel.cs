using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AvatarSavingSystem
{
    public class AvatarModel : MonoBehaviour
    {
		private const string AVATAR_PARTS_PATH = "AvatarParts/";

		/// <summary>
		/// Avatar ID of this model
		/// </summary>
		[SerializeField] private string _avatarID;

		/// <summary>
		/// Avatar Data Manager
		/// </summary>
		[SerializeField]  private AvatarDataManager _dataManager;

		/// <summary>
		/// All slots where parts are to be inserted.
		/// </summary>
		[SerializeField] private List<AvatarPartSlot> _slots;

		/// <summary>
		/// Legacy animation to use (If Using Legacy Animation).
		/// </summary>
		private Animation _legacyAnimation;

		/// <summary>
		/// Animation controller to use
		/// </summary>
		private Animator _animator;

		/// <summary>
		/// All Part attachments currently  inserted.
		/// </summary>
		private PartAttachment[] PartAttachments;

		/// <summary>
		/// Find attached animation and animator on Awake
		/// </summary>
		private void Awake()
        {
			_legacyAnimation = GetComponent<Animation>();
			_animator = GetComponent<Animator>();
		}

        /// <summary>
        /// Attach default parts on all required slots.
        /// </summary>
        private void Start()
		{
			// Initialize Data for this model into the datamanager
			if (!_dataManager.AvatarDatas.ContainsKey(_avatarID))
			{
				AvatarData newData = new AvatarData(1, _avatarID, _slots.Count);
				_dataManager.AvatarDatas.Add(_avatarID, newData);
			}


			for (int slot = 0; slot < _slots.Count; slot++)
            {
				PartSlotData data = new PartSlotData(slot, "Base" + slot, 0, 0, 0, 0, 0);
				if (_slots[slot].Required) Attach(data);
			}
				
		}

		/// <summary>
		/// Find attached animation and animator on Reset
		/// </summary>
		private void Reset()
		{
			_legacyAnimation = GetComponent<Animation>();
			_animator = GetComponent<Animator>();
		}

		/// <summary>
		/// Start animating with a new legacy animation.
		/// </summary>
		/// <param name="animation">Animation to use.</param>
		public void Rebind(Animation animation)
		{
			_legacyAnimation = animation;
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
			_animator = animator;
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
		/// <param name="p_PartSlotData">Part to be used.</param>
		public void Attach(PartSlotData p_PartSlotData)
		{
			
			// Check if provided slot is valid
			if (p_PartSlotData.SlotIndex < 0 || p_PartSlotData.SlotIndex >= _slots.Count)
			{
				Debug.LogWarning("Slot " + p_PartSlotData.SlotIndex + " not found.", this);
				return;
			}

			// Check if provided part is valid
			AvatarPart avatarPart = Resources.Load<AvatarPart>(AVATAR_PARTS_PATH + _slots[p_PartSlotData.SlotIndex].SlotName + "/" + p_PartSlotData.PartID);
			if (avatarPart == null || avatarPart.PartPrefab == null)
			{
				Debug.LogWarning("Part " + p_PartSlotData.PartID + " on slot " + p_PartSlotData.SlotIndex + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, _slots.Count, null);
			
			// If attachment already exists, destroy it
			if (PartAttachments[p_PartSlotData.SlotIndex] != null) Destroy(PartAttachments[p_PartSlotData.SlotIndex].gameObject);

			// Instantiate the new attachement and save it in the attachment list
			PartAttachments[p_PartSlotData.SlotIndex] = Instantiate(avatarPart.PartPrefab, transform);

			// Save Slot Data
			if(_dataManager.AvatarDatas[_avatarID].SlotData.Count > p_PartSlotData.SlotIndex)
            {
				Debug.Log("Saving Slot: " + p_PartSlotData.SlotIndex, this);
				_slots[p_PartSlotData.SlotIndex].SlotData = p_PartSlotData;
			}
				

			// Apply Texture and Color
			Texture texture = p_PartSlotData.TextureIndex < avatarPart.Textures.Count && p_PartSlotData.TextureIndex >= 0 ? 
				avatarPart.Textures[p_PartSlotData.TextureIndex] : avatarPart.Textures[0];
			Color main = p_PartSlotData.MainColorIndex < avatarPart.MainColors.Count && p_PartSlotData.MainColorIndex >= 0 ? 
				avatarPart.MainColors[p_PartSlotData.MainColorIndex] : avatarPart.MainColors[0];
			Color accent = p_PartSlotData.AccentColorIndex < avatarPart.AccentColors.Count && p_PartSlotData.AccentColorIndex >= 0 ? 
				avatarPart.AccentColors[p_PartSlotData.AccentColorIndex] : avatarPart.AccentColors[0];
			Color secondaryAccent = p_PartSlotData.SecondaryAccentColorIndex < avatarPart.SecondaryAccentColors.Count && p_PartSlotData.SecondaryAccentColorIndex >= 0 ? 
				avatarPart.SecondaryAccentColors[p_PartSlotData.SecondaryAccentColorIndex] : avatarPart.SecondaryAccentColors[0];

			PartAttachments[p_PartSlotData.SlotIndex].ApplyPartData(texture, main, accent, secondaryAccent);

			// Apply Blendshapes
			if(p_PartSlotData.BodySegmentData != null) 
				foreach(SegmentScaleData bodySegment in p_PartSlotData.BodySegmentData)
					PartAttachments[p_PartSlotData.SlotIndex].ApplySegmentScale(bodySegment.SegmentIndex, bodySegment.Scale);
			
			

			// Rebind animator in case any new
			if (_animator != null) PartAttachments[p_PartSlotData.SlotIndex].Rebind(_animator);
			if (_legacyAnimation != null) PartAttachments[p_PartSlotData.SlotIndex].Rebind(_legacyAnimation);

			// Detach any conflicts
			foreach (int conflict in _slots[p_PartSlotData.SlotIndex].Conflicts)
			{
				if (conflict < 0 || conflict >= _slots.Count) continue;
				if (conflict == p_PartSlotData.SlotIndex) continue;
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
			if (p_Slot < 0 || p_Slot >= _slots.Count)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, _slots.Count, null);

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

		/// <summary>
		/// Saves the avatar configuration to json
		/// </summary>
		public void SaveAvatarConfiguration()
        {
			for (int i = 0; i < _slots.Count; i++)            
				_dataManager.AvatarDatas[_avatarID].SlotData[i] = _slots[i].SlotData;

			_dataManager.SaveToJSON(_avatarID, "Assets/" + _avatarID + ".json");
        }
	}
}