using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRobot
{
    int level = 0;

    bool die;

    public Transform transform;
    public GameObject gameObject;

    public bool canFall = true;
    bool onGround = true;
    float deltaTime = .0f;
    float gravity = 5.0f;
    float fallSpeed = .0f;
    Vector3 nextPos;

    bool onMoveBoard = false;
    public bool canMove = true;
    int lastCount = 0;
    Collider[] lastHit = new Collider[3];
    Vector3 moveDir;
    Collider[] hits;

    Vector3 curPos;
    protected Transform HUDCamera, leftHand, rightHand;

    // Start is called before the first frame update
    public void Init(Transform t)
    {
        transform = t;
        gameObject = t.gameObject;
    }

    public void SetCameraHands(Transform head, Transform left, Transform right)
    {
        HUDCamera = head;
        leftHand = left;
        rightHand = right;
    }

    // Update is called once per frame
    public void Update(float dt)
    {
        deltaTime = dt;

        switch (PlayerControl.level) {
            case 0:
                curPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Collider[] hits = Physics.OverlapBox(curPos, new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity, 1 << LayerMask.NameToLayer("Platform"));
                if (hits != null && hits.Length > 0)
                {
                    transform.parent = hits[0].transform;
                }
                break;

            case 1:

                if (!canMove) return;

                curPos = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
                hits = Physics.OverlapBox(curPos, new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity, 1 << LayerMask.NameToLayer("MoveBoard"));
                //Debug.DrawRay(curPos, new Vector3(0,-0.3f,0), Color.red);

                if (hits != null && hits.Length > 0)
                {
                    
                    onMoveBoard = true;

                    for (int i = 0; i < hits.Length; i++)
                    {
                        bool ishit = false;
                        for (int j = 0; j < lastHit.Length; j++)
                        {
                            if (lastHit[j] != null)
                            {
                                if (hits[i] == lastHit[j])
                                {
                                    ishit = true;
                                    break;
                                }
                            }
                        }
                        if (!ishit)
                        {
                            lastHit[lastCount] = hits[i];
                            moveDir = lastHit[lastCount].transform.forward;
                            Debug.Log("hit board   " + lastHit[lastCount].transform.forward + "    " + lastHit[lastCount].transform.name);
                            lastCount++;
                            if (lastCount == 3) lastCount = 0;
                        }
                    }

                }
                else onMoveBoard = false;

                if (onMoveBoard)
                {
                    transform.position += dt * moveDir;
                }


                //if (canFall)Fall();
                if (transform.position.z < -8.0f ) {
                    transform.position = new Vector3(9.3f, 7.6f, 4.6f);
                }
                break;
        }

        

     
    }
    void Fall() {
        if (onGround && !Physics.Raycast(transform.position, new Vector3(0,-1,0), 1.0f, 1 << LayerMask.NameToLayer("MoveBoard"))) {
            onGround = false;
        }
        else if (!onGround) {
            fallSpeed += gravity * deltaTime;
            nextPos = transform.position - deltaTime * new Vector3(0, fallSpeed, 0);
            RaycastHit hit;
            if (!Physics.Linecast(transform.position, nextPos, out hit, 1 << LayerMask.NameToLayer("MoveBoard")))
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
