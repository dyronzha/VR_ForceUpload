using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboArmControl : MultiContolBase
{
    bool grib = false;
    Valve.VR.SteamVR_Action_Single squeezeAction;
    public RoboArmControl(Transform t, Transform look) :base(t, look) {

    }

    float armInitY, handInitY;
    Transform  body, arm, hand, grabObject;

    //Transform test;


    public void GiveInputAction(Valve.VR.SteamVR_Action_Single squeeze)
    {
        squeezeAction = squeeze;
    }

    // Start is called before the first frame update
    public override void Init()
    {
        body = transform.Find("Body");
        arm = body.Find("Arm");
        hand = arm.Find("Hand");
        armInitY = arm.localPosition.y;

       // test = GameObject.Find("Sphere").transform;
    }

    public override void Awake()
    {
        handInitY = rightHand.position.y;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {
       

        //Vector3 handDiff = new Vector3(test.position.x - HUDCamera.position.x, 0, test.position.z - HUDCamera.position.z).normalized;
        Vector3 handDiff = new Vector3(rightHand.position.x - HUDCamera.position.x, 0, rightHand.position.z - HUDCamera.position.z).normalized;
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.LookRotation(handDiff, Vector3.up), dt*5.0f);

        //float handOffset = test.position.y - handInitY;
        float handOffset = rightHand.position.y - handInitY;

        if (handOffset > 0.35f) handOffset = 0.35f;
        else if (handOffset < -0.55f) handOffset = -0.55f;
        arm.localPosition = new Vector3(arm.localPosition.x, handOffset + armInitY, arm.localPosition.z);

        Debug.Log(squeezeAction.GetAxis(Valve.VR.SteamVR_Input_Sources.RightHand));
        if ((Input.GetKeyDown(KeyCode.G) || squeezeAction.GetAxis(Valve.VR.SteamVR_Input_Sources.RightHand) > 0.8f) && !grib) {
            Collider[] hits = Physics.OverlapBox(hand.transform.position + new Vector3(0, -0.5f, 0), new Vector3(0.5f,0.5f,0.5f), hand.rotation,
                1 << LayerMask.NameToLayer("GribObject"));
            if (hits.Length > 0) {
                grabObject = hits[0].transform;
                grabObject.parent.parent = hand;
                grib = true;
            }
            Debug.Log(hits.Length);
        }
        else if (squeezeAction.GetAxis(Valve.VR.SteamVR_Input_Sources.RightHand) < 0.1f && grib){
            grib = false;

            grabObject.parent.parent = null;

        }
    }
}
