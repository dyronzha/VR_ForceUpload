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

        Transform control = GameObject.Find("SpiralElevator").transform;
        SpiralElevatorControl spiralElevatorControl = new SpiralElevatorControl(control, control.Find("Pos"));
        spiralElevatorControl.SetHands(player.rightHand,player.leftHand);
        multiControlsDic.Add(control.name, spiralElevatorControl);

        control = GameObject.Find("RoboArm").transform;
        RoboArmControl roboArmControl = new RoboArmControl(control, control.Find("Pos"));
        multiControlsDic.Add(control.name, roboArmControl);

        control = GameObject.Find("Drone").transform;
        DroneControl droneControl = new DroneControl(control, control.Find("Pos"));
        multiControlsDic.Add(control.name, droneControl);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public MultiContolBase LookUpMultiControl(string name) {
        if (multiControlsDic.ContainsKey(name))
        {
            Debug.Log(multiControlsDic[name]);
            return multiControlsDic[name];
        }
        else {
            Debug.Log("can't find " + name);
            return null;
        }
    }

}
