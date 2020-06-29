using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject[] doors;

    public GameObject closedRoom;

    public List<GameObject> rooms;

    public int roomDiff = 100;

    void ChangeBossRoom()
    {
        Spawner bossRoom = rooms[rooms.Count - 1].GetComponent<Spawner>();
        bossRoom.bossRoom = true;
        bossRoom.completed.color = Color.red;
    }
    private void Start()
    {
        invoke();
    }
    public void invoke()
    {
        Invoke("ChangeBossRoom", 3.0f);
    }

}
