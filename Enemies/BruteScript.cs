using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class BruteScript : MonoBehaviour
{
   //Get Player script for damage
    Player player;

    //Charmer stats
    public float health = 400f;
    public float maxHealth = 400f;

    bool charging = false;


    //If player within range or attacks first, charmer chase to attack
    bool isAttacked = false;
    bool enemyRange = false;
    bool canAttack = false;
    bool dead = false;

    float lastAttack;

    public Animator animator;


    public Image healthBar;
    public Image hpBarBG;
    public Image chargeIndicator;


    //Pathfinding variables using package
    public AIPath path;
    public AIDestinationSetter destinationSetter;

    public GameObject movingTarget;

    public AudioSource source;
    public AudioClip chargeSound;

    //Previous pos used for animation detection
    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = chargeSound;
        destinationSetter.target = GameObject.Find("Player").transform;
        lastPos = transform.position;
        path.canMove = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!dead)
        {
            if(lastPos != transform.position)
            {
                animator.SetBool("Moving", true);
            }
            else 
            {
                animator.SetBool("Moving", false);
            }

            //Detect movement on X
            if(lastPos.x > transform.position.x)
            {
                //Going left
                animator.SetFloat("Horizontal", -1f);
            } else if(lastPos.x < transform.position.x) 
            {
                //Going right
                animator.SetFloat("Horizontal", 1f);
            }

            if(lastPos.y > transform.position.y)
            {
                //Going down
                animator.SetFloat("Vertical", -1f);
            } else if(lastPos.y < transform.position.y) 
            {
                //Going up
                animator.SetFloat("Vertical", 1f);
            }


            //Chekc if player is near else dont move
            if(isAttacked || enemyRange)
            {
                path.canMove = true;
                hpBarBG.gameObject.SetActive(true);
            }

            if(destinationSetter.target != null)
            {
                //Check for player in range
                if(Vector2.Distance(transform.position, destinationSetter.target.transform.position) <= 0.85f)
                {
                    enemyRange = true;
                }

                //Use attack
                if(Vector2.Distance(transform.position, destinationSetter.target.transform.position) >= 0.4f && canAttack && enemyRange)
                {
                    StartCoroutine(Charge(destinationSetter.target.transform.position));
                }


                if(destinationSetter.target.transform.position == transform.position)
                {
                    path.canMove = false;
                }
            }
        
            


            lastPos = transform.position;



            if(Time.time - lastAttack >= 2.5f)
            {
                canAttack = true;
            }        
        }



    }


    public void TakeDamage(float dmg)
    {
        health -= dmg;
        isAttacked = true;
        healthBar.fillAmount = health / maxHealth;

        if(health <= 0f)
        {
            dead = true;
            StartCoroutine(Death());
        }

    }


    //Brute unique attack
    IEnumerator Charge(Vector2 target)
    {
        charging = true;
        movingTarget.transform.position = destinationSetter.target.transform.position;
        //Display image, wait for a short time and then attack
        //Flash indicator
        chargeIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        chargeIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        chargeIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        chargeIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        //Do charge
        if(!source.isPlaying && charging)
        {
            source.Play();
        }
         //Play sound clip
        destinationSetter.target = movingTarget.transform;
        path.maxSpeed = 0.6f;
        chargeIndicator.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        destinationSetter.target = GameObject.Find("Player").transform;
        path.maxSpeed = 0.075f;
        path.canMove = true;
        canAttack = false;
        lastAttack = Time.time;
        charging = false;
    }



    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            TakeDamage(other.gameObject.GetComponent<StandardBullet>().bulletDamage);
        }

        if(other.gameObject.tag == "Player")
        {
            if(charging)
            {
                player.Damaged(40f);
            } else 
            {
                player.Damaged(10f);
            }
        }
    }

    IEnumerator Death()
    {
        //Die
        animator.SetBool("Dead", true);
        path.canMove = false;
        yield return new WaitForSeconds(0.85f);
        Destroy(gameObject);
    }
}
