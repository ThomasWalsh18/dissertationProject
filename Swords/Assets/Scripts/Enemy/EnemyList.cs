using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public GameObject[] enimesList;
    public GameObject[] bosses;
    public int remaining;

    public int eDamageMulti = 1;

   
    void Awake()
    {
        //Making sure the probablilty is correct
        float checker = 0;
        for (int i = 0; i < enimesList.Length; i++)
        {
            checker += enimesList[i].GetComponent<EnemyStatController>().spawnChance;
        }

        if (checker != 1)
        {
            float newCheck = 0;
            if (checker > 1)
            {
                newCheck = checker - 1;
                for (int i = 0; i < enimesList.Length; i++)
                {
                    enimesList[i].GetComponent<EnemyStatController>().spawnChance -= newCheck / enimesList.Length;
                }
            }
            else
            {
                newCheck = 1 - checker;
                for (int i = 0; i < enimesList.Length; i++)
                {
                    enimesList[i].GetComponent<EnemyStatController>().spawnChance += newCheck / enimesList.Length;
                }
            }
        }

        checker = 0;
        for (int i = 0; i < bosses.Length; i++)
        {
            checker += bosses[i].GetComponent<EnemyStatController>().spawnChance;
        }

        if (checker != 1)
        {
            float newCheck = 0;
            if (checker > 1)
            {
                newCheck = checker - 1;
                for (int i = 0; i < bosses.Length; i++)
                {
                    bosses[i].GetComponent<EnemyStatController>().spawnChance -= newCheck / bosses.Length;
                }
            }
            else
            {
                newCheck = 1 - checker;
                for (int i = 0; i < bosses.Length; i++)
                {
                    bosses[i].GetComponent<EnemyStatController>().spawnChance += newCheck / bosses.Length;
                }
            }
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 1)
        {
            for(int i= 0; i < bosses.Length; i++)
            {
                bosses[i].GetComponent<EnemyStatController>().spawnWeight = 50;
            }
        }
    }

    void Update()
    {
        if(remaining < 0)
        {
            remaining = 0;
        }
    }
}
