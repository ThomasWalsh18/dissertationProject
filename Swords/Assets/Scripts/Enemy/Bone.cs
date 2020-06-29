using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public PlayerController player;
    public float length = 2f;
    public float strength = 10f;
    public float damage = 1;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, ((200 * 2) * Time.deltaTime));
        Destroy(this.gameObject, length);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);   
    }
}
