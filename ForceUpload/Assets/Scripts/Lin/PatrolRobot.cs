using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRobot : MonoBehaviour{

    public Vector3 Ini_Start;
    public Vector3 Ini_End;
    Vector3 tmp_switch;

    float speed = 0.4f;
    float FirstSpeed;

    void Start(){
        FirstSpeed = Vector3.Distance(Ini_Start, Ini_End) * speed;
    }

    void Update(){
        transform.position = Vector3.Lerp(transform.position, Ini_End, speed * Time.deltaTime);
        speed = CalculateNewSpeed();
    }

    float CalculateNewSpeed() {
        float tmp_dis = Vector3.Distance(transform.position, Ini_End);

        if (tmp_dis == 0){
            tmp_switch = Ini_Start;
            Ini_Start = Ini_End;
            Ini_End = tmp_switch;
            tmp_dis = Vector3.Distance(transform.position, Ini_End);
        }
        return (FirstSpeed / tmp_dis);
    }

}
