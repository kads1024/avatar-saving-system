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
        /// List of PartSlotData for the avatar
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
        public string PartName;

        /// <summary>
        /// First color.
        /// </summary>
        public Color Color1;

        /// <summary>
        /// Second color.
        /// </summary>
        public Color Color2;

        /// <summary>
        /// Third color.
        /// </summary>
        public Color Color3;
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
