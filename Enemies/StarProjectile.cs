using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarProjectile : MonoBehaviour
{
    public float RotateSpeed = 2f;
    public float Radius = 0.5f;
    public GameObject boss;
 
    Player player;
    private Vector2 centre;
    private float _angle;
 
    private void Start()
    {
        centre = boss.transform.position;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
 
    private void Update()
    {
        centre = boss.transform.position;
        _angle += RotateSpeed * Time.deltaTime;
 
        Vector2 offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;
        transform.position = centre + offset;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            player.Damaged(5f);
        }
    }

}
