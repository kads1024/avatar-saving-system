using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSavingSystem
{
    [CreateAssetMenu(menuName = "AvatarPart", fileName = "SampleAvatarPart")]
    public class AvatarPart : ScriptableObject
    {
        /// <summary>
        /// Name of the part this Scriptable object holds.
        /// </summary>
        public string PartName;

        /// <summary>
        /// Slot on where to insert the part.
        /// </summary>
        public string Slot;

        /// <summary>
        /// Actual Prefab that will be inserted to the slot
        /// </summary>
        public PartAttachment PartPrefab;

        /// <summary>
        /// List of valid Main colors for the part.
        /// </summary>
        public List<Color> MainColors;

        /// <summary>
        /// List of valid Accent colors for the part
        /// </summary>
        public List<Color> AccentColors;

        /// <summary>
        ///List of valid Secondary Accent colors for the part.
        /// </summary>
        public List<Color> SecondaryAccentColors;
    }
}