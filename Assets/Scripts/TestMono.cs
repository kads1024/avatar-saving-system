using AvatarSavingSystem;
using UnityEngine;
using System.Collections.Generic;
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
        sampleData.SlotData = new List<PartSlotData>();
        sampleData.SlotData.Add(new PartSlotData());
        sampleData.SlotData.Add(new PartSlotData());
        sampleData.BodySegmentData = new List<SegmentScaleData>();
        sampleData.BodySegmentData.Add(new SegmentScaleData());
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
