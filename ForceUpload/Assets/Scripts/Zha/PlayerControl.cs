using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControl : MonoBehaviour
{
    bool roboMod = true;
    float deltaTime;
    Transform playerRobot;

    [Header("TransMind")]
    public float transDist = 10.0f;
    public SteamVR_Action_Boolean transAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
    public LayerMask otherControlMask, oringinMask;

    [Header("OtherControl")]
    public SteamVR_Action_Boolean GribAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public SteamVR_Action_Single SqueezeAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");

    bool isTransTouchDown = false, goTrans = false;
    float transTime = .0f;
    Transform curTransHand, lineReticle;
    LayerMask transMask;

    Vector3 cameraOffset;

    MultiContolBase targetControl;

    SteamVR_Input_Sources transHand;

    public Transform HUDCamera, leftHand, rightHand;
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

    public void SetTargetControl(Transform player, MultiContolBase control) {
        playerRobot = player;
        targetControl = control;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(SqueezeAction.GetAxis(SteamVR_Input_Sources.RightHand));

        deltaTime = Time.deltaTime; 
        if (!goTrans) TransDetect();
        else GoTrans();

        targetControl.Update(deltaTime);

    }

    private void LateUpdate()
    {
        //if (!roboMod) {
        //    transform.position = (HUDCamera.position - cameraOffset); //Vector3.Lerp(transform.position, transform.position - (HUDCamera.localPosition - cameraOffset), Time.deltaTime*10.0f);
        //}
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


            if (Input.GetKeyDown(KeyCode.Z)) {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("SpiralElevator");
                playerRobot.parent = null;
                playerRobot.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("RoboArm");
                playerRobot.parent = null;
                playerRobot.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("Drone");
                playerRobot.parent = null;
                playerRobot.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("PlayerRobot");
                playerRobot.gameObject.SetActive(false);
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
                    SteamVR_Fade.Start(Color.black, 2.0f);
                    if (roboMod)
                    {
                        //HUDCamera.GetComponent<Camera>().enabled = false;
                        //UnityEngine.XR.InputTracking.disablePositionalTracking = true;
                        cameraOffset = HUDCamera.localPosition;
                        transMask = oringinMask;
                        playerRobot.parent = null;
                        playerRobot.gameObject.SetActive(true);

                    }
                    else
                    {
                        //HUDCamera.GetComponent<Camera>().enabled = enabled;
                        //UnityEngine.XR.InputTracking.disablePositionalTracking = false;
                        transMask = otherControlMask;
                        playerRobot.gameObject.SetActive(false);
                        if (transform.parent != null) transform.parent = null;

                    }
                    
                }
            }
        }
    }
    void GoTrans() {
        transTime += Time.deltaTime;
        if (transTime > 0.5f) {
            transTime = .0f;
            transform.position = targetControl.whereLook.position - HUDCamera.transform.localPosition;
            Debug.Log(targetControl.transform.name);
            transform.rotation = targetControl.whereLook.rotation;
            goTrans = false;
            roboMod = !roboMod;
            targetControl.Awake();
            if(roboMod) playerRobot.parent = transform;
        }
    }
}


//fade
//SteamVR_Fade.Start( Color.clear, 0 );
//SteamVR_Fade.Start(Color.black, currentFadeTime);


//stop tracking
//UnityEngine.XR.InputTracking.disablePositionalTracking = enabled;