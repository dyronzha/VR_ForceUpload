using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Dictionary<string, MultiContolBase> multiControlsDic = new Dictionary<string, MultiContolBase>();

    // Start is called before the first frame update
    private void Awake()
    {
        Transform control = GameObject.Find("SpiralElevator").transform;
        SpiralElevatorControl spiralElevatorControl = new SpiralElevatorControl(control, control.Find("Pos").position);
        multiControlsDic.Add(control.name, spiralElevatorControl);

        control = GameObject.Find("RoboArm").transform;
        RoboArmControl roboArmControl = new RoboArmControl(control, control.Find("Pos").position);
        multiControlsDic.Add(control.name, roboArmControl);

        control = GameObject.Find("Drone").transform;
        DroneControl droneControl = new DroneControl(control, control.Find("Pos").position);
        multiControlsDic.Add(control.name, droneControl);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log(LookUpMultiControl("RoboArm"));
        }
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
