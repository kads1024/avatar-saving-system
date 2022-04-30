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

        /// <summary>
        /// List of SegmentScaleData
        /// </summary>
        public List<SegmentScaleData> BodySegmentData;

        public AvatarData(int p_DataVersion, string p_DataName, int p_SlotCapacity, int p_SegmentCapacity)
        {
            DataVersion = p_DataVersion;
            DataName = p_DataName;
            SlotData = new List<PartSlotData>(p_SlotCapacity);
            BodySegmentData = new List<SegmentScaleData>(p_SegmentCapacity);
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
    }

    [Serializable]
    public struct SegmentScaleData
    {
        /// <summary>
        /// Body segment's type
        /// </summary>
        public string SegmentType;

        /// <summary>
        /// Segment's scale
        /// </summary>
        public Vector2 Scale;
    }
}
