using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour{

    public bool On_Occupy = false;
    public int Conveyor_Dir = 0;
    public Transform HUDPlayer;

    void Start(){
        HUDPlayer = GameObject.Find("Player").transform;
        if (transform.position.x == 4.0f && transform.position.z == 0.0f) Conveyor_Dir = 12;
        else if (transform.position.x == 5.0f && transform.position.z == 1.0f) Conveyor_Dir = 3;
        else if (transform.position.x == 5.0f && transform.position.z == 3.0f) Conveyor_Dir = 6;
    }


    void Update(){
        if (Mathf.Abs(transform.position.x - HUDPlayer.position.x) < 0.05f && Mathf.Abs(transform.position.z - HUDPlayer.position.z) < 0.05f && Conveyor_Dir!=0) {
            float ConveyPosX = transform.position.x;
            float ConveyPosZ= transform.position.z;
            HUDPlayer.gameObject.GetComponent<Player_Move>().ForceConvey(ConveyPosX,ConveyPosZ,Conveyor_Dir);
        }


    }

}
