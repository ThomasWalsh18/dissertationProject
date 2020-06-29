using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSword : MonoBehaviour {

    public float length = 2f;
    public Transform Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (Player.position, Vector3.forward, ((200* 2) * Time.deltaTime));
		Destroy(this.gameObject, length);
	}
}
