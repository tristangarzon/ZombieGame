/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }


    #region Variables
    public Wave[] waves;
    private int nextWave = 0;

    //Spawn Points
    public Transform[] spawnPoints;

    //Time Between Waves
    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    //Timer that checks if Enemies are still alive 
    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;


    //UI
    public TextMeshProUGUI wavesCounter;
    public TextMeshProUGUI zombiesAlive;
    private int zombieCount;

    //Audio
    public AudioSource source;
    //Round Start SFX
    public AudioClip roundStart;



    #endregion

    #region Unity Methods

    void Start()
    {
        //DEBUG ERROR CHECKS

        //Runs an error if no spawn points are set
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No Spawn Points Set!");
        }



        waveCountdown = timeBetweenWaves;
    }

   
    void Update()
    {

        //Checks to see if any enemies are still alive
        //if so it wont spawn any new enemies
        if(state ==  SpawnState.WAITING)
        {
            //Check if enemies are still alive
            //If not then it will begin a new round
            if(!EnemyIsAlive())
            {
                //Begin a new round           
                WaveCompleted();

                return;

                //I can also add some extra stuff in here like a UI Notif or points
               


            }
            else
            {
                return;
            }
        }


        //Checks to see if we are spawning waves
        if(waveCountdown <= 0)
        {
           if(state != SpawnState.SPAWNING)
            {
                //Starts spawning a wave
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            //If we are spawning a wave it start the wave Countdown timer
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted() //Called when a wave is completed
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        //Once all waves are completed it will run the following
        if(nextWave + 1 > waves.Length -1)
        {

            //For now it will reset the wave back to the first wave
            //I could make the mobs harder or something along the lines of that
            nextWave = 0;
            Debug.Log("All waves completed!");
        }
        else
        {
            //Sets next wave
            nextWave++;
        }
   
    }

    bool EnemyIsAlive()
    {
        //Timer that checks to see if any enemies are alive
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            //Resets Search Countdown
            searchCountdown = 1f;

            //Checks to see if any Gameobject with the tag "Enemy" still exists within the world
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }

        }
        //Update UI displaying current zombie count
        zombiesAlive.text = ("Zombies Alive: " + GameObject.FindGameObjectsWithTag("Counter").Length);
        return true;
    }


    IEnumerator SpawnWave(Wave _wave) //Spawns the wave
    {
        Debug.Log("Spawning Wave: " + _wave.name);
    
        //Update UI displaying current Wave
        wavesCounter.text = ("Current Round: " + _wave.name);

        //SFX
        source.PlayOneShot(roundStart);

        state = SpawnState.SPAWNING;

        //Spawn
        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }



        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        //Spawn Enemy
        Debug.Log("New Enemy has Spawned: " + _enemy.name);

        //Picks a random spawn point for the enemy to spawn at
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);


    }


    #endregion
}

[System.Serializable]
public class Wave
{
    //Name of the incoming Wave
    public string name;
    //Reference to the enemy
    public Transform enemy;
    //Wave Count
    public int count;
    //Spawn Rate
    public float rate;
}
