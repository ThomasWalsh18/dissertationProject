using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public int openingDirection;
    private RoomTemplates templates;
    private bool spawned = false;
    void Start()
    {
        Destroy(gameObject, 5.0f);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if (!spawned)
        {
            switch (openingDirection)
            {
                case 1: // bottom
                    Instantiate(templates.bottomRooms[Random.Range(0, templates.bottomRooms.Length)], transform.position, Quaternion.identity);
                    break;
                case 2: // top
                    Instantiate(templates.topRooms[Random.Range(0, templates.topRooms.Length)], transform.position, Quaternion.identity);
                    break;
                case 3: // left
                    Instantiate(templates.leftRooms[Random.Range(0, templates.leftRooms.Length)], transform.position, Quaternion.identity);
                    break;
                case 4: // right
                    Instantiate(templates.rightRooms[Random.Range(0, templates.rightRooms.Length)], transform.position, Quaternion.identity);
                    break;

            }
            spawned = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        // if (collision.GetComponent<RoomSpawner>() != null)
        //{
        if (collision.CompareTag("SpawnPoint"))
            {
                if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
                {
                    if (transform.position.x != 0.0f && transform.position.y != 0.0f)
                    {
                        Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    }
                    Destroy(gameObject);
                }
                spawned = true;
            }
        //}
        
    }
}
