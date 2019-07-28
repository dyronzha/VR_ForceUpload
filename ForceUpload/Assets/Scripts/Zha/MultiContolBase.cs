using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiContolBase 
{
    public Transform transform;
    protected Transform whereLook;

    protected Transform HUDCamera, leftHand, rightHand;

    bool onMoveBoard = false;
    int lastCount = 0;
    Collider[] lastHit = new Collider[3];
    Vector3 moveDir;
    Collider[] hits;


    public MultiContolBase(Transform t, Transform look) {
        transform = t;
        whereLook = look;

    }
    
    public void SetCameraHands(Transform head, Transform left, Transform right) {
        HUDCamera = head;
        leftHand = left;
        rightHand = right;
    }

    public virtual Transform GetWhereLook() {
        return whereLook;
    }

    // Start is called before the first frame update
    public virtual void Init()
    {
        //transform.gameObject.SetActive(false);
    }
    public virtual void Awake() {
    }

    // Update is called once per frame
    public virtual void Update(float dt)
    {



        switch (PlayerControl.level) {
            case 0:
                
                //hits = Physics.OverlapBox(curPos, new Vector3(0.3f, 0.3f, 0.3f), Quaternion.identity, 1 << LayerMask.NameToLayer("Platform"));
                //if (hits != null && hits.Length > 0)
                //{
                //    transform.parent = hits[0].transform;
                //}
                break;

            case 1:
                Vector3 curPos = new Vector3(HUDCamera.position.x, transform.position.y, HUDCamera.position.z);
                hits = Physics.OverlapBox(curPos, new Vector3(0.3f, 0.3f, 0.3f), Quaternion.identity, 1 << LayerMask.NameToLayer("MoveBoard"));
                //Debug.DrawRay(curPos, new Vector3(0,-0.3f,0), Color.red);

                if (hits != null && hits.Length > 0)
                {
                    Debug.Log("hit board");
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
                            moveDir = Vector3.Lerp(moveDir, lastHit[lastCount].transform.forward, dt * 30.0f).normalized;
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
                break;
        }



       
        //transform.localPosition = new Vector3(HUDCamera.localPosition.x, transform.localPosition.y, HUDCamera.localPosition.z);
        //transform.rotation = Quaternion.LookRotation(new Vector3(HUDCamera.forward.x,0, HUDCamera.forward.z), Vector3.up);



    }
}
