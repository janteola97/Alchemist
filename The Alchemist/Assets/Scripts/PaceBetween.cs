using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceBetween : MonoBehaviour {

    Animator anim;
    public Transform[] waypoints;
    public int minWaitAtWaypoint;
    public int maxWaitAtWaypoint;
    public float walkSpeed = 2f;
    private int counter = 0;
    private bool isWaiting; // better way to do this, but just going with what im thinking for right now
    private bool facingRight;
    private Vector3 lastPosition = Vector3.zero;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //The transform walks to the first waypoint, if it is not current waiting
        if (!isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[counter].position, walkSpeed * Time.deltaTime);
            anim.SetBool("isMoving", true);
        }
        //Once the transform reachers the waypint, it gets sent into the coroutine
        if(transform.position == waypoints[counter].position && !isWaiting)
        {
            StartCoroutine(WaitHere());
            counter++;
            if(counter >= waypoints.Length)
            {
                counter = 0;
            }
        }

        //for animator
        float speed = (transform.position - lastPosition).magnitude;
        if ((transform.position - lastPosition).x > 0 && !facingRight)
        {
            flip();
            //Debug.Log("Facing right if statement blue Lady NPC 1");
        }
        else if ((transform.position - lastPosition).x < 0 && facingRight)
        {
            flip();
           // Debug.Log("Facing right if statement blue Lady NPC 2");
        }
        lastPosition = transform.position;
        anim.SetFloat("Speed", Mathf.Abs(speed));
        

    }


    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if(gameObject.transform.childCount > 0)
        {
            foreach(Transform child in transform) //should revert the change for all children in the NPC (mostly for the image that tells the player they can talk to the NPC again)
            {
                Vector3 theChildScale = child.transform.localScale;
                theChildScale.x *= -1;
                child.transform.localScale = theChildScale;
            }
        }

    }

    private IEnumerator WaitHere()
    {
        //Takes a random value in the "Wait" range, then waits that long then returns
        isWaiting = true;
        anim.SetBool("isMoving", false);
        int timeCounter = 0;
        int waitDuration = Random.Range(minWaitAtWaypoint, maxWaitAtWaypoint);
        //Debug.Log("Wait duration" + waitDuration);
        //Debug.Log("Time Counter: " + timeCounter);
        while(timeCounter < waitDuration)
        {
            yield return new WaitForSeconds(1);
            timeCounter++;
        }
        isWaiting = false;
    }
}
