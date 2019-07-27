using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiContolBase 
{
    public Transform transform;
    public Transform whereLook;

    protected Transform HUDCamera, leftHand, rightHand;




    public MultiContolBase(Transform t, Transform look) {
        transform = t;
        whereLook = look;
    }
    
    public void SetCameraHands(Transform head, Transform left, Transform right) {
        HUDCamera = head;
        leftHand = left;
        rightHand = right;
    }

    // Start is called before the first frame update
    public virtual void Init()
    {
        transform.gameObject.SetActive(false);
    }
    public virtual void Awake() {
    }

    // Update is called once per frame
    public virtual void Update(float dt)
    {
        transform.localPosition = new Vector3(HUDCamera.localPosition.x, transform.localPosition.y, HUDCamera.localPosition.z);
        transform.rotation = Quaternion.LookRotation(new Vector3(HUDCamera.forward.x,0, HUDCamera.forward.z), Vector3.up);
    }
}
