using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputTest : MonoBehaviour
{
    addOutline addLine;
    GameObject lastObj;

    // Start is called before the first frame update
    void Start()
    {
        addLine = GetComponent<addOutline>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {

            Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
            Debug.Log(hit.transform.name);
            addLine.addline(hit.transform.gameObject);
            lastObj = hit.transform.gameObject;
        }
        if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out hit))
        {
            addLine.removeLine(lastObj);
        }
    }
}
