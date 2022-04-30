using System;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSavingSystem
{
    public struct AvatarData
    {
        /// <summary>
        /// JSON's version of the data.
        /// </summary>
        public int DataVersion;

        /// <summary>
        /// Name of the specified Avatar Data
        /// </summary>
        public string DataName;

        /// <summary>
        /// List of PartSlotData for the avatar like Armor, Helmet, Clothes, Hair, Facial Hair, BodyMarkings, particle effects, etc
        /// </summary>
        public List<PartSlotData> SlotData;

        

        public AvatarData(int p_DataVersion, string p_DataName, int p_SlotCapacity)
        {
            DataVersion = p_DataVersion;
            DataName = p_DataName;

            SlotData = new List<PartSlotData>();
            for (int i = 0; i < p_SlotCapacity; i++) SlotData.Add(new PartSlotData() { SlotIndex = i });
        }
    }

    [Serializable]
    public struct PartSlotData
    {
        /// <summary>
        /// Slot Index on where to insert the part.
        /// </summary>
        public int SlotIndex;

        /// <summary>
        /// Part to be inserted.
        /// </summary>
        public string PartID;

        /// <summary>
        /// Texture.
        /// </summary>
        public int TextureIndex;

        /// <summary>
        /// First color.
        /// </summary>
        public int MainColorIndex;

        /// <summary>
        /// Second color.
        /// </summary>
        public int AccentColorIndex;

        /// <summary>
        /// Third color.
        /// </summary>
        public int SecondaryAccentColorIndex;

        /// <summary>
        /// List of SegmentScaleData for blend shapes
        /// </summary>
        public List<SegmentScaleData> BodySegmentData;

        public PartSlotData(int p_SlotIndex, string p_PartID, int p_TextureIndex, int p_MainColorIndex, int p_AccentColorIndex, int p_SecondaryAccentIndex, int p_bodySegmentCapacity)
        {
            SlotIndex = p_SlotIndex >= 0 ? p_SlotIndex : 0;
            PartID = p_PartID;
            TextureIndex = p_TextureIndex >= 0 ? p_TextureIndex : 0;
            MainColorIndex = p_MainColorIndex >= 0 ? p_MainColorIndex : 0;
            AccentColorIndex = p_AccentColorIndex >= 0 ? p_AccentColorIndex : 0;
            SecondaryAccentColorIndex = p_SecondaryAccentIndex >= 0 ? p_SecondaryAccentIndex : 0;
            BodySegmentData = new List<SegmentScaleData>();
            for (int i = 0; i < p_bodySegmentCapacity; i++) BodySegmentData.Add(new SegmentScaleData());
        }
    }

    [Serializable]
    public struct SegmentScaleData
    {
        /// <summary>
        /// Body segment's Index
        /// </summary>
        public int SegmentIndex;

        /// <summary>
        /// Segment's scale
        /// </summary>
        public float Scale;
    }
}
