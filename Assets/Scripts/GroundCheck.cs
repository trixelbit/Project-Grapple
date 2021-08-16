using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded = true;
    public GameObject reference;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider c)
    {
        IsGrounded = true;
        reference.GetComponent<PlayerMovement>().IsGrounded = IsGrounded;
    }

    private void OnTriggerExit(Collider c)
    {
        IsGrounded = false;
        reference.GetComponent<PlayerMovement>().IsGrounded = IsGrounded;
    }
}
