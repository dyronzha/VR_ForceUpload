using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRobot : MonoBehaviour{

    public Vector3 Ini_Start;
    public Vector3 Ini_End;
    Vector3 tmp_switch;
    float speed = 0.4f;
    float FirstSpeed;
    TileMap _tilemap;
    public int RouteType;

    //偵測箱子&玩家用
    Ray ray_Dir;
    RaycastHit hit_Dir;
    Vector3 Patrol_Dir;

    void Start(){
        FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
        if (RouteType == 1){
            Patrol_Dir =  new Vector3(0.0f, 0.0f, -3.0f);
            ray_Dir = new Ray(transform.position, Patrol_Dir);
            Debug.DrawRay(transform.position, Patrol_Dir);
        }
        else {
            Patrol_Dir = new Vector3(3.0f, 0.0f, 0.0f);
            ray_Dir = new Ray(transform.position, Patrol_Dir);
            Debug.DrawRay(transform.position, Patrol_Dir);
        }
    }

    void Update(){
        ray_Dir = new Ray(transform.position, Patrol_Dir);
        Debug.DrawRay(ray_Dir.origin, ray_Dir.direction);
        //偵測前方是否有箱子
        if (Physics.Raycast(ray_Dir, out hit_Dir, 3.0f)) {
            if (hit_Dir.transform.tag == "Stage3_Box"){
                if (RouteType == 1){
                    if (transform.position.z >= hit_Dir.transform.position.z) Ini_End = hit_Dir.transform.localPosition + new Vector3(-1.0f, 0.5f, 0.0f);
                    else Ini_End = hit_Dir.transform.localPosition + new Vector3(1.0f, 0.5f, 0.0f);
                }
                else if(RouteType == 2 && hit_Dir.transform.name == "Box_B"){
                    if (transform.position.x >= hit_Dir.transform.position.x) Ini_End = hit_Dir.transform.localPosition + new Vector3(0.0f, 0.5f, 1.0f);
                    else Ini_End = hit_Dir.transform.localPosition + new Vector3(0.0f, 0.5f, -1.0f);
                }
            }
            else if (hit_Dir.transform.tag == "HUDPlayer") Debug.Log("Caughted");
        }


        transform.localPosition = Vector3.Lerp(transform.localPosition, Ini_End, speed * Time.deltaTime);
        speed = CalculateNewSpeed();
    }

    float CalculateNewSpeed() {
        float tmp_dis = Vector3.Distance(transform.localPosition, Ini_End);

        if (tmp_dis == 0){
            tmp_switch = Ini_Start;
            Ini_Start = Ini_End;
            Ini_End = tmp_switch;
            tmp_dis = Vector3.Distance(transform.localPosition, Ini_End);
            if(RouteType ==1)Patrol_Dir.z = Patrol_Dir.z * -1.0f;
            else Patrol_Dir.x = Patrol_Dir.x * -1.0f;
        }
        return (FirstSpeed / tmp_dis);
    }

}
