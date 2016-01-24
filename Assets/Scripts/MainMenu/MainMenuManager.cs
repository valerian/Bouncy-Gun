using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour 
{
    public Scene gameScene;

    public void play()
    {
        SceneManager.LoadScene("Game");
    }
}
