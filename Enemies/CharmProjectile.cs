using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmProjectile : MonoBehaviour
{
    Vector2 startPos;
    Vector3 target;
    Vector3 endPos;
    public Rigidbody2D rb;


    public PlayerController playerScript;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        endPos = transform.position;
        target = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = target * 1.5f;

        if(Vector3.Distance(endPos, transform.position) >= 0.6f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.Damaged(10f);

            if(Time.time - playerScript.charmedStamp >= playerScript.charmCooldown)
            {
                playerScript.charmEnemy = startPos;
                playerScript.charmed = true;
                playerScript.charmedStamp = Time.time;
            }
            Destroy(gameObject);
        }
    }
}
