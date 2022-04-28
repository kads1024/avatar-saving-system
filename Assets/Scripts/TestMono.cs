using AvatarSavingSystem;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    [SerializeField] AvatarDataManager mngr;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    AvatarData GenerateSampleAvatar(string p_Name)
    {
        AvatarData sampleData = new AvatarData();
        sampleData.DataName = p_Name;

        return sampleData;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SAVING");
            AvatarData temp = GenerateSampleAvatar("Test");
            mngr.AvatarDatas.Add(temp.DataName, temp);
            mngr.SaveToJSON(temp.DataName, "Assets/" + temp.DataName + ".json");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Loading");
            mngr.LoadFromJSON("Assets/Test.json");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(JsonUtility.ToJson(mngr.AvatarDatas["Test"], true));
        }

    }
}
