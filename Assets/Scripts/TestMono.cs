using AvatarSavingSystem;
using UnityEngine;
using System.Collections.Generic;
public class TestMono : MonoBehaviour
{
    [SerializeField] AvatarDataManager mngr;
    [SerializeField] string SampleAvatarID;
    [SerializeField] AvatarModel SampleAvatar;

    [SerializeField] string ArmsID;
    [SerializeField] string BootsID;
    [SerializeField] string GlovesID;
    [SerializeField] string HatsID;
    [SerializeField] string HeadsID;
    [SerializeField] string LegsID;
    [SerializeField] string PantsID;
    [SerializeField] string ShirtsID;
    [SerializeField] string TorsosID;
    [SerializeField] string RiflesID;
    [SerializeField] string Balls;

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
        return sampleData;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("SAVING");
            SampleAvatar.SaveAvatarConfiguration();
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
            Debug.Log(JsonUtility.ToJson(mngr.AvatarDatas[SampleAvatarID], true));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdateAvatar();
        }

    }

    public void UpdateAvatar()
    {
        SampleAvatar.Attach(0, ArmsID);
        SampleAvatar.Attach(1, BootsID);
        SampleAvatar.Attach(2, GlovesID);
        SampleAvatar.Attach(3, HatsID);
        SampleAvatar.Attach(4, HeadsID);
        SampleAvatar.Attach(5, LegsID);
        SampleAvatar.Attach(6, PantsID);
        SampleAvatar.Attach(7, ShirtsID);
        SampleAvatar.Attach(8, TorsosID);
        SampleAvatar.Attach(9, RiflesID);
    }
}
