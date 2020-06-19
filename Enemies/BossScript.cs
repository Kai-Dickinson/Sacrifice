using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering.LWRP;
public class BossScript : MonoBehaviour
{
   //Get Player script for damage
    Player player;
    float playerLastHit;
    float playerDamageTime = 1f;

    //Boss stats
    public float health = 4000f;
    public float maxHealth = 4000f;



    //Bool check distances and aggressor
    bool isAttacked = false;
    bool enemyRange = false;
    bool dead = false;


    public Animator animator;


    public Image healthBar;
    public Image hpBarBG;
    public TextMeshProUGUI lightIndicator;


    //Pathfinding variables using package
    public AIPath path;
    public AIDestinationSetter destinationSetter;

    public AudioSource source;
    public AudioClip attackSound;


    //Variables for lightsout
    public List<Light2D> lights;
    public GameObject playerLight;
    float lastLightsOut;
    float lightsOutTime;
    bool lightTextShow = false;
    bool hpShow = false;
    bool lightsROut = false;


    //Star manipulation
    public StarProjectile star;
    float lastStarChange;
    float starChangeTime;
    bool starAttacking = false;

    //Previous pos used for animation detection
    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        source.clip = attackSound;
        destinationSetter.target = GameObject.Find("Player").transform;
        lastPos = transform.position;
        path.canMove = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        starChangeTime = Random.Range(15f,20f);
        lightsOutTime = Random.Range(10f,20f);
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
                if(!hpShow)
                {
                    hpBarBG.gameObject.SetActive(true);
                }
                
            }

            if(destinationSetter.target != null)
            {
                //Check for player in range
                if(Vector2.Distance(transform.position, destinationSetter.target.transform.position) <= 1f)
                {
                    enemyRange = true;
                }

                //Use attack
                if(Vector2.Distance(transform.position, destinationSetter.target.transform.position) >= 0.4f)
                {
                    //attack
                }
            }

            lastPos = transform.position;


            //Check for lightsout
            if(Time.time - lastLightsOut >= lightsOutTime && !lightsROut)
            {
                StartCoroutine(LightsOut());
            }

            if(Time.time - lastStarChange >= starChangeTime && !starAttacking)
            {
                StartCoroutine(StarManipulate());
            }

            playerLight.gameObject.transform.position = player.transform.position;

        }



    }

    //Boss unique ability
    IEnumerator LightsOut()
    {
        //Indicator
        if(!lightTextShow)
        {
            lightIndicator.gameObject.SetActive(true);
            lightTextShow = true;
        }
        
        //Wait
        yield return new WaitForSeconds(2f);

        lightIndicator.gameObject.SetActive(false);
        //Play sound

        //Lights out
        if(!lightsROut)
        {
            foreach(Light2D obj in lights)
            {
                obj.gameObject.SetActive(false);
            }
            lightsROut = true;
        }
        
        if(!hpShow)
        {
            hpBarBG.gameObject.SetActive(false);
            hpShow = true;
        }
        
        //Turn player light on
        playerLight.SetActive(true);

        //Wait
        yield return new WaitForSeconds(5f);
        
        //Lights on
        if(lightsROut)
        {
            foreach(Light2D obj in lights)
            {
                obj.gameObject.SetActive(true);
            }
        }
        

        playerLight.SetActive(false);

        lightsOutTime = Random.Range(10,20);
        lastLightsOut = Time.time;

        hpBarBG.gameObject.SetActive(true);

        lightTextShow = false;
        hpShow = false;
        lightsROut = false;
    }


    IEnumerator StarManipulate()
    {
        if(!starAttacking)
        {
            star.Radius = 0.55f;
            star.RotateSpeed = 1.5f;
            yield return new WaitForSeconds(1f);
            star.Radius = 0.6f;
            star.RotateSpeed = 2f;
            yield return new WaitForSeconds(1f);
            star.Radius = 0.65f;
            star.RotateSpeed = 2.5f;
            yield return new WaitForSeconds(1f);
            star.Radius = 0.7f;
            star.RotateSpeed = 3f;
            yield return new WaitForSeconds(1f);
            starAttacking = true;
        }
        lastStarChange = Time.time;
        

        yield return new WaitForSeconds(5f);

        if(starAttacking)
        {
            star.Radius = 0.65f;
            star.RotateSpeed = 2.5f;
            yield return new WaitForSeconds(1f);
            star.Radius = 0.6f;
            star.RotateSpeed = 2f;
            yield return new WaitForSeconds(1f);
            star.Radius = 0.55f;
            star.RotateSpeed = 1.5f;
            yield return new WaitForSeconds(1f);
            star.Radius = 0.5f;
            star.RotateSpeed = 1f;
            yield return new WaitForSeconds(1f);
            starAttacking = true;
        }
        star.Radius = 0.5f;
        star.RotateSpeed = 1f;
        starChangeTime = Random.Range(15f,20f);
        starAttacking = false;
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
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && Time.time - playerLastHit >= playerDamageTime)
        {
            player.Damaged(40f);
            playerLastHit = Time.time;
        }
    }

    IEnumerator Death()
    {
        //Die
        animator.SetBool("Dead", true);
        path.canMove = false;
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
