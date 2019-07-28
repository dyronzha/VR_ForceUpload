using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerControl : MonoBehaviour
{
    bool roboMod = true;
    float deltaTime;
    //Transform playerRobot;

    [Header("TransMind")]
    public float transDist = 500.0f;
    public SteamVR_Action_Boolean transAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
    public LayerMask otherControlMask, oringinMask;

    [Header("OtherControl")]
    public SteamVR_Action_Boolean GribAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");
    public SteamVR_Action_Single SqueezeAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");

    bool isTransTouchDown = false, goTrans = false;
    float transTime = .0f;
    Transform curTransHand, lineReticle;
    LayerMask transMask;

    Vector3 cameraOffset, cameraFixPos;

    MultiContolBase targetControl;

    SteamVR_Input_Sources transHand;

    public Transform HUDCamera, leftHand, rightHand;
    LineRenderer transLineRender;

    bool goBlcak = false;
    UnityEngine.UI.Image blackOut;

    PlayerRobot playerRobot;

    public static int level = 0;
    bool nextLevel;
    float nextLevelTime;
    Transform[] levelPoints;


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

        blackOut = HUDCamera.GetChild(0).Find("BlackOut").GetComponent<UnityEngine.UI.Image>();

        Transform points = GameObject.Find("GameManager").transform.Find("NextLevelPoint");
        levelPoints = new Transform[points.childCount];
        for (int i = 0; i < levelPoints.Length; i++) {
            levelPoints[i] = points.GetChild(i);
        }
        
    }
    void Start()
    {
        
    }

    public void SetTargetControl(Transform player, MultiContolBase control) {
        playerRobot = new PlayerRobot();
        playerRobot.Init(player);
        targetControl = control;
        //player.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(SqueezeAction.GetAxis(SteamVR_Input_Sources.RightHand));

        deltaTime = Time.deltaTime;
        if (!goTrans) {
            targetControl.Update(deltaTime);
            TransDetect();
            if (!roboMod) playerRobot.Update(Time.deltaTime);
            else {
                if (!nextLevel)
                {
                    Vector3 curPos = new Vector3(HUDCamera.position.x, transform.position.y, HUDCamera.position.z);
                    Debug.Log("next    "  + curPos);
                    Collider[] hits = Physics.OverlapBox(curPos, new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity, 1 << LayerMask.NameToLayer("NextLevel"));
                    if (hits != null && hits.Length > 0)
                    {
                        nextLevel = true;
                        blackOut.enabled = true;
                        transform.position = levelPoints[level].position;
                        transform.rotation = levelPoints[level].rotation;
                        PlayerControl.level++;
                    }
                }
                else
                {
                    nextLevelTime += deltaTime * 2.0f;
                    blackOut.color = new Color(0, 0, 0, 1.0f - nextLevelTime);
                    if (nextLevelTime > 0.95f)
                    {
                        nextLevelTime = .0f;
                        nextLevel = false;
                        blackOut.color = new Color(0, 0, 0, 1);
                        blackOut.enabled = false;
                    }
                }
            }
           
        } 
        else GoTrans();

        

    }

    private void LateUpdate()
    {
        if (!roboMod)
        {
            if (targetControl is DroneControl) {
                cameraFixPos = targetControl.GetWhereLook().position - HUDCamera.transform.localPosition;
            }

            transform.position = cameraFixPos + (cameraOffset - HUDCamera.localPosition); //Vector3.Lerp(transform.position, transform.position - (HUDCamera.localPosition - cameraOffset), Time.deltaTime*10.0f);
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


            if (Input.GetKeyDown(KeyCode.Z)) {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("SpiralElevator");
                playerRobot.transform.parent = null;
                playerRobot.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("RoboArm");
                playerRobot.transform.parent = null;
                playerRobot.gameObject.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("Drone");
                playerRobot.transform.parent = null;
                playerRobot.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                goTrans = true;
                cameraOffset = HUDCamera.localPosition;
                targetControl = GameManager.Instance.LookUpMultiControl("PlayerRobot");
                playerRobot.transform.gameObject.SetActive(false);
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
                    blackOut.enabled = true;
                    //SteamVR_Fade.Start(Color.clear, 0);
                    //SteamVR_Fade.Start(Color.black, 2.0f);
                    if (roboMod)
                    {
                        //HUDCamera.GetComponent<Camera>().enabled = false;
                        //UnityEngine.XR.InputTracking.disablePositionalTracking = true;

                        transMask = oringinMask;
                        playerRobot.transform.localPosition = new Vector3(HUDCamera.localPosition.x, 0, HUDCamera.localPosition.z);
                        playerRobot.transform.rotation = Quaternion.LookRotation(new Vector3(HUDCamera.forward.x, 0, HUDCamera.forward.z), Vector3.up);
                        //playerRobot.transform.parent = null;

                        playerRobot.gameObject.SetActive(true);
                        if (targetControl is RoboArmControl) playerRobot.canFall = true;
                    }
                    else
                    {
                        //HUDCamera.GetComponent<Camera>().enabled = enabled;
                        //UnityEngine.XR.InputTracking.disablePositionalTracking = false;
                        transMask = otherControlMask;
                        playerRobot.transform.gameObject.SetActive(false);
                        if (targetControl is RoboArmControl) playerRobot.canFall = false;
                    }
                    
                }
            }
        }
    }
    void GoTrans() {
        transTime += Time.deltaTime;

        if (transTime > 0.5f)
        {
            if (!goBlcak)
            {
                goBlcak = true;
                blackOut.enabled = false;
                //blackOut.color = new Color(0, 0, 0, 1);

                roboMod = !roboMod;

                if (!roboMod)playerRobot.transform.parent = null;

                
                transform.position = new Vector3(targetControl.GetWhereLook().position.x - HUDCamera.transform.localPosition.x, targetControl.GetWhereLook().position.y, targetControl.GetWhereLook().position.z - HUDCamera.transform.localPosition.z);
                Debug.Log(targetControl.transform.name);
                transform.rotation = targetControl.GetWhereLook().rotation;

                cameraOffset = HUDCamera.localPosition;
                cameraFixPos = transform.position;



                targetControl.Awake();
                if (roboMod) playerRobot.transform.parent = transform;
            }
            else {
                blackOut.color = new Color(0, 0, 0, 1.0f  - transTime);
                if (transTime >= 0.95f) {
                    blackOut.color = new Color(0, 0, 0, 1);
                    blackOut.enabled = false;
                    goBlcak = false;
                    goTrans = false;
                    transTime = .0f;
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