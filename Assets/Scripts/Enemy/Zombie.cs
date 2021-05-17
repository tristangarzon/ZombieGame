/*
* Made by: Tristan Garzon
* 
* Script Summary:
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    #region Variables
    //Follow Target
    public Transform target;
    private NavMeshAgent agent;

    //Array Reference to the ragdoll
    private Rigidbody[] rbs;

    //Audio
    public AudioClip deathAudio;

    //Zombie Damage
    public float attackDamage = 20f;

    //How often the zombie inflicts damage onto the enemy
    [SerializeField]
    private float attackTime = 5f;

    //Checks to see if the zombie is able to attack
    //Really just need this so the AttackTime function can fully play through
    public bool canAttack = true;


    //Reference to the animator
    Animator anim;

    //Called if the leg of the zombie is shot
    public bool isDown;

    //Distance the Zombie must be away from the player before he begins attacking
    [SerializeField]
    private float chaseDistance = 2f;

    //Turn Speed of the Zombie
    [SerializeField]
    private float turnSpeed = 5f;

    #endregion


	
    #region Unity Methods

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //Finds the target player by looking for an XR Rig
        target = FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.XRRig>().transform;
        DisactivateRagdoll();
    }

   
    void Update()
    {
     

        //If the Zombie is close to the player, the player will begin to lose HP
        if(Vector3.Distance(target.position, transform.position) < chaseDistance && canAttack == true && !target.GetComponent<PlayerStats>().isDead)
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer();
        }

        //Checks if the zombie has been downed
        //if so plays the down animation
        if (isDown == true)
        {
            //Play the down animation
            anim.SetBool("isFalling", true);

        }

    }



    void ChasePlayer() 
        //Chases the player
        //Plays the proper animation wether zombie is standing or down
    {
        //Updates Rotation when chasing, this is disabled when attacking
        agent.updateRotation = true;
        //Updates Position when chasing
        agent.updatePosition = true;
        //Sets the target value to be the agent (AKA the player)
        agent.SetDestination(target.position);


        if(isDown == false)
        {   
            //Stops the Attack animation
            anim.SetBool("isAttacking", false);
            //Plays the walk animation
            anim.SetBool("isWalking", true);
        }
        if(isDown == true)
        {
            anim.SetBool("isBiting", false);
            anim.SetBool("isCrawling", true);
        }
    
    }

    void AttackPlayer()
        //Attacks the player using the proper animation
        //For if the player is down or not
    {
        //Stops the Navmesh from handeling the rotation, considering were doing it below
        agent.updateRotation = false;
        //Makes sure that the direction of the enemy always faces the player before attacking
        Vector3 direction = target.position - transform.position;
        //Makes sure both are on the same Y axis
        direction.y = 0;
        //Handles Rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        //Stops the Navmesh from updating positiong while the zombie is attacking
        agent.updatePosition = false;

        if (isDown == false)
        {
            //Plays the attack Animation
            anim.SetBool("isAttacking", true);
            //Stops the walk animation
            anim.SetBool("isWalking", false);

            //Player Loses HP
            StartCoroutine(AttackTime());

            canAttack = true;

        }
        if (isDown == true)
        {
            //Plays the Bitting Animation
            anim.SetBool("isBiting", true);
            //Stops the crawling Animation
            anim.SetBool("isCrawling", false);

            //Player Loses HP
            StartCoroutine(AttackTime());

            canAttack = true;
        }

      
    }

    IEnumerator AttackTime()
    {
        //Sets to false so the whole method can be run
        canAttack = false;

        yield return new WaitForSeconds(0.5f);
       
        if (isDown == false)
        {
            target.GetComponent<PlayerStats>().RemoveHealth(attackDamage);
        }
        if (isDown == true)
        {
            target.GetComponent<PlayerStats>().RemoveHealth(attackDamage / 2);
        }

        yield return new WaitForSeconds(attackTime);
        
    }

    public void Death() 
    {
        //Activates Ragdoll upon Death
        ActivateRagdoll();
        //Disables the agent, Animator, & Script
        agent.enabled = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<AudioSource>().PlayOneShot(deathAudio);
        Destroy(gameObject, 10);
        Destroy(this);
        
    }


    void ActivateRagdoll()
    {
        //Sets each part of the bodies Kinematic to false
        foreach (var item in rbs)
        {
            item.isKinematic = false;
        }
    }

    void DisactivateRagdoll()
    {
        //Sets each part of the bodies Kinematic to true
        foreach (var item in rbs)
        {
            item.isKinematic = true;
        }
    }

    #endregion
}
