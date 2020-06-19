using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;


//Used to modify animation and attacks
public class CharmerScript : MonoBehaviour
{
    //Get Player script for damage
    Player player;

    //Charmer stats
    public float health = 150f;
    public float maxHealth = 150f;

    public GameObject charmProjectile;
    float fireRate = 3f;
    float fireStamp = 0f;

    //If player within range or attacks first, charmer chase to attack
    bool isAttacked = false;
    bool enemyRange = false;
    bool dead = false;
    public Animator animator;


    public Image healthBar;
    public Image hpBarBG;


    //Pathfinding variables using package
    public AIPath path;
    public AIDestinationSetter destinationSetter;



    //Previous pos used for animation detection
    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
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
                if(Vector2.Distance(transform.position, destinationSetter.target.transform.position) <= 1f)
                {
                    enemyRange = true;
                }

                if(Vector2.Distance(transform.position, destinationSetter.target.transform.position) <= 0.6f)
                {
                    CharmAttack();
                }
            }
        


            lastPos = transform.position;
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

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            TakeDamage(other.gameObject.GetComponent<StandardBullet>().bulletDamage);
        }

        if(other.gameObject.tag == "Player")
        {
            player.Damaged(25f);
        }
    }


    //Throw charm attack
    void CharmAttack()
    {  
        if(Time.time - fireStamp >= fireRate)
        {
            Instantiate(charmProjectile, transform.position, Quaternion.identity);
            fireStamp = Time.time;
        }
    }


    IEnumerator Death()
    {
        //Die
        animator.SetBool("Dead", true);
        path.canMove = false;
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
