using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndScript : MonoBehaviour
{
    GameObject boss;
    public GameObject leavePillar;
    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Enemy_Boss");
    }

    // Update is called once per frame
    void Update()
    {
        if(boss == null)
        {
            leavePillar.SetActive(true);
        }
    }
}
