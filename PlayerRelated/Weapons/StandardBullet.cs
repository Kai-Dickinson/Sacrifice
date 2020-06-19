using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : MonoBehaviour
{
    Player player;
    public float bulletDamage = 30f;

    Vector2 mouseDir;
    Vector2 targetDir;

    public float bulletSpeed = 1.5f;

    public Rigidbody2D rb;

    public float lifeTime;
    float spawnTime;


    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player").GetComponent<Player>();
        bulletDamage = player.standardWeaponDamage;
        lifeTime = player.bulletLifeTime;

        mouseDir = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
        targetDir = new Vector2(transform.position.x, transform.position.y);

        targetDir = mouseDir - targetDir;
        targetDir.Normalize();
        spawnTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = targetDir * bulletSpeed;

        if(Time.time - spawnTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
