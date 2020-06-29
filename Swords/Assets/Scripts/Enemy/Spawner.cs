using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Spawner : MonoBehaviour
{
    public GameObject EntryRoom;
    public DiffManager Diff;
    public GameObject[] doors;
    public GameObject Player;
    public RawImage completed;
    public RoomTemplates rooms;

    public GameObject[] spawnPoints;
    public EnemyList enemies;
    private GameObject[] list;
    private GameObject[] bossList;
    public bool bossRoom;
    public bool playerInside;
    bool startTime = false;
    public float time = 0.0f;

    public BoxCollider2D playerDetector;
    public DiffManager DiffManager;

    void Start()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 0)
        {
            DiffManager = GameObject.FindGameObjectWithTag("Controller").GetComponent<DiffManager>();
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        enemies = GameObject.FindGameObjectWithTag("Controller").GetComponent<EnemyList>();
        list = enemies.enimesList;
        System.Random rnd = new System.Random(System.DateTime.Now.Millisecond);
        list = list.OrderBy(x => rnd.Next()).ToArray();
        bossList = enemies.bosses;
        bossList = bossList.OrderBy(x => rnd.Next()).ToArray();

        playerDetector = gameObject.GetComponent<BoxCollider2D>();
    }
    void destroy()
    {
        Destroy(this.gameObject);
    }
    void Update()
    {
        if (startTime)
        {
            time += Time.deltaTime;
        }
        // Check when to close the doors
        if (playerInside)
        {
            if(enemies.remaining == 0)
            {
                if (bossRoom)
                {
                    playerInside = false;
                    completed.enabled = true;
                    for(int i = 0; i < rooms.rooms.Count; i++)
                    {
                        rooms.rooms[i].GetComponent<Spawner>().destroy();
                    }
                    rooms.rooms.Clear();
                    rooms.doors = new GameObject[0];
                    Player.transform.position = new Vector3(0, 0, 0);
                    makeStates();
                    Player.GetComponent<PlayerController>().wave++;
                    Player.GetComponent<PlayerController>().maxHealth = Player.GetComponent<PlayerController>().currentHealth;
                    Player.GetComponent<PlayerController>().roomsCompleted = 0;
                    Instantiate(EntryRoom, new Vector3(0,0,0), Quaternion.identity);
                    rooms.invoke();
                }
                else
                {
                    playerInside = false;
                    completed.enabled = true;
                    for (int i =0; i < doors.Length; i++)
                    {
                        doors[i].SetActive(false);
                    }
                    makeStates();
                }
                startTime = false;
            }
        }
    }
    void makeStates()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 0) // Adaptive needs no changes
        {
            DiffManager.makeState = true;
            Player.GetComponent<PlayerController>().roomsCompleted++;
            Player.GetComponent<PlayerController>().times.Add(time);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (playerDetector.enabled == true)
            {
                // make doors appear
                for (int i =0; i < doors.Length; i++)
                {
                    doors[i].SetActive(true);
                }
                //deactivate the collision box
                playerDetector.enabled = false;
                //invoke a the spawner script
                if (bossRoom)
                {
                    SpawnBoss();
                } else
                {
                    SpawnEnemy();
                }
                playerInside = true;
                startTime = true;
            }
        }
    }
    void SpawnBoss()
    {
        print("Boss");
        int j = 0;
        float tempDiff = 0;
        while (rooms.roomDiff > tempDiff)
        {
            int selected = -1;
            int startWeight = Random.Range(0, 100);
            float totalWeight = 0;
            while (startWeight > totalWeight && selected < bossList.Length - 1)
            {
                selected++;
                totalWeight = totalWeight + (bossList[selected].GetComponent<EnemyStatController>().spawnChance * 100);
            }
            if (selected == -1)
            {
                selected = 0;
            }
            tempDiff = tempDiff + bossList[selected].GetComponent<EnemyStatController>().spawnWeight;
            Instantiate(bossList[selected], spawnPoints[j].transform.position, Quaternion.identity);
            enemies.remaining++;
            
            if (j == 0)
            {
                j = 1;
            }
            else
            {
                j = 0;
            }
        }
    }
    void SpawnEnemy()
    {
        //print("Spawning");
        int j = 0;
        float tempDiff = 0;
        while (rooms.roomDiff > tempDiff)
        {
            int selected = -1;
            int startWeight = Random.Range(0, 100);
            float totalWeight = 0;
            while (startWeight > totalWeight && selected < list.Length - 1)
            {
                selected++;
                totalWeight = totalWeight + (list[selected].GetComponent<EnemyStatController>().spawnChance * 100);
            }
            if (selected == -1)
            {
                selected = 0;
            }
            tempDiff = tempDiff + list[selected].GetComponent<EnemyStatController>().spawnWeight;
            if (rooms.roomDiff > tempDiff)
            {
                GameObject temp = Instantiate(list[selected], spawnPoints[j].transform.position, Quaternion.identity);
                temp.SetActive(true);
                enemies.remaining++;
            }
            if(j == 0)
            {
                j = 1;
            } else
            {
                j = 0;
            }
        }
    }
}
