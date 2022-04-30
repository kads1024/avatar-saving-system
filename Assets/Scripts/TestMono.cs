using AvatarSavingSystem;
using UnityEngine;
using System.Collections.Generic;
public class TestMono : MonoBehaviour
{
    [SerializeField] AvatarDataManager mngr;
    [SerializeField] string SampleAvatarID;
    [SerializeField] AvatarModel SampleAvatar;

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
            mngr.SaveToJSON(SampleAvatarID, "Assets/" + SampleAvatarID + ".json");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Loading");
            mngr.LoadFromJSON("Assets/" + SampleAvatarID + ".json");
            
            foreach(PartSlotData slotData in mngr.AvatarDatas[SampleAvatarID].SlotData)
            {
                SampleAvatar.Attach(slotData.SlotIndex, slotData.PartID);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(mngr.AvatarDatas[SampleAvatarID].SlotData.Count);
            Debug.Log(JsonUtility.ToJson(mngr.AvatarDatas[SampleAvatarID], true));
        }

    }
}
