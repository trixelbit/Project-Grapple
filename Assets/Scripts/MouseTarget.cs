using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTarget : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.tag=="MousePlane")
                transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);

            
        }

    }
}
