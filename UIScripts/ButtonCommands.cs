using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour
{
    public Player player;
    public void RestartGame()
    {
        player.prevScene = SceneManager.GetActiveScene().buildIndex;
        player.SavePlayer();
        SceneManager.LoadScene(1);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
