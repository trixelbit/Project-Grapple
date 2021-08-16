using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


enum playerState
{ 
    idle,
    run,
    jump,
    groundHook,
    airHook,
    launch,
    collide,
    fall
}
public class PlayerMovement : MonoBehaviour
{

    public float runSpeed;
    public float airSpeed;
    public float jumpStrength;
    public float stopSpeed;
    public float retractSpeed;

    public GameObject mouse;
    public GameObject spritePlane;
    public GameObject claw;

    public SpriteBinder idle;
    public SpriteBinder run;
    public SpriteBinder jump;
    public SpriteBinder groundHook;

    [HideInInspector]
    public bool IsGrounded = true;
    private playerState state = playerState.idle;
    private bool isFloating = false;
    private Vector3 grabTarget;
    private Renderer r;
    private InGame controls;
    private Rigidbody rb;
    private AnimatedMaterial sprite;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InGame();
        controls.Player.Jump.performed += _ => Jump();
        controls.Player.Grab.performed += _ => Grab();
        r = spritePlane.GetComponent<Renderer>();
        sprite = new AnimatedMaterial(r, spritePlane.transform, idle);

    }

    // Update is called once per frame
    void Update()
    {
        if (isFloating)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            float xCoeffecient = controls.Player.HorizontalMovement.ReadValue<float>();

            if (xCoeffecient != 0)
            {
                rb.velocity = new Vector3(xCoeffecient * runSpeed, rb.velocity.y, 0);
                state = IsGrounded ? playerState.run : playerState.jump;
            }
            else
            {
                rb.velocity = Vector3.Lerp(new Vector3(rb.velocity.x, rb.velocity.y, 0), new Vector3(0, rb.velocity.y, 0), stopSpeed);
                state = playerState.idle;
                state = IsGrounded ? playerState.idle : playerState.jump;
            }

            spritePlane.transform.localScale = rb.velocity.x >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        updatePlayerSprite(state);

    }

    void Jump()
    {
        if (IsGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpStrength, 0);
        }
    }

    void Grab()
    {
        state = playerState.groundHook;
        isFloating = true;
        grabTarget = new Vector3(mouse.transform.position.x, mouse.transform.position.y, transform.position.z);
        claw.GetComponent<clawBehavior>().hook(grabTarget);
        spritePlane.transform.localScale = grabTarget.x - transform.position.x >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);

        StartCoroutine(Retract());
    }


    IEnumerator Retract()
    {
        yield return new WaitForSeconds(.35f);
        


        while (transform.position != grabTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, grabTarget, retractSpeed);
            yield return new WaitForSeconds(.01f);
        }

        
        claw.GetComponent<clawBehavior>().release();
        isFloating = false;


    }

    private void updatePlayerSprite(playerState s)
    {

        switch (s)
        {
            case playerState.idle:
                sprite.UpdateSprite(idle);
                break;
            case playerState.run:
                sprite.UpdateSprite(run);
                break;
            case playerState.jump:
                sprite.UpdateSprite(jump);
                break;
            case playerState.groundHook:
                sprite.UpdateSprite(groundHook);
                break;
            case playerState.airHook:
                break;
            case playerState.launch:
                break;
            case playerState.collide:
                break;
            case playerState.fall:
                break;
            default:
                break;
        }

        sprite.Render();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }


}
