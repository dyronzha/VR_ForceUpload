using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralElevatorControl : MultiContolBase
{
    bool inCircle = false;
    public SpiralElevatorControl(Transform t, Transform look) : base(t, look)
    {

    }

    Transform spiral;
    Transform[] platforms;
    float maxRotateSpeed = 5.0f, rotateSpeed = .0f;

    bool hasThree = false;
    int cirNum = 0;
    float circleTime = .0f;
    float lastCross = 0f;
    float degree, cross;
    Vector2 lastVector, curVector;
    Vector2[] recordPoint = new Vector2[3];

    Transform test;

    // Start is called before the first frame update
    public override void Init()
    {
        spiral = transform.Find("Spiral");
        Transform t = transform.Find("Platforms");
        platforms = new Transform[t.childCount];
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i] = t.GetChild(i);
        }

        test = GameObject.Find("Sphere").transform;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {
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
                Debug.Log(degree + "   " + cross);
                if (Mathf.Abs(cross) > 0.1f) cross = -Mathf.Sign(cross);
                else cross = .0f;
 

            }
        }


        if (degree > 50.0f && degree < 160.0f)
        {
            rotateSpeed += Time.deltaTime;
            if (rotateSpeed > maxRotateSpeed) rotateSpeed = maxRotateSpeed;
            Debug.Log("speed  plus " + rotateSpeed);
            for (int i = 0; i < platforms.Length; i++)
            {
                platforms[i].position += cross * dt * new Vector3(0, rotateSpeed, 0);
                platforms[i].Rotate(cross * dt * new Vector3(0, rotateSpeed * 50.0f));
                if (platforms[i].localPosition.y > 1.5f)
                {
                    platforms[i].localPosition = new Vector3(0, -2.0f, 0);
                }
                else if (platforms[i].localPosition.y < -2.5f)
                {
                    platforms[i].localPosition = new Vector3(0, 1.3f, 0);
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
