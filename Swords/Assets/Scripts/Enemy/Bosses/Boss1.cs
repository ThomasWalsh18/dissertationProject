using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    public bool flipped = false;
    public GameObject player;
    public bool ranged;
    public float Speed;
    public float range;
    public float fireRate = 0.8f;
    private float nextFire = 0f;
    private SpriteRenderer sprite;

    public GameObject Projectile;

    private enum AIState { CHASING, DEAD, ATTACK };
    private AIState state = AIState.CHASING;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    void flipControll()
    {
        if (this.gameObject.transform.position.x < player.transform.position.x && flipped == false)
        {
            // transform.Rotate(new Vector3(0, 180, 0));
            sprite.flipX = true;
            flipped = true;
        }
        if (this.gameObject.transform.position.x > player.transform.position.x && flipped == true)
        {
            //transform.Rotate(new Vector3(0, 180, 0));
            sprite.flipX = false;
            flipped = false;
        }
    }

    void chase()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance > range)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            state = AIState.ATTACK;
        }
    }
    void dead()
    {

    }
    void attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Vector3 shot = new Vector3(transform.position.x + 2.3f, transform.position.y, transform.position.z);
            if (!flipped)
            {
                shot.x -= 5.3f;
            }
            if (ranged)
            {

                GameObject proj = Instantiate(Projectile, shot, Quaternion.identity) as GameObject;
                proj.transform.parent = transform;

            }
            else
            {
                if (Projectile == null)
                {

                }
                else
                {
                    shot.y += 1;
                    GameObject proj = Instantiate(Projectile, shot, Quaternion.Euler(0, 0, 45)) as GameObject;
                    proj.transform.parent = transform;
                }
            }
        }
        state = AIState.CHASING;
    }

    void Update()
    {
        flipControll();

        switch (state)
        {
            case AIState.CHASING:
                chase();
                break;
            case AIState.DEAD:
                dead();
                break;
            case AIState.ATTACK:
                attack();
                break;
        }

    }
}
