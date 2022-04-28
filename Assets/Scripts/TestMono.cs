using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AvatarSavingSystem.AvatarDataManager mngr = new AvatarSavingSystem.AvatarDataManager();

        mngr.SaveToJSON("Assets/SampleAvatar.json");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
