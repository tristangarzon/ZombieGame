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

public class PlayerStats : MonoBehaviour
{
    #region Variables

    //Reference to this script
    public static PlayerStats singleton;

    //Players Total HP
    public float totalHealth = 100f;
    //Players Current HP
    public float currentHealth;

    //Checks to see if the player is dead
    public bool isDead = false;

    //% of HP
    [SerializeField]
    public float percentageHP;

    //Array Reference to the renderers
    [SerializeField]
    private Renderer[] healthRenderer = new Renderer[0];

    //Blend Values
    private float targetDissolveValue = 1f;
    private float currentDissolveValue = 1f;

    //Respawn Point
    [SerializeField]
    private Transform respawnPoint;




    #endregion

    #region Unity Methods

    private void Awake()
    {
        singleton = this;
    }


    private void Start()
    {
        //Sets Health to totalHealth (100)
        currentHealth = totalHealth;

        //Sets the % HP
        percentageHP = currentHealth / totalHealth;
    }


    private void Update()
    {
        HealthBar();
    }


    public void RemoveHealth(float damage)
    {
        //Takes away Health from the player
        if(currentHealth > 0)
        {
            currentHealth -= damage;

            //Updates the % HP after reciving damage
            percentageHP = currentHealth / totalHealth;

        }
        else
        {
            
            Death();
        }
       
    }


    public void AddHealth(float healthAmount)
    {
        currentHealth += healthAmount;

        //Updates the % HP after reciving damage
        percentageHP = currentHealth / totalHealth;

    }


    void HealthBar()
    {
        //Sets the dissolve Value to smoothly blend
        currentDissolveValue = Mathf.Lerp(currentDissolveValue, targetDissolveValue, 2f * Time.deltaTime);

        //Sets the targetDissolve Value to be the percentageHP 
        targetDissolveValue = percentageHP;

        //Each Renderer in the healthRenderer Array
        //changes the Health value to be the currentDissolveValue
        //I put this here just in case I want to dissolve more than 1 renderer
        foreach (Renderer renderer in healthRenderer)
        {
            renderer.material.SetFloat("_Health", currentDissolveValue);
        }
    }


    void Death()
    {
        currentHealth = 0;
        Debug.Log("Player is Dead");
        isDead = true;
        Respawn();

    }

    void Respawn()
    {
        this.transform.position = respawnPoint.transform.position;
        isDead = false;
        totalHealth = 10000f;
        currentHealth = totalHealth;
    }

    #endregion
}
