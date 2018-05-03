using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    //most of the core code is from https://unity3d.com/learn/tutorials/topics/2d-game-creation/2d-character-controllers?playlist=17093
    public float maxSpeed = 10f;
    bool facingRight = true; //This really needed to be called facing left or something (everything is backwards)

    Animator anim;
    GameObject dragon;

    //For jump
    [Header("For Jump")]
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;
    private float groundRadius = 0.2f;
    private bool grounded = false;

    //for health/healing (on 'q') 
    [Header("For heal/helaing (on 'q')")]
    public float startingHealth;
    private float currentHealth;
    public float startingHealthPacks;
    public float healAmount;
    private float currentHealthPacks;
    private bool dead = false;
    public Text healthPackCounter;
    public ParticleSystem healingEffect;
    public Slider healthBar;
    private Image healthBarImage;
    public float flashSpeed = .4f;
    private bool isFlashing;
    public AudioSource playerHitSound;
    public AudioSource playerEating;

    //melee attack 1 (Binded to "Fire1" which is mouse1 and Ctrl 
    [Header("For melee attack 1 (on mouse1 and crtl")]
    //Need to add animaator stuff for this attack in the code 
    public float meleeAttack1Duration = 0.7f; //CHANGE THIS and just find length of animation maybe
    public float meleeAttack1Damage = 30f;
    public float meleeAttack1Distance = .1f;
    public float meleeAttack1Knockback = 5f;
    public ParticleSystem melee1HitEffect;
    private float nextAttack = 0;
    private Vector2 tempRay; //because idk how to change a direction vector when the character changes the way they are facing
    public AudioSource meleeAttackSound;

    //Projectile (using 'f' for now)
    [Header("For potion throw")]
    //Edit the gravity setting in the bullet rigidbody
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Image[] potionImages;
    public float potionRechargeSec = 10f;
    public AudioSource potionReady;
    public AudioSource potionNotReady;
    private int potionsAvailable = 3;
    private bool potionRechargeRunning = false;

    //For talking with npcs
    [Header("For talking with NPCs (on 'e')")]
    public Text NPCtext;
    private bool withNPC = false;
    private GameObject NPC;
    private Image NPCNextDialogueAvaible; //THis will show when the player can press "e" to get the next piece of dialogue
    private Image tempNPCHead; //i use this is 2 seperate coroutines so i need to instatiate the variable here
    private bool NPCtalking = false;
    private string NPCphrase;   //going to use this as a temp string to send into a coroutine to make the text look like it's writting itself
    public float NPCtalkingDelay = .2f;
    [Tooltip("This is the amount of time it will wait after a player leaves an npc to close out of the dialogue box")]public float NPCwalkAwayText = 4f;

    //For pause menu (IMPORTANT****- i disabled the checkmark in the Event System in order for this to work)
    //reference code: https://answers.unity.com/questions/171492/how-to-make-a-pause-menu-in-c.html
    [Header("For Pause Menu")]
    [Tooltip("The whole Canvas")]public Canvas pauseMenu;
    private bool isPaused;

    [Header("For Artifact")]
    public AudioSource artifactSoundPickUp;
    public ParticleSystem artifactParticlePickUp;

    void Start () {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        currentHealthPacks = startingHealthPacks;
        healthPackCounter.text = currentHealthPacks.ToString();
        //NPCNextDialogueAvaible.enabled = false;   // Doing this in the NPCNManager now
        healthBarImage = healthBar.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        dragon = GameObject.FindGameObjectWithTag("Dragon");
	}
	

	void FixedUpdate () {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Grounded", grounded);

        anim.SetFloat("vSpeed",GetComponent<Rigidbody2D>().velocity.y);

        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if(move < 0 && !facingRight)    //Fliped the greater than or less than statments because everything is backwards
        {
            flip();
        }
        else if(move > 0 && facingRight)
        {
            flip();
        }
	}

     void Update()
    {
        
        //Avoids game inputs while paused
        //There is a better way to do this but i can't code
        if (!isPaused)
        {
            //Pause Menu ( leave this outside the total if statment if you want to use 'Esc' to leave the pause menu aswell
            if (Input.GetButtonDown("Pause"))
            {
                togglePause();
            }

            if (grounded && Input.GetButtonDown("Jump") && !isPaused)
            {
                anim.SetBool("Grounded", false);
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            }

            //Healing
            if (currentHealthPacks > 0 && Input.GetButtonDown("Heal"))
            {
                playerTakeDamage(healAmount * -1);
                currentHealthPacks--;
                //Need to update canvas stuff here as well
                healthPackCounter.text = currentHealthPacks.ToString();
                healingEffect.Play();
                playerEating.Play();
            }

            //attempting melee1 again
            if (Input.GetButtonDown("Fire1") && (Time.time > nextAttack))
            {
                anim.SetTrigger("Melee Attack");
                meleeAttackSound.Play();
                //Stupid vector thing idk how to fix without this if statement(idk how to make a direction vector change to the direction of the character
                if (!facingRight)//need to inverse bool statements because of model is wrong way of what i though
                {
                    tempRay = new Vector2(1, 0);
                }
                else if (facingRight)
                {
                    tempRay = new Vector2(-1, 0);
                }

                nextAttack = Time.time + meleeAttack1Duration;
                RaycastHit2D[] melee1Detect = Physics2D.RaycastAll(transform.position, tempRay, meleeAttack1Distance); //Need array in case of multiple enemies hit at once

                for (int i = 0; i < melee1Detect.Length; i++)
                {
                    //find the tags here
                    if (melee1Detect[i].transform.tag == "Enemy")
                    {
                        melee1Detect[i].transform.GetComponent<EnemyHealth>().enemyTakeDamage(meleeAttack1Damage);
                        melee1HitEffect.Play();
                        //Tried to make the melee attack knockback the enemy but kinda doesn't work on the flying enemy because of how i coded movement(i think)
                        //melee1Detect[i].transform.GetComponent<Rigidbody2D>().AddForce(transform.right * meleeAttack1Knockback);

                        //for dragon attacking
                        if (!dragon.GetComponent<dragonFollow>().dragonAttacking)
                        {
                            dragon.GetComponent<dragonFollow>().dragonTarget = melee1Detect[i].transform;
                            dragon.GetComponent<dragonFollow>().toggleAttackEnemy();
                        }
                    }
                }
            }

            //Projectile/ shooting
            if (Input.GetButtonDown("Shoot"))
            {
                if (potionsAvailable > 0)
                {
                    anim.SetTrigger("Throw Potion");
                    //There is a cleaner way to do this but it works
                    //It changes the vector and flips the x scale of the bullet depending on where the player is facing
                    if (!facingRight) //Need to inverse the bool statments
                    {
                        GameObject projectileClone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                        projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, projectileSpeed * 2);
                    }
                    else if (facingRight)
                    {

                        GameObject projectileClone = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation * Quaternion.Euler(0, 180f, 0)); // flip the bullet
                        projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, projectileSpeed * 2); //negative speed and whatnot to go backwards
                    }
                    //anim.ResetTrigger("Throw Potion");
                    potionsAvailable--;
                    if (!potionRechargeRunning) // if the coroutine isn't running, then start it up
                    {
                        StartCoroutine(potionRecharge());
                    }

                    //Update canvas for potions
                    for (int i = 0; i < potionImages.Length; i++) // will enable the potion iamges when it is useable 
                    {
                        if (i < (potionsAvailable))
                        {     //Have to subtract 1 because of array starting at 0
                            potionImages[i].enabled = true;
                        }
                        else
                        {
                            potionImages[i].enabled = false;
                        }
                    }
                }
                else
                {
                    //THis happens when there are no potions ready os it will just play an error sound
                    potionNotReady.Play();
                }

            }

            //Talking with NPCs
            //THis is for talkign with NPCs
            if (withNPC && Input.GetButtonDown("Talk"))
            {
                if (!NPCtalking)
                {
                    NPCtext.text = null;//reseting the dialogue box
                    NPCtalking = true;
                    NPCphrase = NPC.GetComponent<NPCManager>().characterSpeak();
                    StartCoroutine(NPCtextDisplay());
                }
            }
        }       
    }

    private IEnumerator potionRecharge()
    {
        potionRechargeRunning = true;
        int potionsNeeded = potionImages.Length - potionsAvailable; 
        while (potionsNeeded > 0 )
        {
            yield return new WaitForSeconds(potionRechargeSec); // wait for recharge amount
            potionsAvailable++;
            potionsNeeded = potionImages.Length - potionsAvailable;
            //Update Canvas when a potion becomes available
            for (int i = 0; i < potionImages.Length; i++) // will enable the potion iamges when it is useable 
            {
                if (i < (potionsAvailable))
                {     //Have to subtract 1 because of array starting at 0
                    potionImages[i].enabled = true;
                }
            }
            potionReady.Play(); //Plays the sound when a potion is ready
        }
        potionRechargeRunning = false;
    }

    private IEnumerator NPCtextDisplay()
    {  
        int characterPlace = 0;
        char[] tempChar = NPCphrase.ToCharArray();
        tempNPCHead = NPC.GetComponent<NPCManager>().NPCHead; //need to assign it to a variable because i null out the 'NPC' gameobject when the player leaves the collider for the NPC
        tempNPCHead.enabled = true;

        if (NPCNextDialogueAvaible.enabled)//once the player presses for the next dialogue, need to disable the visual queue that says they can press it again immediately
        {
            NPCNextDialogueAvaible.enabled = false;
        }

        while (characterPlace < tempChar.Length) // keep going until the character array is fully shown
        {
            NPCtext.text += tempChar[characterPlace];
            characterPlace++;
            yield return new WaitForSeconds(NPCtalkingDelay);
        }

        if (!withNPC)
        {
            //if the player is no longer near the NPC when it finishes the dialogue, then wait the specified amount of seconds (default 4)the disable the NPC head image and text box
            //yield return new WaitForSeconds(NPCwalkAwayText);
            //Need to make custom WaitForSeconds so if the player is out of the NPC when the text scroll ends, and they reenter the NPC it will show that they can Continue the dialogue
            float tempWaitSecondsGoal = Time.time + NPCwalkAwayText;
            while(Time.time < tempWaitSecondsGoal && !withNPC)
            {
                yield return null;
            }

            if (!withNPC) //If you are still not with the NPC then disable everything
            {
                tempNPCHead.enabled = false;
                NPCtext.text = null;
                NPCNextDialogueAvaible.enabled = false;
                yield return new WaitForEndOfFrame();
            }

        }

        if (withNPC) // need to make sure the player is still with the NPC
        {
            NPCNextDialogueAvaible.enabled = true;
        }
        
        NPCtalking = false; //PUtting this after the if statment just incase of really rare edge cases

    }

    private IEnumerator NPCleavingTextDisable() //This function is for if the player walks away from an NPC and the NPC wasn't talking. THen it will disable the text box thing
    {
        NPCNextDialogueAvaible.enabled = false; // if the player leaves the NPC then they can't get the next dialogue 
        yield return new WaitForSeconds(NPCwalkAwayText);
        if (!withNPC && !NPCtalking)    //Avoiding edge cases
        {
            tempNPCHead.enabled = false;
            NPCtext.text = null;
        }
        
    }

    //This is no longer needed but i am going to keep it incase the raycast bugs out 
    private void OnTriggerStay2D(Collider2D collision)
    {
        //For melee attack 1
        //Debug.Log(collision.tag);
        /*
        if(collision.tag == "Enemy" && Input.GetButtonDown("Fire1") && (Time.time > nextAttack))
        {
            nextAttack = Time.time + meleeAttack1Duration;
            collision.GetComponent<EnemyHealth>().enemyTakeDamage(meleeAttack1Damage);
            melee1HitEffect.Play();
            // Debug.Log("Is in the damage function");
            //Previously you had to leave the collider before you could hit the enemy again. this should prevent that

        }
        */

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Health Pack")
        {
            currentHealthPacks++;
            healthPackCounter.text = currentHealthPacks.ToString();
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "NPC")//should be here for determining if the character is in a NPC
        {
            withNPC = true;
            NPC = collision.gameObject;
            NPCNextDialogueAvaible = NPC.GetComponent<NPCManager>().NPCRdyToTalk;

            if (!NPCtalking)
            {
                NPCNextDialogueAvaible.enabled = true;
            }
        }
        else if (collision.tag == "Artifact")
        {
            artifactParticlePickUp.Play();
            artifactSoundPickUp.Play(); // Could've instantiated it, but it's late
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "NPC")  //should be here for determining if the character is in a NPC
        {
            withNPC = false;
            NPC = null;
            if (!NPCtalking)
            {
                StartCoroutine(NPCleavingTextDisable()); //if the player leaves the collider and the NPC is no longer talking, it will wait (have to do this before i null out the NPC game object
            }
        }
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    public void playerTakeDamage(float damage)
    {
        //Debug.Log("took damage: " + damage);
        currentHealth -= damage;
        if(damage > 0)
        {
            playerHitSound.Play(); //if the player isn't healing...
        }

        if(currentHealth <= 0 && !dead)
        {
            dead = true;
            onDeath();
        }
        healthBar.value = currentHealth;

        if (currentHealth < (startingHealth * .25) && !isFlashing) // if the player is under 25% health, and the health bar isn't already flashing, go to the coroutine to start flashin
        {
            StartCoroutine(FlashHealthBar());
        }
    }

    private IEnumerator FlashHealthBar()
    {
        isFlashing = true;
        while (currentHealth < (startingHealth * .25))
        {
            healthBarImage.enabled = !healthBarImage.IsActive(); //makes the thing flash
            yield return new WaitForSeconds(flashSpeed);
        }
        healthBarImage.enabled = true; // incase you catch it when it is toggled off
        isFlashing = false;
    }

    public void togglePause()
    {
        //Toggles between the game being paused with the menu showing and no being paused and menu not being showed
        if (Time.timeScale == 0f)
        {
            isPaused = false;
            pauseMenu.enabled = false;
            Time.timeScale = 1f;
        }
        else
        {
            isPaused = true;
            pauseMenu.enabled = true;
            Time.timeScale = 0f;
        }
    }

    private void onDeath()
    {
        //This is where we will put what happens when the player dies
        Debug.Log("THe player has died :(");
    }
}
