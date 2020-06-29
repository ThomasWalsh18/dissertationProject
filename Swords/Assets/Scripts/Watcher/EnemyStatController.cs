using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatController : MonoBehaviour
{
    public float spawnChance;
    public float spawnWeight;
    public EHealth MainStats;
    public eController Stats;
    void Start()
    {

    }
    public void StatChange(float percent)
    {
        MainStats = gameObject.GetComponent<EHealth>();
        Stats = gameObject.GetComponent<eController>();

        float mSpeedChange = percent / Stats.Speed;
        if (Stats.Speed + mSpeedChange > 0)
        {
            Stats.Speed += mSpeedChange;
        }
        //Damage has a lower starting value so needs to change at a higher rate 
        float damageChange = (percent * 1.5f) / MainStats.enemyDamage;
        if (MainStats.enemyDamage + damageChange > 0)
        {
            MainStats.enemyDamage += damageChange;
        }
        
        float healthChange = (percent * 1.5f) / MainStats.enemyHealth;
        if (MainStats.enemyHealth + healthChange > 0)
        {
            MainStats.enemyHealth += healthChange;
        }

        float fireRateChange = percent / Stats.fireRate;
        if (Stats.fireRate - fireRateChange > 0)
        {
            Stats.fireRate -= fireRateChange;
        }
        
        float scoreChange = percent / MainStats.ScoreValue;
        if (MainStats.ScoreValue + scoreChange > 0)
        {
            MainStats.ScoreValue += Mathf.Round(scoreChange);
        }

        if(Stats.Projectile != null)
        {
            if (Stats.Projectile.GetComponent<EnemyProjectile>() != null)
            {
                float speed = Stats.Projectile.GetComponent<EnemyProjectile>().shotSpeed;
                float speedChange = (percent * 1.5f) / speed;
                if (speed + speedChange > 0)
                {
                    Stats.Projectile.GetComponent<EnemyProjectile>().shotSpeed += speedChange;
                }

                float range = Stats.Projectile.GetComponent<EnemyProjectile>().range;
                float rangeChange = (percent * 1.5f) / range;
                if (range + rangeChange > 0)
                {
                    Stats.Projectile.GetComponent<EnemyProjectile>().range += rangeChange;
                }
            
                float damage = Stats.Projectile.GetComponent<EnemyProjectile>().damage;
                float projDamageChange = (percent * 1.5f) / damage;
                if (damage + projDamageChange > 0)
                {
                    Stats.Projectile.GetComponent<EnemyProjectile>().damage += projDamageChange;
                }
            }
        }

    }

}
