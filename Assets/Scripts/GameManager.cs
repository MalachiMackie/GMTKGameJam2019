using Assets.Scripts;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject PlayerGO;
    private Player Player;

    private Levels currentLevel = Levels.Menu;

    private bool hasPlayer;

    public Image black;
    public Animator anim;
    public GameObject MainMenu;

    // Start is called before the first frame update
    void Start()
    {
        hasPlayer = GetPlayer();
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        //push image to the back
        //black.transform.position += new Vector3(0, 0, 10);

        StartCoroutine(InitialFade());
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Do Other Things
        hasPlayer = GetPlayer();    
        if (currentLevel == Levels.One)
        {
            Player.DisableDash();
        }

        GetImage();
    }

        private IEnumerator InitialFade()
    {
        black.transform.SetAsLastSibling();
        yield return new WaitUntil(() => black.color.a == 0);
        MainMenu.transform.SetAsLastSibling();
    }
    

        //Do things to make switching levels nice

    public IEnumerator LoadLevel(Levels level)
    {
        if (level == currentLevel)
        {
            yield break;
        }

        black.transform.SetAsLastSibling();

        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);

        currentLevel = level;
        switch (level)
        {
            case Levels.Menu:
                {
                    SceneManager.LoadSceneAsync("MainMenu");
                    break;
                }
            case Levels.End:
                {
                    SceneManager.LoadSceneAsync("EndScene");
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

    private void GetImage()
    {
        var image = FindObjectOfType<Image>();

        black = image;
        anim = image.gameObject.GetComponent<Animator>();
    }


    public void QuitGame()
    {
        //Todo: Make quitting nice
        Application.Quit();
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel(currentLevel + 1));
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    public void ReloadAfterSeconds(float seconds)
    {
        StartCoroutine(reloadAfterSeconds(seconds));
    }

    private IEnumerator reloadAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReloadScene();
    }
}
