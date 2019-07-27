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
    public Transform HUDPlayer;
    int Push_Dir = 0;  //0為重置狀態，3向右，6向下，9向左，12向上
    public Vector3 GoalPos;
    bool Goal = false;

    void Start(){
        _tilemap.SetTileOccupy(transform.position.x, transform.position.z, true);
        FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
        speed = 0;
    }

    void Update(){
        if (speed != 0) {
            transform.position = Vector3.Lerp(transform.position, Ini_End, speed * Time.deltaTime);
            speed = CalculateNewSpeed();
        }
        if (transform.position.x == GoalPos.x && transform.position.z == GoalPos.z) Goal = true;
    }

    float CalculateNewSpeed(){
        float tmp_dis = Vector3.Distance(transform.position, Ini_End);

        if (tmp_dis == 0){
            return tmp_dis;
        }
        else return (FirstSpeed / tmp_dis);
    }

    public void PushByPlayer(float HUDPosX,float HUDPosZ) {
        if (Goal == false) {
            //決定方向
            if (transform.position.x > HUDPosX) Push_Dir = 3;
            else if (transform.position.x < HUDPosX) Push_Dir = 9;
            else if (transform.position.z > HUDPosX) Push_Dir = 12;
            else if (transform.position.z < HUDPosX) Push_Dir = 6;

            //根據方向進行移動
            Ini_Start = transform.position;
            switch (Push_Dir){
                case 3:
                    Ini_End = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                    break;
                case 6:
                    Ini_End = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                    break;
                case 9:
                    Ini_End = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                    break;
                case 12:
                    Ini_End = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                    break;
            }
            NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x, Ini_End.z);
            if (NextPosisWalkable == 1 || NextPosisWalkable == 3){
                _tilemap.SetTileOccupy(Ini_Start.x, Ini_Start.z, false);
                _tilemap.SetTileOccupy(Ini_End.x, Ini_End.z, true);
                speed = CalculateNewSpeed();
            }
        }

    }


}
