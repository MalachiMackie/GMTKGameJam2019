using Assets.Scripts;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        gameManager.NextLevel();
    }

    public void QuitGame()
    {
        gameManager.QuitGame();
    }

}
