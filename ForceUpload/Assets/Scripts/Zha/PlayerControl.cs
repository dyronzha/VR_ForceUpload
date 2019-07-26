using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControl : MonoBehaviour
{
    [Header("TransMind")]
    public float transDist = 10.0f;
    public SteamVR_Action_Boolean transAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
    public LayerMask transMask;

    bool isTeleportTouchDown = false, canTrans = false;
    Transform curTransHand, lineReticle;
    SteamVR_Input_Sources transHand;

    Transform leftHand, rightHand;
    LineRenderer transLineRender;


    // Start is called before the first frame update
    private void Awake()
    {
        leftHand = transform.GetChild(0);
        rightHand = transform.GetChild(1);
        transLineRender.enabled = false;
        lineReticle = transform.Find("Reticle");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TransDetect() {
        if (!isTeleportTouchDown)
        {
            if (transAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                transHand = SteamVR_Input_Sources.LeftHand;
                isTeleportTouchDown = true;
                curTransHand = leftHand;
            }
            else if (transAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                transHand = SteamVR_Input_Sources.RightHand;
                isTeleportTouchDown = true;
                curTransHand = rightHand;
            }
        }
        else {

            RaycastHit hit;
            Physics.Raycast(curTransHand.position, curTransHand.forward, out hit, transDist, transMask);
            transLineRender.positionCount = 2;
            transLineRender.SetPosition(0, curTransHand.position);
            transLineRender.SetPosition(1, curTransHand.position + transDist * curTransHand.forward);
            if (hit.transform != null)
            {
                canTrans = true;

            }
            else {

            }
            if (transAction.GetStateDown(transHand)) {
                if (transHand == SteamVR_Input_Sources.LeftHand)
                {


                }
                else {

                }
            }
        }
    }
   
}


//fade
//SteamVR_Fade.Start( Color.clear, 0 );
//SteamVR_Fade.Start(Color.black, currentFadeTime);


//stop tracking
//UnityEngine.XR.InputTracking.disablePositionalTracking = enabled;