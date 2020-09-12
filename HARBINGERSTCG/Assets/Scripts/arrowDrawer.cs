using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initPos;
    Vector3 endPos;
    LineRenderer arrow;
    public GameObject card;
    Camera cam;
    public Vector3 offset = new Vector3(0, 0 , 10);
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (arrow == null)
            {
                arrow = gameObject.AddComponent<LineRenderer>();
            }
            arrow.enabled = true;
            arrow.startWidth = 0.4f;
            arrow.endWidth = 0.3f;
            arrow.positionCount = 2;
            initPos = card.GetComponent<Transform>().position + offset;
            arrow.SetPosition(0, initPos);
            arrow.useWorldSpace = true;
        }
        if(Input.GetMouseButton(0))
        {

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            endPos = Camera.main.ScreenToWorldPoint(mousePos) + offset;

            // endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            arrow.SetPosition(1, endPos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            // arrow.enabled = false;
        }
    }
} 
