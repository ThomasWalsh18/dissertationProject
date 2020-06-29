using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public float shotSpeed;
    public float range;
    private GameObject Player;
    private Vector3 PlayersPos;
    public bool flip;
    public bool tracker;
    public float strength = 10.0f;
    public float damage = 1f;
    Vector3 heading;
    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        flip = GameObject.FindGameObjectWithTag("Enemy").GetComponent<eController>().flipped;
        PlayersPos = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z);
        if (!flip)
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }
        heading = PlayersPos - transform.position;
        lookAt();
    }
	
    void lookAt()
    {
        //this code is the 2d equvilant of the look at function
        Quaternion rotation = Quaternion.LookRotation(Player.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

	void Update () {
       
        if (tracker)
        {
            lookAt();
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, shotSpeed * Time.deltaTime);
        } else
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(heading * shotSpeed);
            //transform.position += heading.normalized * shotSpeed * Time.deltaTime;
            //gameObject.GetComponent<Rigidbody2D>().velocity = (heading * shotSpeed);
            //Vector3 normHead = heading.normalized;
            //transform.position += normHead * shotSpeed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, shotSpeed * Time.deltaTime);
        }
        Destroy(this.gameObject, range);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
