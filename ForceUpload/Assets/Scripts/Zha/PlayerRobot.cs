using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobot
{

    public Transform transform;
    public GameObject gameObject;

    float deltaTime = .0f;
    float gravity = 5.0f;
    float fallSpeed = .0f;
    Vector3 nextPos;

    // Start is called before the first frame update
    public void Init(Transform t)
    {
        transform = t;
        gameObject = t.gameObject;
    }

    // Update is called once per frame
    public void Update(float dt)
    {
        deltaTime = dt;
        Fall();
    }
    void Fall() {
        fallSpeed += gravity * deltaTime;
        nextPos = transform.position - deltaTime * new Vector3(0,fallSpeed,0);
        RaycastHit hit;
        if (!Physics.Linecast(transform.position, nextPos, out hit, 1 << LayerMask.NameToLayer("Ground")))
        {
            transform.position = nextPos;
        }
        else {
            transform.position = hit.transform.position + new Vector3(0,0.3f,0);
        }
    }
    void DetectDie() {
        if (Physics.Raycast(transform.position, new Vector3(0, -1.0f, 0), 1 << LayerMask.NameToLayer("DieArea")) ) {

        }
    }
}
