using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addRooms : MonoBehaviour
{
    private RoomTemplates templates;
    private GameObject[] Doors;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
        Doors = GameObject.FindGameObjectsWithTag("Door1");
        templates.doors = Doors;
        if (gameObject.name != "Entry Room") {
            for(int i = 0; i <Doors.Length; i++)
            {
                Doors[i].SetActive(false);
            }
        }
    }
}
