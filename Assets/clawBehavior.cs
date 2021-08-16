using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clawBehavior : MonoBehaviour
{
    public GameObject player;
    public float extendSpeed;
    public float retractSpeed;
    public SpriteBinder open;
    public SpriteBinder closed;
    public GameObject[] links = new GameObject[8];

    private Renderer rend;
    private AnimatedMaterial am;
    private Vector3 mouseTarget;
    private bool isWorking = false;


    public void Awake()
    {
        rend = GetComponent<Renderer>();
        am = new AnimatedMaterial(rend);
        
    }

    public void Start()
    {
        am.UpdateSprite(open);
    }

    public void Update()
    {
        if(isWorking)
        {
            updateLinks();
        }
        
    }

    public void hook(Vector3 _target)
    {
        mouseTarget = _target;
        isWorking = true;
        foreach (GameObject e in links)
        {
            e.SetActive(true);
            e.transform.position = player.transform.position;
        }

        
        transform.position = player.transform.position;
        rend.enabled = true;

        am.UpdateSprite(open);

        rotateObject(transform, _target);
        
        StartCoroutine(moveToTarget(_target));
    }

    public void release()
    {
        rend.enabled = false;
        foreach (GameObject e in links)
        {
            e.SetActive(false);
        }
        isWorking = false;
    }

    private IEnumerator moveToTarget(Vector3 _target)
    {
        while (Vector3.Distance(transform.position, _target) > .02f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, extendSpeed);
            
            yield return new WaitForSeconds(.01f);
        }

        am.UpdateSprite(closed);
    }

    private void updateLinks()
    {
        float deltaX, deltaY, X, Y;
        deltaX = transform.position.x - player.transform.position.x;
        deltaY = transform.position.y - player.transform.position.y;

        for(int i = 0; i < 8; i++)
        {
            X = (deltaX / 8 * i) + player.transform.position.x;
            Y = (deltaY / 8 * i) + player.transform.position.y;
            links[i].transform.position = new Vector3(X, Y, links[i].transform.position.z);
        }

    }

    private void rotateObject(Transform t, Vector3 _target)
    {
        float yComp = _target.y - player.transform.position.y;
        float xComp = _target.x - player.transform.position.x;
        float theta;

        theta = Mathf.Rad2Deg * Mathf.Atan(yComp / xComp);
        theta = xComp > 0 ? theta : theta - 180;

        t.eulerAngles = new Vector3(0, 0, theta);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
