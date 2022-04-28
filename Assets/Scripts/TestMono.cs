using AvatarSavingSystem;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    [SerializeField] AvatarDataManager mngr;
    // Start is called before the first frame update
    void Start()
    {
        AvatarData temp = GenerateSampleAvatar("Test");
        mngr.AvatarDatas.Add(temp.DataName, temp);
        mngr.SaveToJSON(temp.DataName, "Assets/" + temp.DataName + ".json");
    }

    AvatarData GenerateSampleAvatar(string p_Name)
    {
        AvatarData sampleData = new AvatarData();
        sampleData.DataName = p_Name;

        return sampleData;
    }
}
