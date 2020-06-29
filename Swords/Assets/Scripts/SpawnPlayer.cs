using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {
    public GameObject Player;
	void Awake ()
	{
        Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
