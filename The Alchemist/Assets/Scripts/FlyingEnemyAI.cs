using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour {

    [Header("This is probably only going to work for flying enemies")]
    public Transform player;
    public float enemySpeed;
    public float detectionRange;
    Animator anim;

    [Header("This is where the enemy will go back to if the player is out of range")]
    public Transform enemyHome;

    private bool facingRight;
    private Vector3 lastPosition = Vector3.zero;

    // most of this is reused from the dragon follow scipt, i think there is something that i could've done with subclasses to avoid this but eh
    void FixedUpdate () {
		if(Vector2.Distance(player.position, transform.position) < detectionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyHome.position, enemySpeed * Time.deltaTime);
        }

        //for animator
        float speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        anim.SetFloat("Speed", Mathf.Abs(speed));
        
        if ((transform.position - transform.position).x > 0 && !facingRight)
        {
            flip();
        }
        else if ((transform.position - transform.position).x < 0 && facingRight)
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
