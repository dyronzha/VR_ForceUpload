using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour{

    public Vector3 Ini_Start;
    public Vector3 Ini_End;
    public float speed = 0.4f;
    float FirstSpeed;
    public TileMap _tilemap;
    int NextPosisWalkable = 1;  //1可行，2//不行，3輸送帶
    bool AutoMoving = false;
    bool PlayerManualing = false;

    //推箱子用
    Ray ray_Dir;
    RaycastHit hit_Dir;

    void Start(){
        ray_Dir = new Ray(transform.position, transform.position);
        //Ini_Start = transform.position;
        //FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
    }


    void Update(){
        //Debug.Log("CurrentSpeed = " + speed);
        if (AutoMoving == false) {
            if (Input.GetKeyDown(KeyCode.J)){
                Ini_Start = transform.position;
                Ini_End = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
                FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
                NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x, Ini_End.z);
                if (NextPosisWalkable == 1 || NextPosisWalkable ==3){
                    speed = CalculateNewSpeed();
                    PlayerManualing = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.H)){
                Ini_Start = transform.position;
                Ini_End = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
                FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
                NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x, Ini_End.z);
                if (NextPosisWalkable == 1 || NextPosisWalkable == 3){
                    speed = CalculateNewSpeed();
                    PlayerManualing = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.U)){
                Ini_Start = transform.position;
                Ini_End = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
                FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
                NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x, Ini_End.z);
                if (NextPosisWalkable == 1 || NextPosisWalkable == 3){
                    speed = CalculateNewSpeed();
                    PlayerManualing = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.K)){
                Ini_Start = transform.position;
                Ini_End = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
                FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
                NextPosisWalkable = _tilemap.CheckTileWalkable(Ini_End.x, Ini_End.z);
                if (NextPosisWalkable == 1 || NextPosisWalkable == 3){
                    speed = CalculateNewSpeed();
                    PlayerManualing = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.W)){
                ray_Dir = new Ray(transform.position, new Vector3(0.0f, 0.0f, 0.6f));
                Debug.DrawRay(transform.position, new Vector3(0.0f, 0.0f, 0.6f));
                if (Physics.Raycast(ray_Dir, out hit_Dir, 0.6f)){
                    if (hit_Dir.transform.tag == "Stage3_Box") hit_Dir.transform.gameObject.GetComponent<Box_Move>().PushByPlayer(transform.position.x, transform.position.z);
                }
            }

            if (Input.GetKeyDown(KeyCode.A)){
                ray_Dir = new Ray(transform.position, new Vector3(-0.6f, 0.0f, 0.0f));
                Debug.DrawRay(transform.position, new Vector3(-0.6f, 0.0f, 0.0f));
                if (Physics.Raycast(ray_Dir, out hit_Dir, 0.6f)){
                    if (hit_Dir.transform.tag == "Stage3_Box"){
                        hit_Dir.transform.gameObject.GetComponent<Box_Move>().PushByPlayer(transform.position.x, transform.position.z);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.S)){
                ray_Dir = new Ray(transform.position, new Vector3(0.0f,0.0f, -0.6f));
                Debug.DrawRay(transform.position, new Vector3( 0.0f, 0.0f,-0.6f));
                if (Physics.Raycast(ray_Dir, out hit_Dir, 0.6f)){
                    if (hit_Dir.transform.tag == "Stage3_Box") hit_Dir.transform.gameObject.GetComponent<Box_Move>().PushByPlayer(transform.position.x, transform.position.z);
                }
            }

            if (Input.GetKeyDown(KeyCode.D)){
                ray_Dir = new Ray(transform.position, new Vector3(0.6f, 0.0f,0.0f));
                Debug.DrawRay(transform.position, new Vector3(0.6f, 0.0f, 0.0f));
                if (Physics.Raycast(ray_Dir, out hit_Dir, 0.6f)){
                    if (hit_Dir.transform.tag == "Stage3_Box") hit_Dir.transform.gameObject.GetComponent<Box_Move>().PushByPlayer(transform.position.x, transform.position.z);
                }
            }

            if (speed != 0 &&PlayerManualing == true){
                transform.position = Vector3.Lerp(transform.position, Ini_End, speed * Time.deltaTime);
                speed = CalculateNewSpeed();
            }

        }

        if (AutoMoving == true) {
            transform.position = Vector3.Lerp(transform.position, Ini_End, speed * Time.deltaTime);
            speed = CalculateNewSpeed();
        }

    }

    public void ForceConvey(float PosX,float PosZ,int Dir) {
        Ini_Start = transform.position;
        int CheckTimes = 0;
        int unit = 1;
        NextPosisWalkable = 1;
        switch (Dir) {
            case 3:
                Ini_End = new Vector3(6.0f, transform.position.y, transform.position.z);
                CheckTimes = Mathf.Abs(6 - (int)transform.position.x);
                for (int i = 0; i < CheckTimes && NextPosisWalkable == 1; i++){
                    NextPosisWalkable = _tilemap.CheckTileWalkable(PosX + 1.0f + i, PosZ);
                    if (NextPosisWalkable == 2)Ini_End = new Vector3(PosX + i, transform.position.y, PosZ);
                    else if (NextPosisWalkable == 3) Ini_End = new Vector3(PosX +1.0f + i, transform.position.y, PosZ);
                    unit = 1 + i;
                }
                break;
            case 6:
                Ini_End = new Vector3(transform.position.x, transform.position.y, 0.0f);
                CheckTimes = Mathf.Abs(0 - (int)transform.position.z);
                for (int i = 0; i < CheckTimes && NextPosisWalkable == 1; i++){
                    NextPosisWalkable = _tilemap.CheckTileWalkable(PosX, PosZ - 1.0f - i);
                    if (NextPosisWalkable == 2) Ini_End = new Vector3(PosX, transform.position.y, PosZ  - i );
                    else if (NextPosisWalkable == 3) Ini_End = new Vector3(PosX, transform.position.y, PosZ - 1.0f - i);
                    unit = 1 + i;
                }
                break;
            case 9:
                Ini_End = new Vector3(0.0f, transform.position.y, transform.position.z);
                CheckTimes = Mathf.Abs(0 - (int)transform.position.x);
                for (int i = 0; i < CheckTimes && NextPosisWalkable == 1; i++){
                    NextPosisWalkable = _tilemap.CheckTileWalkable(PosX - 1.0f - i, PosZ);
                    if (NextPosisWalkable == 2) Ini_End = new Vector3(PosX - i, transform.position.y, PosZ);
                    else if (NextPosisWalkable == 3) Ini_End = new Vector3(PosX - 1.0f - i, transform.position.y, PosZ);
                    unit = 1 + i;
                }
                break;
            case 12:
                Ini_End = new Vector3(transform.position.x, transform.position.y, 6.0f);
                CheckTimes = Mathf.Abs(6 - (int)transform.position.z);
                for (int i = 0; i < CheckTimes && NextPosisWalkable == 1; i++){
                    NextPosisWalkable = _tilemap.CheckTileWalkable(PosX, PosZ + 1.0f + i);
                    if (NextPosisWalkable == 2) Ini_End = new Vector3(PosX, transform.position.y, PosZ  + i );
                    else if (NextPosisWalkable == 3) Ini_End = new Vector3(PosX, transform.position.y, PosZ + 1.0f + i);
                    unit = 1 + i;
                }
                break;
        }
        speed = 1.5f;
        FirstSpeed = (Vector3.Distance(Ini_Start, Ini_End) * speed) / unit;
        speed = CalculateNewSpeed();
        AutoMoving = true;
    }

    float CalculateNewSpeed(){
        float tmp_dis = Vector3.Distance(transform.position, Ini_End);
        if (tmp_dis == 0){
            PlayerManualing = false;
            AutoMoving = false;
            speed = 0.4f;
            return speed;
        }
        else return (FirstSpeed / tmp_dis);
    }

}
