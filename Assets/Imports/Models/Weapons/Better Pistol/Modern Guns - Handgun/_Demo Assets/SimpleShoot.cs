using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    //Source of the Fire
    public AudioSource source;
    //Gun Firing Sound
    public AudioClip fireSound;
    //Reload Sound
    public AudioClip reload;
    //No Ammo Sound
    public AudioClip noAmmo;
    //Current Mag weapon is using
    public Magazine magazine;
    //Reference to the Socket Interactor
    public XRBaseInteractor socketInteractor;
    //Used for reloading
    private bool hasSlide;



    public void AddMagazine(XRBaseInteractable interactable) //Called when Mag is put inside weapons XR Socket
    {
        //Sets the Mag to the mag attached to the Weapon
        magazine = interactable.GetComponent<Magazine>();
        //Plays reload Sound
        source.PlayOneShot(reload);

        //Sets the slide to false, player must pull back on the slider after reloading for mag to work
        hasSlide = false;
    }

    public void RemoveMagazine(XRBaseInteractable interactable)
    {
        magazine = null;
        //Plays reload sound
        source.PlayOneShot(reload);

    }

    public void Slide()
    {
        hasSlide = true;
        source.PlayOneShot(reload);

    }


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        socketInteractor.onSelectEntered.AddListener(AddMagazine);
        socketInteractor.onSelectExited.AddListener(RemoveMagazine);
    }

 
    public void PullTheTrigger()
    {
        //Checks to see if the gun has a mag with bullets
        if(magazine && magazine.numberOfBullet > 0 && hasSlide)
        {
            //Calls animation on the gun that has the relevant animation events that will fire
            gunAnimator.SetTrigger("Fire");
        }
        else
        {
            source.PlayOneShot(noAmmo);
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        //Decreases bullets by 1 after every shot
        magazine.numberOfBullet--;

        //Plays the Shoot SFX
        source.PlayOneShot(fireSound);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }


    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
