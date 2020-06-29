using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFire : MonoBehaviour
{
    public float range = 1f;
    public string ChangeD;
    public float shotSpeed = 15.0f;
    public GameObject player;
    private Vector3 heading;
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        heading = transform.position - player.transform.position;
        Debug.DrawLine(player.transform.position, transform.position, Color.white, 2.5f);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" ||
            collision.gameObject.tag == "Enemy" ||
            collision.gameObject.tag == "Door1" ||
            collision.gameObject.tag == "Door2" ||
            collision.gameObject.tag == "Hole")
        {
            {
                Destroy(this.gameObject);
            }
        }
    }
    void Update () {
        transform.position += heading.normalized * shotSpeed * Time.deltaTime;
        //rb.AddForce(heading * shotSpeed);
        Destroy(gameObject, range);
	}
}
