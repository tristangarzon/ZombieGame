/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    #region Variables
    //Reference to the material
    [SerializeField]
    private PlayerStats health = null;
    [SerializeField]
    private Renderer[] healthRenderer = new Renderer[0];

    private float targetDissolveValue = 1f;
    private float currentDissolveValue = 1f;



    #endregion

    #region Unity Methods

    private void Update()
    {
        currentDissolveValue = Mathf.Lerp(currentDissolveValue, targetDissolveValue, 2f * Time.deltaTime);

        foreach(Renderer renderer in healthRenderer)
        {
            renderer.material.SetFloat("_Health", currentDissolveValue);
        }
    }


    private void HandleHealthChanged(int health, int maxHealth)
    {
        targetDissolveValue = (float)health / maxHealth;
    }

    #endregion
}
