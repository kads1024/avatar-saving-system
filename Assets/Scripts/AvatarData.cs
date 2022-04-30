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
        /// Object's BodyType.
        /// </summary>
        public BodyType BodyType;

        /// <summary>
        /// Object's skin color.
        /// </summary>
        public Color SkinColor;

        /// <summary>
        /// Object's tint color.
        /// </summary>
        public Color TintColor;

        /// <summary>
        /// List of PartSlotData for the avatar like Armor, Helmet, Clothes, Hair, Facial Hair, BodyMarkings, particle effects, etc
        /// </summary>
        public List<PartSlotData> SlotData;

        /// <summary>
        /// List of SegmentScaleData
        /// </summary>
        public List<SegmentScaleData> bodySegmentData;
    }

    [Serializable]
    public struct PartSlotData
    {
        /// <summary>
        /// Slot on where to insert the part.
        /// </summary>
        public string Slot;

        /// <summary>
        /// Part to be inserted.
        /// </summary>
        public string PartID;

        /// <summary>
        /// First color.
        /// </summary>
        public Color MainColor;

        /// <summary>
        /// Second color.
        /// </summary>
        public Color AccentColor;

        /// <summary>
        /// Third color.
        /// </summary>
        public Color SecondaryAccentColor;
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
