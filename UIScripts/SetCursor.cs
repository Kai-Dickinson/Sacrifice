using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetCursor : MonoBehaviour
{
    public Texture2D menuCursor;
    public Texture2D gameCursor;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.Auto);
        } else 
        {
            Cursor.SetCursor(gameCursor, new Vector2(10,10), CursorMode.Auto);
        }
    }

}
