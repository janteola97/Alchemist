using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour {
    public float bulletDamage = 50f;
    public GameObject bulletHitEnemy;
    public GameObject bulletHitWall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            return;
        }
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealth>().enemyTakeDamage(bulletDamage);
           var tempEffect = Instantiate(bulletHitEnemy, transform.position, Quaternion.identity); //have to instantiate because otherwise they get deleted
            Destroy(tempEffect, tempEffect.GetComponent<ParticleSystem>().main.duration);
        }
        else
        {
            var tempEffect = Instantiate(bulletHitWall, transform.position, Quaternion.identity);
            Destroy(tempEffect, tempEffect.GetComponent<ParticleSystem>().main.duration);
        }
        Debug.Log(other.name);
        Destroy(gameObject);
    }

}
