using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiContolBase 
{
    public Transform tranform;
    public Transform whereLook;

    protected Transform leftHand, rightHand;




    public MultiContolBase(Transform t, Transform look) {
        tranform = t;
        whereLook = look;
    }
    public void SetHands(Transform left, Transform right) {
        leftHand = left;
        rightHand = right;
    }

    // Start is called before the first frame update
    public virtual void Init(Transform t, Transform pos)
    {

    }

    // Update is called once per frame
    public virtual void Update(float dt)
    {
        
    }
}
