/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    #region Variables
    //Health Gained Amount
    public float healthPack = 1000f;

    //Reference to the player
    PlayerStats playerHealth;

 
    //Pickup SFX
    private AudioSource pickUpSFX;
    public AudioClip SFX;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerStats>();
    }

    private void Start()
    {
        pickUpSFX = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        //If the Health pack object collides with the player
        //Player cannot go over the totalHealth amount
        if (playerHealth.currentHealth < playerHealth.totalHealth)
        {

            //Then Remove the Health Pack
            Destroy(gameObject);

            //Add Health to the player
            //playerHealth.currentHealth = playerHealth.currentHealth + healthPack;
            playerHealth.AddHealth(healthPack);

            //SFX
            pickUpSFX.PlayOneShot(SFX);

        }
    }

    #endregion
}
