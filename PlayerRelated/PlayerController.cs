using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player playerScript; //Reference Player script
    public float moveSpeed = 25f; //Player movement speed multiplier, can be changed if needed
    public Rigidbody2D rb;
    public Animator animator;

    //Sound related variables
    public AudioSource audioSource;

    //SHOOTING SECTION
    public GameObject firingPoint; //Point at which projectiles are fired from
    float fireRate = 1f; //ie one shot per second
    float fireStamp = 0f; //Time stamp of last time shot
    public GameObject standardBullet;
    public AudioSource source;
    public AudioClip gunShot;



    //Effects section
    public bool charmed = false;
    public float charmedStamp = 0f;
    public float charmedLength = 2f;
    public float charmCooldown = 3f;
    public Vector2 charmEnemy;


    Vector2 movement;

    void Start()
    {
        source.clip = gunShot;
    }


    // Update is called once per frame
    void Update()
    {
        //Get movement values input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement *= 0.02f;

        //Convert mouse position to center
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        mousePos.x -= Screen.width/2;
        mousePos.y -= Screen.height/2;

        //Set animator values using mouse
        animator.SetFloat("Horizontal", mousePos.x);
        animator.SetFloat("Vertical", mousePos.y);
        animator.SetFloat("speed", movement.sqrMagnitude);


        if(!charmed)
        {
            if(Input.GetMouseButtonDown(0))
            {
                FireStandardWeapon();
            }

        } else 
        {
            //Timer to remove charm
            if(Time.time - charmedStamp >= charmedLength)
            {
                charmed = false;
            }
        }
        
    }

    void FixedUpdate()
    {
        //Apply movement to rigidbody
        if(!charmed)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        } else 
        {
            //Charm towards enemy
            rb.MovePosition(Vector2.Lerp(transform.position, charmEnemy, moveSpeed * 0.01f * Time.deltaTime));
        }


    }


    //Fire standard weapon from firing point
    void FireStandardWeapon()
    {
        if(Time.time - fireStamp >= fireRate)
        {
            //Shoot
            Instantiate(standardBullet, firingPoint.transform.position, Quaternion.identity);
            fireStamp = Time.time;
            source.Play();
        }
    }


}
