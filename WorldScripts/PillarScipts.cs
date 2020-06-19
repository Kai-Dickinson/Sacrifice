using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PillarScipts : MonoBehaviour
{
    Player player;
    PlayerController playerController;
    string labelTextSacrifice = "Press E to Sacrifice";
    string labelTextLeave = "Press E to Leave";

    bool sacrificeTrigger = false;
    bool leaveTrigger = false;

    bool buttonPressed;
    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;

        player = GameObject.Find("Player").GetComponent<Player>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Display button press for player
        if(other.tag == "Player")
        {
            if(gameObject.tag == "SacrificePillar")
            {
                sacrificeTrigger = true;
            }

            if(gameObject.tag == "LeavePillar")
            {
                leaveTrigger = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        leaveTrigger = false;
        sacrificeTrigger = false;
    }

    void OnGUI()
    {
        if(sacrificeTrigger)
        {
            GUI.Box(new Rect(Screen.width/2 - (Screen.width/10)/2, Screen.height * 3 / 5, Screen.width/8, Screen.height/14), labelTextSacrifice);

            if(Input.GetKeyDown(KeyCode.E) && !buttonPressed)
            {
                player.currentSacrifice = player.Sacrifice(player.currentSacrifice);
                buttonPressed = true;
                player.highestScene += 1;
                player.prevScene = SceneManager.GetActiveScene().buildIndex;
                player.standardWeaponDamage *= 1.3f; // 30% increase in damage for each sacrifice
                player.bulletLifeTime *= 1.15f; // 15% increase in lifetime for each sacrifice
                player.SavePlayer();
                SceneManager.LoadScene("SacrificeScene");
                //Next level loads
            }
        }

        if(leaveTrigger)
        {
            GUI.Box(new Rect(Screen.width/2 - (Screen.width/10)/2, Screen.height * 3 / 5, Screen.width/8, Screen.height/14), labelTextLeave);

            if(Input.GetKeyDown(KeyCode.E) && !buttonPressed)
            {
                player.SavePlayer();
                buttonPressed = true;
                SceneManager.LoadScene("LeaveScene");
                //Return to start
            }
        }
    }

}
