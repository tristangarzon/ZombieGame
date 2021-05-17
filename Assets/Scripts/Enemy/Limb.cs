/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour
{
    #region Variables
    //Stores a reference to the limb prefab
    public GameObject limbPrefab;
    //Checks to see if the Zombie is alive
    public bool fatal = false;
    //Checks to see if Leg is hit
    public bool isLeg;
    #endregion
	
    #region Unity Methods
    public void Hit()
    {

        //Checks for Limb Component
        //And Checks if Hit
        Limb childlimb = transform.GetChild(0).GetComponentInChildren<Limb>();
        if (childlimb)
        {
            childlimb.Hit();
        }
        

        //Sets local scale of hit object to zero
        transform.localScale = Vector3.zero;

        //Spawns the Limb prefab
        GameObject spawnedLimb = Instantiate(limbPrefab, transform.parent);
        //Allows the Prefab to fall off the body
        spawnedLimb.transform.parent = null;
        //Destorys limb after time
        Destroy(spawnedLimb, 10);

        //If the target is hit, activate the ragdoll
        if (fatal)
            GetComponentInParent<Zombie>().Death();

        Destroy(this);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //If the object collides with a weapon with the tag "Weapon"
        if(collision.gameObject.CompareTag("Weapon"))
        {
            //If leg is hit, sets is down to true
            if(isLeg == true)
            {
                GetComponentInParent<Zombie>().isDown = true; 
            }

            //Call Hit Function
            Hit();
        }
    }

    #endregion
}
