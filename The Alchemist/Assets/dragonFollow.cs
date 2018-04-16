using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class dragonFollow : MonoBehaviour {
    [Header("For dragon following")]
    public Transform dragonTarget;
    public float dragonSpeed = 5f;
    public float distanceFromPlayer;

    [Header("For Jumping")]//taking this from the playerController
    public Transform dragonGroundCheck;
    public LayerMask whatIsGround;
    [Tooltip("This is how fast the dragon will fall when it is in range of the player but still in the air")] public float dragonFallSpeed = .1f;
    private float groundRadius = .2f;
    private bool grounded;
    private bool facingRight;



    // https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
    private void FixedUpdate()
    {
        float step = dragonSpeed * Time.deltaTime;
        grounded = Physics2D.OverlapCircle(dragonGroundCheck.position, groundRadius, whatIsGround);

        if (Vector3.Distance(transform.position, dragonTarget.position) > distanceFromPlayer){
            transform.position = Vector3.MoveTowards(transform.position, dragonTarget.position, step);
        }
        //the extra statement prevents the dragon from bugging out under a floor but it makes the dragon move weirdly when the player jumps near the dragon
        else if (!grounded && (transform.position.y > dragonTarget.position.y)){    //If the dragon is not on the ground and still above the player
            //if the dagon is in range of the player but in the air, it will slowly fall down
            Vector3 tempVector = new Vector3(transform.position.x, transform.position.y - dragonFallSpeed);
            transform.position = Vector3.MoveTowards(transform.position, tempVector, dragonSpeed/4);

            
        }


        if ((transform.position - dragonTarget.position).x > 0 && !facingRight)
        {
            flip();
        }
        else if ((transform.position - dragonTarget.position).x < 0 && facingRight)
        {
            flip();
        }
    }


    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
}
