using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboArmControl : MultiContolBase
{
    Valve.VR.SteamVR_Action_Boolean GribAction;
    public RoboArmControl(Transform t, Transform look) :base(t, look) {

    }

    public void GiveInputAction(Valve.VR.SteamVR_Action_Boolean grib)
    {
        GribAction = grib;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
