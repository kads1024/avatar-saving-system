using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AvatarSavingSystem
{
    [CreateAssetMenu(menuName = "Managers/AvatarDataManager", fileName = "AvatarDataManager")]
    public class AvatarDataManager : ScriptableObject
    {
        private Dictionary<string, AvatarData> _avatarDatas = new Dictionary<string, AvatarData>();
        public Dictionary<string, AvatarData> AvatarDatas 
        { 
            get 
            {
                return _avatarDatas;
            }
            set
            {
                _avatarDatas = value;
            }
        }

        /// <summary>
        /// Save Avatar as JSON file on a given path.
        /// </summary>
        /// <param name="p_AvatarKey">Avatar Key to be saved.</param>
        /// <param name="p_FilePath">Desired file path.</param>
        /// <returns>Returns 'true' on success, otherwise 'false'.</returns>
        public bool SaveToJSON(string p_AvatarKey, string p_FilePath)
        {
            try {
                if (!_avatarDatas.ContainsKey(p_AvatarKey)) throw new Exception("ERROR: INVALID AVATAR DATA KEY");

                AvatarData data = _avatarDatas[p_AvatarKey];
                data.DataName = p_AvatarKey;
                data.DataVersion++;

                string content = JsonUtility.ToJson(data, true);
                string directory = Path.GetDirectoryName(p_FilePath);

                if (string.IsNullOrEmpty(directory))
                    directory = Directory.GetCurrentDirectory();
                
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(p_FilePath, content);
                return true;
            }
            catch (Exception e) {
                Debug.LogError("error on save to JSON:\n" + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Load and assign data from a JSON file in a given path to _avatarDatas.
        /// </summary>
        /// <param name="p_FilePath">Desired file path.</param>
        /// <returns>Returns 'true' on success, otherwise 'false'.</returns>
        public bool LoadFromJSON(string p_FilePath)
        {
            if (!File.Exists(p_FilePath))
            {
                Debug.LogWarning("Load AvatarData '" + name + "' from JSON failed: file doesn't exist");
                return false;
            }

            try
            {
                AvatarData data = JsonUtility.FromJson<AvatarData>(File.ReadAllText(p_FilePath));

                if (_avatarDatas.ContainsKey(data.DataName))
                    _avatarDatas[data.DataName] = data;
                else
                    _avatarDatas.Add(data.DataName, data);
              
                
                // TODO: Assign and Apply Avatar Data

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("error on load from JSON:\n" + e.ToString());
                return false;
            }
        }
    }
}