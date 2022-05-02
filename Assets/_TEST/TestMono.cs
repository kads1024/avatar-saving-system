using AvatarSavingSystem;
using UnityEngine;
using System.Collections.Generic;

public class TestMono : MonoBehaviour
{
    [SerializeField] AvatarDataManager mngr;
    [SerializeField] string SampleAvatarID;
    [SerializeField] AvatarModel SampleAvatar;

    [SerializeField] PartSlotData ArmsID;
    [SerializeField] PartSlotData BootsID;
    [SerializeField] PartSlotData GlovesID;
    [SerializeField] PartSlotData HatsID;
    [SerializeField] PartSlotData HeadsID;
    [SerializeField] PartSlotData LegsID;
    [SerializeField] PartSlotData PantsID;
    [SerializeField] PartSlotData ShirtsID;
    [SerializeField] PartSlotData TorsosID;
    [SerializeField] PartSlotData RiflesID;
    [SerializeField] PartSlotData Balls;

    // Start is called before the first frame update
    void Start()
    {
        UpdateAvatar();
    }

    AvatarData GenerateSampleAvatar(string p_Name)
    {
        AvatarData sampleData = new AvatarData();
        sampleData.DataName = p_Name;
        sampleData.SlotData = new List<PartSlotData>();
        sampleData.SlotData.Add(new PartSlotData());
        sampleData.SlotData.Add(new PartSlotData());
        return sampleData;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SAVING");
            SampleAvatar.SaveAvatarConfiguration();
            mngr.SaveToJSON(SampleAvatarID, "_TEST/Assets/" + SampleAvatarID + ".json");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Loading");
            mngr.LoadFromJSON("_TEST/Assets/" + SampleAvatarID + ".json");
            
            foreach(PartSlotData slotData in mngr.AvatarDatas[SampleAvatarID].SlotData)
            {
                SampleAvatar.Attach(slotData);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(JsonUtility.ToJson(mngr.AvatarDatas[SampleAvatarID], true));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdateAvatar();
        }

    }

    public void UpdateAvatar()
    {
        SampleAvatar.Attach(ArmsID);
        SampleAvatar.Attach(BootsID);
        SampleAvatar.Attach(GlovesID);
        SampleAvatar.Attach(HatsID);
        SampleAvatar.Attach(HeadsID);
        SampleAvatar.Attach(LegsID);
        SampleAvatar.Attach(PantsID);
        SampleAvatar.Attach(ShirtsID);
        SampleAvatar.Attach(TorsosID);
        SampleAvatar.Attach(RiflesID);
    }

}
