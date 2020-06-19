using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int prevScene;
    public int highestScene = 0;
    public PlayerController playerScript;
    public HealthController healthBar;
    public float maxHappiness = 100f;
    public float currentHappiness;

    public int currentSacrifice = 0;
    public Animator animator;
    public GameObject deathScreen;



    //Weapon related
    public float standardWeaponDamage; //First weapon
    public float bulletLifeTime;


    // Start is called before the first frame update
    void Start()
    {   
        standardWeaponDamage = 30f;
        bulletLifeTime = 0.8f;
        
        LoadPrevScene();
        if(prevScene >= 1)
        {   
            LoadHighScene();
        } else 
        {
            SavePlayer();
        }

        if(highestScene >= 3)
        {
            highestScene = 1;
        }
        
        
        if(highestScene >= 1)
        { 
            LoadPlayer();
        }

        currentHappiness = maxHappiness;
        healthBar.ChangeMaxHP(maxHappiness);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damaged(float dmg)
    {
        currentHappiness -= dmg;
        healthBar.SetHP(currentHappiness);

        if(currentHappiness <= 0f)
        {
            StartCoroutine(Death());
        }
    }

    public int Sacrifice(int currentState)
    {
        switch(currentState)
        {
            case 0:
                maxHappiness = 80f;
                currentHappiness = 80f;
                healthBar.ChangeMaxHP(80f);
                return 1;
            case 1:
                maxHappiness = 60f;
                currentHappiness = 60f;
                healthBar.ChangeMaxHP(60f);
                return 2;
            case 2:
                maxHappiness = 40f;
                currentHappiness = 40f;
                healthBar.ChangeMaxHP(40f);
                return 3;
        }
        return 3;

    }

    IEnumerator Death()
    {
        //Activate death animation, then death screen
        deathScreen.SetActive(true);
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(0.46f);
        Destroy(gameObject);
    }

    public void SavePlayer()
    {
        GameMaster.Instance.currentSacrifice = currentSacrifice;
        GameMaster.Instance.maxHappiness = maxHappiness;
        GameMaster.Instance.standardWeaponDamage = standardWeaponDamage;
        GameMaster.Instance.highestScene = highestScene;
        GameMaster.Instance.bulletLifeTime = bulletLifeTime;
        GameMaster.Instance.prevScene = prevScene;
    }

    public void LoadPlayer()
    {
        //Call saved from gamemaster
        maxHappiness = GameMaster.Instance.maxHappiness;
        currentSacrifice = GameMaster.Instance.currentSacrifice;
        standardWeaponDamage = GameMaster.Instance.standardWeaponDamage;
        bulletLifeTime = GameMaster.Instance.bulletLifeTime;
    }

    public void LoadHighScene()
    {
        highestScene = GameMaster.Instance.highestScene;
    }


public void LoadPrevScene()
    {
        prevScene = GameMaster.Instance.prevScene;
    }



}
