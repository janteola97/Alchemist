using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    //most of the core code is from https://unity3d.com/learn/tutorials/topics/2d-game-creation/2d-character-controllers?playlist=17093
    public float maxSpeed = 10f;
    bool facingRight = true;

    Animator anim;

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

    //melee attack 1 (Binded to "Fire1" which is mouse1 and Ctrl 
    [Header("For melee attack 1 (on mouse1 and crtl")]
            //Need to add animaator stuff for this attack in the code 
    public float meleeAttack1Duration = 0.7f; //CHANGE THIS and just find length of animation maybe
    public float meleeAttack1Damage = 30f;
    public float meleeAttack1Distance = .1f;
    public ParticleSystem melee1HitEffect;
    private float nextAttack = 0;
    private Vector2 tempRay; //because idk how to change a direction vector when the character changes the way they are facing

    //Projectile (using 'f' for now)
    [Header("For potion throw")]
         //Edit the gravity setting in the bullet rigidbody
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    //For talking with npcs
    private bool withNPC = false;
    private GameObject NPC;

    void Start () {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        currentHealthPacks = startingHealthPacks;
        healthPackCounter.text = currentHealthPacks.ToString();
	}
	

	void FixedUpdate () {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Grounded", grounded);

        anim.SetFloat("vSpeed",GetComponent<Rigidbody2D>().velocity.y);

        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("Speed", Mathf.Abs(move));

        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        if(move > 0 && !facingRight)
        {
            flip();
        }
        else if(move <0 && facingRight)
        {
            flip();
        }
	}

     void Update()
    {
        if (grounded && Input.GetButtonDown("Jump")){
            anim.SetBool("Grounded", false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }
        //Healing
        if(currentHealthPacks > 0 && Input.GetButtonDown("Heal"))
        {
            takeDamage(healAmount * -1);
            currentHealthPacks--;
            //Need to update canvas stuff here as well
            healthPackCounter.text = currentHealthPacks.ToString();
            healingEffect.Play();
        }

        //attempting melee1 again
        if (Input.GetButtonDown("Fire1") && (Time.time > nextAttack))
        {
            //Stupid vector thing idk how to fix without this if statement(idk how to make a direction vector change to the direction of the character
            if (facingRight)
            {
                tempRay = new Vector2(1, 0);
            }
            else if (!facingRight)
            {
                tempRay = new Vector2(-1, 0);
            }

            nextAttack = Time.time + meleeAttack1Duration;
            RaycastHit2D[] melee1Detect = Physics2D.RaycastAll(transform.position, tempRay, meleeAttack1Distance); //Need array in case of multiple enemies hit at once

            for(int i = 0; i <melee1Detect.Length; i++)
            {
                //find the tags here
                if(melee1Detect[i].transform.tag == "Enemy")
                {
                    melee1Detect[i].transform.GetComponent<EnemyHealth>().enemyTakeDamage(meleeAttack1Damage);
                    melee1HitEffect.Play();
                } 
            }
        }

        //Projectile/ shooting
        if (Input.GetButtonDown("Shoot"))
        {
            //There is a cleaner way to do this but it works
            if (facingRight)
            {
                GameObject projectileClone = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, projectileSpeed*2);
            }
            else if (!facingRight)
            {

                GameObject projectileClone = Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation*Quaternion.Euler(0,180f,0)); // flip the bullet
                projectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, projectileSpeed*2); //negative speed and whatnot to go backwards
            }
        }

        //Talking with NPCs
        //THis is for talkign with NPCs
        if (withNPC && Input.GetButtonDown("Talk"))
        {
            Debug.Log(NPC.GetComponent<NPCmanager>().characterSpeak());
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "NPC")  //should be here for determining if the character is in a NPC
        {
            withNPC = false;
            NPC = null;
        }
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    private void takeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0 && !dead)
        {
            dead = true;
            onDeath();
        }
    }

    private void onDeath()
    {
        //This is where we will put what happens when the player dies
    }
}
