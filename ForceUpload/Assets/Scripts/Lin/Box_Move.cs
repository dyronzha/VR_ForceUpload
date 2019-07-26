using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Move : MonoBehaviour{
    public Vector3 Ini_Start;
    public Vector3 Ini_End;
    public float speed = 0.4f;
    float FirstSpeed;
    public TileMap _tilemap;
    int NextPosisWalkable = 1;  //1可行，2//不行，3輸送帶

    void Start(){
        FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
        speed = 0;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.A)) {
            Ini_Start = transform.position;
            Ini_End = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x,Ini_End.z);
            if (NextPosisWalkable == 1 ||NextPosisWalkable == 3) {
                _tilemap.SetTileOccupy(Ini_Start.x, Ini_Start.z, false);
                _tilemap.SetTileOccupy(Ini_End.x,Ini_End.z,true);
                speed = CalculateNewSpeed();
            }
        }

        if (Input.GetKeyDown(KeyCode.S)){
            Ini_Start = transform.position;
            Ini_End = new Vector3(transform.position.x, transform.position.y, transform.position.z-1);
            NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x, Ini_End.z);
            if (NextPosisWalkable == 1 || NextPosisWalkable == 3){
                _tilemap.SetTileOccupy(Ini_Start.x, Ini_Start.z, false);
                _tilemap.SetTileOccupy(Ini_End.x, Ini_End.z, true);
                speed = CalculateNewSpeed();
            }
        }

        if (speed != 0) {
            transform.position = Vector3.Lerp(transform.position, Ini_End, speed * Time.deltaTime);
            speed = CalculateNewSpeed();
        }
    }

    float CalculateNewSpeed(){
        float tmp_dis = Vector3.Distance(transform.position, Ini_End);

        if (tmp_dis == 0){
            return tmp_dis;
        }
        else return (FirstSpeed / tmp_dis);
    }

}
