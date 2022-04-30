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
		[SerializeField] private Animation _legacyAnimation;

		/// <summary>
		/// Animation controller to use
		/// </summary>
		[SerializeField] private Animator _animator;

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
				if(_slots[slot].Required) Attach(slot, "Base" + slot.ToString());		
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
		/// <param name="p_Slot">Slot to be inserted.</param>
		/// <param name="p_PartName">Part to be used.</param>
		public void Attach(int p_Slot, string p_PartName)
		{
			
			// Check if provided slot is valid
			if (p_Slot < 0 || p_Slot >= _slots.Count)
			{
				Debug.LogWarning("Slot " + p_Slot + " not found.", this);
				return;
			}

			// Check if provided part is valid
			AvatarPart avatarPart = Resources.Load<AvatarPart>(AVATAR_PARTS_PATH + _slots[p_Slot].SlotName + "/" + p_PartName);
			if (avatarPart == null || avatarPart.PartPrefab == null)
			{
				Debug.LogWarning("Part " + p_PartName + " on slot " + p_Slot + " not found.", this);
				return;
			}

			// Resize the part attachments to match the same size as the slots
			Resize(ref PartAttachments, _slots.Count, null);
			
			// If attachment already exists, destroy it
			if (PartAttachments[p_Slot] != null) Destroy(PartAttachments[p_Slot].gameObject);

			// Instantiate the new attachement and save it in the attachment list
			PartAttachments[p_Slot] = Instantiate(avatarPart.PartPrefab, transform);

			// Save Slot Data
			if(_dataManager.AvatarDatas[_avatarID].SlotData.Count > p_Slot)
            {
				Debug.Log("Saving Slot: " + p_Slot, this);
				_slots[p_Slot].SlotData = _dataManager.AvatarDatas[_avatarID].SlotData[p_Slot];
			}
				

			// Apply Texture and Color
			Texture texture = _slots[p_Slot].SlotData.TextureIndex < avatarPart.Textures.Count ? avatarPart.Textures[_slots[p_Slot].SlotData.TextureIndex] : avatarPart.Textures[0];
			Color main = _slots[p_Slot].SlotData.MainColorIndex < avatarPart.MainColors.Count ? avatarPart.MainColors[_slots[p_Slot].SlotData.MainColorIndex] : avatarPart.MainColors[0];
			Color accent = _slots[p_Slot].SlotData.AccentColorIndex < avatarPart.AccentColors.Count ? avatarPart.AccentColors[_slots[p_Slot].SlotData.AccentColorIndex] : avatarPart.AccentColors[0];
			Color secondaryAccent = _slots[p_Slot].SlotData.SecondaryAccentColorIndex < avatarPart.SecondaryAccentColors.Count ? avatarPart.SecondaryAccentColors[_slots[p_Slot].SlotData.SecondaryAccentColorIndex] : avatarPart.SecondaryAccentColors[0];

			PartAttachments[p_Slot].ApplyPartData(texture, main, accent, secondaryAccent);

			// Apply Blendshapes
			if(_slots[p_Slot].SlotData.BodySegmentData != null) 
				foreach(SegmentScaleData bodySegment in _slots[p_Slot].SlotData.BodySegmentData)
					PartAttachments[p_Slot].ApplySegmentScale(bodySegment.SegmentIndex, bodySegment.Scale);
			
			

			// Rebind animator in case any new
			if (_animator != null) PartAttachments[p_Slot].Rebind(_animator);
			if (_legacyAnimation != null) PartAttachments[p_Slot].Rebind(_legacyAnimation);

			// Detach any conflicts
			foreach (int conflict in _slots[p_Slot].Conflicts)
			{
				if (conflict < 0 || conflict >= _slots.Count) continue;
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