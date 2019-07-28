using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {
        get {
            return instance;
        }
    }

    PlayerControl player;

    Dictionary<string, MultiContolBase> multiControlsDic = new Dictionary<string, MultiContolBase>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        player = GameObject.Find("[CameraRig]").GetComponent<PlayerControl>();

       

    }
    void Start()
    {
        Transform control = GameObject.Find("PlayerRobot").transform;
        MultiContolBase playerRobot = new MultiContolBase(player.transform, control.Find("Pos"));
        player.SetTargetControl(control, playerRobot);
        playerRobot.Init();
        playerRobot.SetCameraHands(player.HUDCamera, player.rightHand, player.leftHand);
        multiControlsDic.Add(control.name, playerRobot);

        control = GameObject.Find("SpiralElevator").transform;
        SpiralElevatorControl spiralElevatorControl = new SpiralElevatorControl(control, control.Find("Pos"));
        spiralElevatorControl.Init();
        spiralElevatorControl.SetCameraHands(player.HUDCamera, player.leftHand, player.rightHand);
        multiControlsDic.Add(control.name, spiralElevatorControl);

        control = GameObject.Find("RoboArm").transform;
        RoboArmControl roboArmControl = new RoboArmControl(control, control.Find("Pos"));
        roboArmControl.Init();
        roboArmControl.SetCameraHands(player.HUDCamera, player.leftHand, player.rightHand);
        roboArmControl.GiveInputAction(player.SqueezeAction);
        multiControlsDic.Add(control.name, roboArmControl);

        control = GameObject.Find("Drone").transform;
        DroneControl droneControl = new DroneControl(control, control.Find("Pos"));
        droneControl.Init();
        droneControl.SetCameraHands(player.HUDCamera, player.leftHand, player.rightHand);
        droneControl.GiveInputAction(player.SqueezeAction);
        multiControlsDic.Add(control.name, droneControl);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public MultiContolBase LookUpMultiControl(string name) {
        if (multiControlsDic.ContainsKey(name))
        {
            return multiControlsDic[name];
        }
        else {
            Debug.Log("can't find " + name);
            return null;
        }
    }

}
