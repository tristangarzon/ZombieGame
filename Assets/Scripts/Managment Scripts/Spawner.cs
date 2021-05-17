/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Variables
    //Spawn Timer
    public float spawnTime = 1f;
    //General Timer
    private float timer;
    //Objects it will Spawn
    public GameObject spawnGameObject;
    //Array of Spawn Points
    public Transform[] spawnPoints;


    //Amount of Zombies spawned
    private int zombieAmount;


    #endregion
	
    #region Unity Methods

    void Start()
    {
      
    }

   
    void Update()
    {
        if (timer > spawnTime)
        {
            //Will Spawn the object at a random spawn point
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            //Spawns the gameobject
            Instantiate(spawnGameObject, randomPoint.position, randomPoint.rotation);
            timer = 0;
            //**NOTE** I need a way to cap the amount of zombies spawning
        }
        timer += Time.deltaTime;
    }

    #endregion
}
