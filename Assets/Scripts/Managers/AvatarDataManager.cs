using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AvatarSavingSystem
{
    public class AvatarDataManager
    {
        /// <summary>
        /// Generates a default value for a certain avatar
        /// </summary>
        /// <returns>AvatarData with default values</returns>
        public AvatarData GenerateDefaultData()
        {
            AvatarData data = new AvatarData();

            data.DataVersion = 1;
            data.BodyType = BodyType.Character_Male;
            data.SkinColor = Color.white;
            data.TintColor = Color.black;
            data.SlotData = new List<PartSlotData>();
            data.bodySegmentData = new List<SegmentScaleData>();

            return data;
        }

        /// <summary>
        /// Save Avatar as JSON file on a given path.
        /// </summary>
        /// <param name="filePath">Desired file path.</param>
        /// <returns>Returns 'true' on success, otherwise 'false'.</returns>
        public bool SaveToJSON(string filePath)
        {
            try
            {
                AvatarData data = GenerateDefaultData();

                string content = JsonUtility.ToJson(data, true);
                string directory = Path.GetDirectoryName(filePath);

                if (string.IsNullOrEmpty(directory))
                    directory = Directory.GetCurrentDirectory();
                
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(filePath, content);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("error on save to JSON:\n" + e.ToString());
                return false;
            }
        }
    }
}