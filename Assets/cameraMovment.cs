using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovment : MonoBehaviour
{
    public GameObject target;
    public float movementSpeed;
    public float stopDistance;
    public AnimationCurve smoothIn;

    private float index;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        

        if (target != null)
        {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, distance / 100);     
        }
    }


}
