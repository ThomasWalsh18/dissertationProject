using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffManager : MonoBehaviour
{
    class GameState
    {
        public float maxLife;
        public float currentLife;
        public int hits;
        public int misses;

        public float advTimeToCompleteRoom;
        public float RoomDiff;
        public int howManyRooms;
        public int howManyRoomsCleared;
        public int waves;

        public float evaluation = 0;
        public GameState(float max, float current, int hit, int miss, float time, float diff, int maxRooms, int cleared, int wave)
        {
            maxLife = max; // needs to be starting life of the current wave
            currentLife = current;
            hits = hit;
            misses = miss;
            advTimeToCompleteRoom = time;
            RoomDiff = diff;
            howManyRooms = maxRooms;
            howManyRoomsCleared = cleared;
            waves = wave;
        }

        public float evaluate()
        {
            float eval = 0.0f;
            //Ammount of life expected to lose is 1/3 of the current max life
            int expectedLoss = 0;
            expectedLoss = (int)Mathf.Round((0.33f * maxLife) + 0.33f * howManyRooms);
            if(expectedLoss > currentLife)
            {
                expectedLoss = (int)currentLife;
            }
            //How many waves has the player got through
            float percentWavesCleared = 0.0f;
            percentWavesCleared = (float)howManyRoomsCleared / (float)howManyRooms;

            //How much of the expected life should be lost so far
            float expectedLossSoFar = Mathf.Round((percentWavesCleared / (float)expectedLoss) * 100.0f);
            
            float lifeLost = maxLife - currentLife;       
            if (lifeLost < expectedLossSoFar)
            {
                //Less than expected, doing well so we add the difference
                eval += expectedLossSoFar - lifeLost; 
            } 
            else if (lifeLost == expectedLossSoFar)
            {
                //Equal to the life lost, nothing to do
            } else
            {
                //Larger than the expected, doing bad minus the difference
                eval -= lifeLost - expectedLossSoFar; 
            }

            //Now hit ratio
            //Find total shots taken, then percentage of them that hit
            int totalShots = hits + misses;

            //Check to see if the player has fired any shots
            //This is becasue it is possible to just use the ability
            if(totalShots != 0)
            {
                //A max flat value that the hits and misses can add or take away
                int value = 2;
                float percentHits = (hits / (float)totalShots) * 100.0f;
                //Expected around 60% hits
                if(percentHits < 60.0f)
                {
                    //Less than the expected ammount of hits
                    //We need to get the inverse percentage inorder to minus the correct ammounts
                    float invPercent = 100 - percentHits;
                    eval -= (invPercent / 100) * value; 
                } else if (percentHits == 60.0f)
                {
                    //Exactly right no tuning required 
                }else
                {
                    //Over the expected, doing well
                    eval += (percentHits / 100) * value; 
                }
            }

            //AdvTime to complete room the larger the worse, the longer they spend in the rooms
            //the harder they are trying, and the more they are focusing
            int advSeconds = 9;
            if(advTimeToCompleteRoom < advSeconds)
            {
                //Less than expected, doing well
                eval += advSeconds - advTimeToCompleteRoom;
            } else if (advTimeToCompleteRoom == advSeconds)
            {
                //Exactly right
            } else
            {
                //More than expected
                eval -= advSeconds - advTimeToCompleteRoom;
            }
            //This number will be used to calculate the current skill level
            //The smaller the number the worse player and visa versa

            //This will then compare to the current game diff, then if in the same catagory, nothing happens
            //else the games stats will go up or down depending
            print(eval);
            return eval;
        }
    }
    RoomTemplates rooms;
    PlayerController player;
    public bool makeState = true;
    EnemyList list;
    void Start()
    {
        if (PlayerPrefs.GetInt("Difficulty") != 0)
        {
            gameObject.GetComponent<DiffManager>().enabled = false;
        }
        rooms = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        list = GameObject.FindGameObjectWithTag("Controller").GetComponent<EnemyList>();
    }
    void ChangeDiff(float eval)
    {
        //Changing by the eval alone is a too small increase to notice the differences
        //Due to the nature of the project this number will have to be enlarged inorder to notice differences
        float percentChange = (20 + eval) / 100;
        //sanity checks
        if(percentChange > 100)
        {
            percentChange = 100;
        }
        //If the eval number is between a certain range from zero, then the player is on the right diff
        //However if the eval is above this range then the player is finding it easier 
        //and visa versa
        if (eval >= -0.3f && eval <= 0.3f)
        {
            //Do nothing
        } else
        {
            //The different stats have different scales so need to be increased at different ammounts
            //There also has to be a max and a min check to make sure that damage doesnt go minus for example
            float mSpeedChange = percentChange / player.Mspeed;
            if (player.Mspeed - mSpeedChange > 0)
            {
                player.Mspeed -= mSpeedChange;
            }

            float damageChange = percentChange / player.PlayerDamage;
            if(player.PlayerDamage - damageChange > 0)
            {
                player.PlayerDamage -= damageChange;
            }

            float abilDamageChange = percentChange / player.abilDamage;
            if(player.abilDamage - abilDamageChange > 0)
            {
                player.abilDamage -= abilDamageChange;
            }

            float fireRateChange = percentChange / player.fireRate;
            if(player.fireRate + fireRateChange > 0)
            {
                player.fireRate += fireRateChange;
            }

            int roomDiffChange = (int)Mathf.Round(eval)* 10;
            if(rooms.roomDiff + roomDiffChange > 50)
            {
                rooms.roomDiff += roomDiffChange;
            }

            for(int i = 0; i < list.enimesList.Length; i++)
            {
                list.enimesList[i].GetComponent<EnemyStatController>().StatChange(percentChange);
            }
        }
    }

    void Update()
    {
        //Checks to stop pointless game states being evaluated, such as the first room before the player has done an action
        if (player.roomsCompleted != 0)
        {
            if(player.roomsCompleted % 3 == 0)
            {
                if (makeState)
                {
                    GameState current = new GameState(player.maxHealth, player.currentHealth, player.hits, player.misses, player.advTimes(), rooms.roomDiff, rooms.rooms.Count, player.roomsCompleted, player.wave);
                    ChangeDiff(current.evaluate());
                    makeState = !makeState;
                }
            }
        }
    }
}
