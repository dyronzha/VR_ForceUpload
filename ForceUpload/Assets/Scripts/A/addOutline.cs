using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addOutline : MonoBehaviour
{
    //public Material outlineMaterial;
    Material[] originMat;
   // GameObject lastObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addline(GameObject obj)
    {
        int materialLen = obj.GetComponentInChildren<MeshRenderer>().materials.Length;
        obj.GetComponentInChildren<MeshRenderer>().materials[materialLen - 1].EnableKeyword("_INUSE_ON");//.SetFloat("_InUse", 1.0f);
    }
    public void removeLine(GameObject obj)
    {
        int materialLen = obj.GetComponentInChildren<MeshRenderer>().materials.Length;

        obj.GetComponentInChildren<MeshRenderer>().materials[materialLen-1].DisableKeyword("_INUSE_ON");//.SetFloat("_InUse", 0.0f);

    }
}
