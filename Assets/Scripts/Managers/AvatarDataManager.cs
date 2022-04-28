using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AvatarSavingSystem
{
    [Serializable]
    public class AvatarDataDictionary : Dictionary<string, AvatarData> { }

    [CreateAssetMenu(menuName = "Managers/AvatarDataManager", fileName = "AvatarDataManager")]
    public class AvatarDataManager : ScriptableObject
    {
        [SerializeField] private AvatarDataDictionary _avatarDatas = new AvatarDataDictionary() { };
        public AvatarDataDictionary AvatarDatas 
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
            try
            {
                if (!_avatarDatas.ContainsKey(p_AvatarKey)) throw new Exception("ERROR: INVALID AVATAR DATA KEY");

                AvatarData data = _avatarDatas[p_AvatarKey];

                string content = JsonUtility.ToJson(data, true);
                string directory = Path.GetDirectoryName(p_FilePath);

                if (string.IsNullOrEmpty(directory))
                    directory = Directory.GetCurrentDirectory();
                
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(p_FilePath, content);
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