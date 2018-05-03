using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float startingEnemyHealth = 100f;
    [HideInInspector] public float currentEnemyHealth;      //Most likely don't need this to be public, but if for what ever reason it is 
    private bool Dead = false;

	void Start () {
        currentEnemyHealth = startingEnemyHealth;
	}
	
    
    public void enemyTakeDamage(float damage)
    {
        currentEnemyHealth -= damage;
        
        if(currentEnemyHealth <= 0 && !Dead)
        {
            onDeath();
        }
    }

    //Add particle effects, animations, etc.. here 
    private void onDeath()
    {
        Dead = true;
        gameObject.SetActive(false);
    }
}
