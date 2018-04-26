using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceBetween : MonoBehaviour {

    public Transform[] waypoints;
    public int minWaitAtWaypoint;
    public int maxWaitAtWaypoint;
    public float walkSpeed = 2f;
    private int counter = 0;
    private bool isWaiting; // better way to do this, but just going with what im thinking for right now

    private void Update()
    {
        //The transform walks to the first waypoint, if it is not current waiting
        if (!isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[counter].position, walkSpeed * Time.deltaTime);
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
    }

    private IEnumerator WaitHere()
    {
        //Takes a random value in the "Wait" range, then waits that long then returns
        isWaiting = true;
        int timeCounter = 0;
        int waitDuration = Random.Range(minWaitAtWaypoint, maxWaitAtWaypoint);
        Debug.Log("Wait duration" + waitDuration);
        Debug.Log("Time Counter: " + timeCounter);
        while(timeCounter < waitDuration)
        {
            yield return new WaitForSeconds(1);
            timeCounter++;
        }
        isWaiting = false;
    }
}
