using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiContolBase 
{
    Transform tranform;
    Vector3 lookPos;

    public MultiContolBase(Transform t, Vector3 pos) {
        tranform = t;
        lookPos = pos;
    }

    // Start is called before the first frame update
    public virtual void Init(Transform t, Vector3 pos)
    {

    }

    // Update is called once per frame
    public virtual void Update(float dt)
    {
        
    }
}
