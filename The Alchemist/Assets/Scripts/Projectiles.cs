using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour {
    public float potionDamage = 50f;
    public GameObject potionHitEnemy;
    public GameObject potionHitWall;
    public GameObject potionHitBreakable;
    public float potionRadius = 2f;
    public LayerMask potionMask;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")   //Have to break function if the bullet hits the player (happens when the player is moving at full speed and firing sometimes)
        {
            return;
        }
       // Debug.Log("the potion has hit something");
        Collider2D[] tempHit = Physics2D.OverlapCircleAll(transform.position, potionRadius, potionMask, -Mathf.Infinity, Mathf.Infinity);
       // Debug.Log(tempHit.Length);

        for (int i = 0; i < tempHit.Length; i++)
        {
            //Debug.Log(tempHit[i].name);
            if (tempHit[i].tag == "Player") 
            {
                return;
            }
            else if (tempHit[i].tag == "Enemy")
            {
                tempHit[i].GetComponent<EnemyHealth>().enemyTakeDamage(potionDamage);
                var tempEffect = Instantiate(potionHitEnemy, transform.position, Quaternion.identity); //have to instantiate because otherwise they get deleted
                Destroy(tempEffect, tempEffect.GetComponent<ParticleSystem>().main.duration);
            }
            else if(tempHit[i].tag == "Breakable"){
                Destroy(tempHit[i].gameObject);
            }
            else
            {
                var tempEffect = Instantiate(potionHitWall, transform.position, Quaternion.identity);
                Destroy(tempEffect, tempEffect.GetComponent<ParticleSystem>().main.duration);
            }
            Destroy(gameObject);
           // Debug.Log(other.name);
        }
         
        
    }

}
