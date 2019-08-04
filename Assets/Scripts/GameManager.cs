using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject PlayerGO;
    private Player Player;

    private Levels currentLevel = Levels.Menu;

    private bool hasPlayer;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayer = GetPlayer();
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Do Other Things
        hasPlayer = GetPlayer();
        if (currentLevel == Levels.One)
        {
            Player.DisableDash();
        }
    }

    public void LoadLevel(Levels level)
    {
        if (level == currentLevel)
        {
            return;
        }

        currentLevel = level;

        //Do things to make switching levels nice
        switch (level)
        {
            case Levels.Menu:
                {
                    SceneManager.LoadSceneAsync("MainMenu");
                    break;
                }
            default:
                {
                    SceneManager.LoadSceneAsync($"Level {level.ToString()}");
                    break;
                }
        }
    }
    
    private bool GetPlayer()
    {
        var player = FindObjectOfType<Player>();

        Player = player;
        PlayerGO = player?.gameObject;

        if (Player != null)
        {
            Player.gameManager = this;
        }

        return player != null;
    }

    public void QuitGame()
    {
        //Todo: Make quitting nice
        Application.Quit();
    }

    public void NextLevel()
    {
        LoadLevel(currentLevel + 1);
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
