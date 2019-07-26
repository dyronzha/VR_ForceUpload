using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTest : MonoBehaviour
{
    bool hasThree = false;
    int cirNum = 0;
    float circleTime = .0f;
    Vector2 lastVector, curVector;
    Vector2[] recordPoint = new Vector2[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (circleTime < 0.1f)
        {
            circleTime += Time.deltaTime;
        }
        else
        {
            circleTime = .0f;
            recordPoint[cirNum] = new Vector2(transform.position.x , transform.position.z);
            if (hasThree || cirNum > 0)
            {
                if (hasThree || cirNum > 1) lastVector = curVector;
                curVector = recordPoint[cirNum] - recordPoint[((cirNum) <= 0 ? 2 : (cirNum - 1))];
            }
            cirNum++;

            if (cirNum > 2)
            {
                hasThree = true;
                cirNum = 0;
            }

            if (hasThree)
            {
                float degree = Vector2.Angle(lastVector, curVector);
                float cross = lastVector.x * curVector.y - lastVector.y * curVector.x;
            }
        }
    }
}
