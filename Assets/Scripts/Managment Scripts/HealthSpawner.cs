/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    #region Variables
    //Spawn Points
    public Transform[] spawnPoints;

    //Spawn Delay
    public float spawnDelay = 10f;

    //Time the time Health Pack will Spawn
    private float nextSpawnTime;

    //Reference to the Health Pack
    public GameObject hpPacks;

    //Stops the Spawning of HP Packs
    public bool stopSpawn = false;

    //Timer that checks for HP packs in positions
    private float searchCountdown = 1f;

    #endregion

    #region Unity Methods

    void Start()
    {
        //Runs an error if no spawn points are set
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points Set!");
        }
    }


    void Update()
    {
        if (ShouldSpawn())
        {
            SpawnHP();
        }
    }

    void SpawnHP()
    {
        if(stopSpawn == false)
        {
            //Sets delay
            nextSpawnTime = Time.time + spawnDelay;

            //Picks a random Spawn point for HP to spawn at
            Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

            //Checks to see if the _sp.Position is empty
            //If so it will spawn a new HP Pack
            if (checkIfPosEmpty(_sp.position))
            {
                //Spawns the prefab
                Instantiate(hpPacks, _sp.position, _sp.rotation);
            }
            else
            {
                Debug.Log("Reset HP Pack Position");

                nextSpawnTime = Time.time + spawnDelay;
                //SpawnHP();
            }
        }
    
  

    }

    bool checkIfPosEmpty(Vector3 targetPos) //Checks to see if the position is empty before spawning
    {
        //Finds all Gameobjects will tag "Health"
        //AKA just the healthpacks
        GameObject[] allHPPacks = GameObject.FindGameObjectsWithTag("Health");

        //Runs through the array
        foreach (GameObject hp in allHPPacks)
        {
            if (hp.gameObject.transform.position == targetPos)
            {
                return false;
            }
        }
         
            if (allHPPacks.Length == spawnPoints.Length + 1)
            {
                return true;
            }
 
        

        return true;
    }

    private bool ShouldSpawn()
    {
        return Time.time > nextSpawnTime;
    }
    #endregion
}
