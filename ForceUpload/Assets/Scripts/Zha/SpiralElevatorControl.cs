using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralElevatorControl : MultiContolBase
{
    int transNum = 0;
    Transform whereLook2;

    public SpiralElevatorControl(Transform t, Transform look) : base(t, look)
    {

    }

    Transform spiral;
    Transform[] platforms;
    float maxRotateSpeed = 1.0f, rotateSpeed = .0f;

    bool hasThree = false;
    int cirNum = 0;
    float circleTime = .0f;
    float lastCross = 0f;
    float degree, cross;
    Vector2 lastVector, curVector;
    Vector2[] recordPoint = new Vector2[3];

    Transform player;
    Collider[] panel = new Collider[2];
    //Transform test;

    // Start is called before the first frame update
    public override void Init()
    {
        spiral = transform.Find("Spiral");
        Transform t = transform.Find("Platforms");
        platforms = new Transform[t.childCount];
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i] = t.GetChild(i);
        }
        whereLook2 = transform.Find("Pos2");

        player = GameObject.Find("PlayerRobot").transform;
        player.gameObject.SetActive(false);

        panel[0] = transform.Find("ControlPanel").GetComponent<Collider>() ;
        panel[1] = transform.Find("ControlPanel1").GetComponent<Collider>();
        //test = GameObject.Find("Sphere").transform;
    }

    public override Transform GetWhereLook()
    {
        if (transNum == 0) return whereLook;
        else return whereLook2;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {
        Debug.Log(HUDCamera.position.y);
        if (player.position.y > 5.0f && transNum == 0)
        {
            transNum = 1;
            panel[0].enabled = false;
            panel[1].enabled = true;
        }
        else if (player.position.y < 4.5f && transNum == 1) {
            transNum = 0;
            panel[1].enabled = false;
            panel[0].enabled = true;
        } 
        if (circleTime < 0.1f)
        {
            circleTime += dt;
        }
        else
        {
            circleTime = .0f;

            Vector2 handPos = new Vector2(rightHand.transform.position.x, rightHand.transform.position.z);
            //Debug.Log(handPos);
            //recordPoint[cirNum] = new Vector2(test.position.x, test.position.z);


            if (hasThree || cirNum > 0)
            {
                //Debug.Log(Vector2.SqrMagnitude(handPos - recordPoint[((cirNum) <= 0 ? 2 : (cirNum - 1))]));

                if (Vector2.SqrMagnitude(handPos - recordPoint[((cirNum) <= 0 ? 2 : (cirNum - 1))]) > 0.03f) {
                    recordPoint[cirNum] = handPos;
                }
                else recordPoint[cirNum] = recordPoint[((cirNum) <= 0 ? 2 : (cirNum - 1))];

                if (hasThree || cirNum > 1) lastVector = curVector;
                curVector = 10.0f*(recordPoint[cirNum] - recordPoint[((cirNum) <= 0 ? 2 : (cirNum - 1))]).normalized;
                //Debug.Log(curVector);
            }
            else recordPoint[cirNum] = handPos;

            cirNum++;

            //if (cirNum > 2)
            //{
            //    hasThree = true;
            //    cirNum = 0;
            //}

            if (cirNum > 2)
            {
                hasThree = true;
                cirNum = 0;

                degree = Vector2.Angle(lastVector, curVector);
                cross = lastVector.x * curVector.y - lastVector.y * curVector.x;
                //Debug.Log(degree + "   " + cross);
                if (Mathf.Abs(cross) > 0.1f) cross = -Mathf.Sign(cross);
                else cross = .0f;
 

            }
        }


        if (degree > 50.0f && degree < 160.0f)
        {
            rotateSpeed += Time.deltaTime;
            if (rotateSpeed > maxRotateSpeed) rotateSpeed = maxRotateSpeed;
            //Debug.Log("speed  plus " + rotateSpeed);
            spiral.Rotate(cross * dt * new Vector3(0, 0, maxRotateSpeed * 150.0f));

            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].localPosition += cross * dt * new Vector3(0, maxRotateSpeed, 0);
                platforms[i].Rotate(cross * dt * new Vector3(0, 0, maxRotateSpeed * 150.0f));
                if (platforms[i].localPosition.y > 9.0f)
                {
                    platforms[i].localPosition = new Vector3(0, -0.7f, 0);
                }
                else if (platforms[i].localPosition.y < -0.8f)
                {
                    platforms[i].localPosition = new Vector3(0, 8.9f, 0);
                }
            }

            if (Mathf.Abs(cross - lastCross) > 0.5f)
            {
                //Debug.Log("reverse   " + Mathf.Abs(cross - lastCross));
                rotateSpeed = 0f;
                lastCross = cross;
            }
        }
        else
        {

            rotateSpeed -= dt;
            if (rotateSpeed < 3.0f) rotateSpeed = .0f;
            //Debug.Log("speed  minus " + rotateSpeed);

        }
    }
}
