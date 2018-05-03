﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class dragonFollow : MonoBehaviour {
    Animator anim;
    [Header("For dragon following")]
    public Transform dragonTarget;
    public float dragonSpeed = 5f;
    public float distanceFromPlayer;
    private Vector3 lastPosition; // for dragon animator

    [Header("For Jumping")]//taking this from the playerController
    public Transform dragonGroundCheck;
    public LayerMask whatIsGround;
    [Tooltip("This is how fast the dragon will fall when it is in range of the player but still in the air")] public float dragonFallSpeed = .1f;
    private float groundRadius = .2f;
    private bool grounded;
    private bool facingRight;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    public AudioSource wingsFlap;
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
    public AudioSource wingsFlap;

<<<<<<< HEAD
    [Header("For dragon attack")]
    public float dragonDamage = 20f;
>>>>>>> be68db2f52a29dfa15941fc8e3a94909d8e6e066
=======
    public AudioSource wingsFlap;
>>>>>>> parent of d2d2013... Merge conflicts

=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======

>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
=======
>>>>>>> parent of 2d624b8... this will make problems
=======
>>>>>>> parent of 2d624b8... this will make problems
=======
    public AudioSource wingsFlap;
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
>>>>>>> parent of 5fa5f41... idk why
=======
    public AudioSource wingsFlap;
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
>>>>>>> parent of 5fa5f41... idk why

>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
    [Header("For The Hunger")]
    public Slider dragonHungerMeter;
    public Image sliderFIllImage;
    public float initialHungerTimer = 60f;
    public float percentageToFlash = .25f;
    public float flashSpeed = 0.5f;
    private bool dragonHungry;
    private float currentHungerTimer;
    private bool isFlashing;

    public void Start()
    {
        anim = GetComponent<Animator>(); // for animator
        //for the dragon hungry mechanic
        currentHungerTimer = initialHungerTimer;
        dragonHungerMeter.maxValue = initialHungerTimer;
        dragonHungry = true;

        lastPosition = Vector3.zero;    //For dragon animator
    }
    public void Update()
    {
        //For the hunger
        if(dragonHungry == true)
        {

            dragonHungerMeter.value = currentHungerTimer;
            currentHungerTimer -= Time.deltaTime;
        }
        
        if(currentHungerTimer / initialHungerTimer <= percentageToFlash && !isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashHungerTimer());
        }
    }

    private IEnumerator FlashHungerTimer()
    {
        while(currentHungerTimer / initialHungerTimer <= percentageToFlash)
        {
            //dragonTarget.Find("Fill").GetComponent<Image>().enabled = !dragonTarget.Find("Fill").GetComponent<Image>().IsActive();
            sliderFIllImage.enabled = !sliderFIllImage.IsActive();
            yield return new WaitForSeconds(flashSpeed);
        }
        isFlashing = false;
    }

    //This is for dragon following player
    // https://docs.unity3d.com/ScriptReference/Vector3.MoveTowards.html
    //for the speed calculation
    private void FixedUpdate()
    {
        float step = dragonSpeed * Time.deltaTime;
        //for animator
        float speed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        anim.SetFloat("Speed", Mathf.Abs(speed));
        //Debug.Log(speed);

        grounded = Physics2D.OverlapCircle(dragonGroundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Grounded", grounded);

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
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> be68db2f52a29dfa15941fc8e3a94909d8e6e066
=======
>>>>>>> parent of 5fa5f41... idk why
=======
>>>>>>> parent of 5fa5f41... idk why
=======
>>>>>>> parent of d2d2013... Merge conflicts
        //Sounds for wings flapping
        if (!grounded && !wingsFlap.isPlaying)
        {
            wingsFlap.Play();
        }

        //if the dragon target dies, go back to the player
        Debug.Log(dragonTarget.name);
        if (dragonAttacking && !dragonTarget.gameObject.activeSelf) 
        {
            dragonTarget = GameObject.FindGameObjectWithTag("Player").transform;
            toggleAttackEnemy();
        }
        //attack enemy if the bool is true and the dragon is in range, and if it is not already attacking
        if (dragonAttacking && Vector3.Distance(transform.position, dragonTarget.position) < distanceFromPlayer && !dragonAttackInAnim)
        {
            StartCoroutine(dragonAttack());
        }
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> parent of 5fa5f41... idk why
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
<<<<<<< HEAD
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
>>>>>>> be68db2f52a29dfa15941fc8e3a94909d8e6e066
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
=======
>>>>>>> parent of 2d624b8... this will make problems
=======
>>>>>>> parent of 2d624b8... this will make problems
=======
>>>>>>> parent of 5fa5f41... idk why
=======
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
<<<<<<< HEAD
=======
>>>>>>> parent of 54667b5... ***This will probably break any "cave levels" you're working on.....Bunch of updated stuff, sorry i forgot about this github thing because yeah
>>>>>>> parent of 5fa5f41... idk why
=======
>>>>>>> parent of d2d2013... Merge conflicts
    }


    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
}
