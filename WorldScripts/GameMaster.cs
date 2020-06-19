using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance;

    //Variables to be saved
    public int currentSacrifice = 0;
    public int highestScene = 0;
    public float maxHappiness = 100f;

    public float standardWeaponDamage = 30f;

    public float bulletLifeTime = 0.8f;

    public int prevScene;

    void Awake ()   
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }

    

    
}
