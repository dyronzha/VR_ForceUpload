using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControl : MonoBehaviour
{
    bool roboMod = true;
    float deltaTime;

    [Header("TransMind")]
    public float transDist = 10.0f;
    public SteamVR_Action_Boolean transAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
    public LayerMask otherControlMask, oringinMask;

    [Header("OtherControl")]
    public SteamVR_Action_Boolean GribAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    bool isTransTouchDown = false, goTrans = false;
    float transTime = .0f;
    Transform HUDCamera, curTransHand, lineReticle;
    LayerMask transMask;

    Vector3 cameraOffset;

    MultiContolBase targetControl;

    SteamVR_Input_Sources transHand;

    public Transform leftHand, rightHand;
    LineRenderer transLineRender;


    // Start is called before the first frame update
    private void Awake()
    {
        HUDCamera = transform.GetChild(2);
        leftHand = transform.GetChild(0);
        rightHand = transform.GetChild(1);
        transLineRender = GetComponent<LineRenderer>();
        transLineRender.enabled = false;
        lineReticle = transform.Find("Reticle");
        transMask = otherControlMask;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) GameManager.Instance.LookUpMultiControl("SpiralElevator");
        deltaTime = Time.deltaTime; 
        if (!goTrans) TransDetect();
        else GoTrans();

        if (!roboMod) {
            targetControl.Update(deltaTime);
        }

    }

    private void LateUpdate()
    {
        if (!roboMod) {
            transform.position = Vector3.Lerp(transform.position, transform.position - (HUDCamera.localPosition - cameraOffset), Time.deltaTime*10.0f);
        }
    }

    void TransDetect() {
        if (!isTransTouchDown)
        {
            if (transAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                transHand = SteamVR_Input_Sources.LeftHand;
                isTransTouchDown = true;
                curTransHand = leftHand;
                transLineRender.enabled = true;
            }
            else if (transAction.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                transHand = SteamVR_Input_Sources.RightHand;
                isTransTouchDown = true;
                curTransHand = rightHand;
                transLineRender.enabled = true;
            }
        }
        else {
            transLineRender.positionCount = 2;
            transLineRender.SetPosition(0, curTransHand.position);
            transLineRender.SetPosition(1, curTransHand.position + transDist * curTransHand.forward);
            if (transAction.GetStateUp(transHand))
            {
                isTransTouchDown = false;
                transLineRender.enabled = false;


                RaycastHit hit;
                Physics.Raycast(curTransHand.position, curTransHand.forward, out hit, transDist, transMask);

                if (hit.transform != null)
                {
                    Debug.Log(hit.transform.name);
                    targetControl = GameManager.Instance.LookUpMultiControl(hit.transform.parent.name);

                    goTrans = true;
                    SteamVR_Fade.Start(Color.clear, 0);
                    SteamVR_Fade.Start(Color.black, 0.8f);
                    if (roboMod)
                    {
                        //HUDCamera.GetComponent<Camera>().enabled = false;
                        //UnityEngine.XR.InputTracking.disablePositionalTracking = true;
                        cameraOffset = HUDCamera.localPosition;
                        transMask = oringinMask;
                    }
                    else
                    {
                        //HUDCamera.GetComponent<Camera>().enabled = enabled;
                        //UnityEngine.XR.InputTracking.disablePositionalTracking = false;
                        transMask = otherControlMask;
                    }
                    roboMod = !roboMod;
                }


               

            }


          
        }
    }
    void GoTrans() {
        transTime += Time.deltaTime;
        if (transTime > 0.5f) {
            transTime = .0f;
            transform.position = targetControl.whereLook.position - HUDCamera.transform.localPosition;
            transform.rotation = targetControl.whereLook.rotation;
            goTrans = false;
        }
    }
}


//fade
//SteamVR_Fade.Start( Color.clear, 0 );
//SteamVR_Fade.Start(Color.black, currentFadeTime);


//stop tracking
//UnityEngine.XR.InputTracking.disablePositionalTracking = enabled;