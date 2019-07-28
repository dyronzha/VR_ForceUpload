using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobot
{
    int level = 0;

    public Transform transform;
    public GameObject gameObject;

    public bool canFall = false;
    bool onGround = true;
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

        switch (PlayerControl.level) {
            case 0:
                Vector3 curPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Collider[] hits = Physics.OverlapBox(curPos, new Vector3(0.3f, 0.3f, 0.3f), Quaternion.identity, 1 << LayerMask.NameToLayer("Platform"));
                if (hits != null && hits.Length > 0)
                {
                    transform.parent = hits[0].transform;
                }
                break;

            case 1:
                if (canFall) Fall();
                break;
        }

        

     
    }
    void Fall() {
        if (onGround && !Physics.Raycast(transform.position, new Vector3(0,-1,0), 0.5f, 1 << LayerMask.NameToLayer("Ground"))) {
            onGround = false;
        }
        else if (!onGround) {
            fallSpeed += gravity * deltaTime;
            nextPos = transform.position - deltaTime * new Vector3(0, fallSpeed, 0);
            RaycastHit hit;
            if (!Physics.Linecast(transform.position, nextPos, out hit, 1 << LayerMask.NameToLayer("Ground")))
            {
                transform.position = nextPos;
            }
            else {
                onGround = true;
                transform.position = hit.point + new Vector3(0, 0.05f, 0);
            } 
        }

    }
    void DetectDie() {
        if (Physics.Raycast(transform.position, new Vector3(0, -1.0f, 0), 1 << LayerMask.NameToLayer("DieArea")) ) {

        }
    }
}
