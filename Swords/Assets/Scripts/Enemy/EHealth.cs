using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EHealth : MonoBehaviour {

    public float enemyHealth = 2;
	public float ScoreValue = 10;
	public PlayerController PlayerStats;

    public EnemyList enemies;
    public float knockBackStrength;
    public float enemyDamage = 1;
    void Start()
    {
        PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemies = GameObject.FindGameObjectWithTag("Controller").GetComponent<EnemyList>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            PlayerStats.misses--;
            PlayerStats.hits++;
			enemyHealth = enemyHealth - PlayerStats.PlayerDamage;
            if (enemyHealth <= 0)
            {
                PlayerStats.NumberScore = PlayerStats.NumberScore + ScoreValue;
                enemies.remaining--;
                Destroy(this.gameObject);
            }

        }
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "BigSword")
        {
            enemyHealth = enemyHealth - PlayerStats.abilDamage;
            if (enemyHealth <= 0)
            {
                PlayerStats.NumberScore = PlayerStats.NumberScore + ScoreValue;
                enemies.remaining--;
                Destroy(this.gameObject);
            }

        }
    }
    void Update()
    {
    
    }
}
