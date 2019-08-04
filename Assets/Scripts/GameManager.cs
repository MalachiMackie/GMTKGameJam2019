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

    private AudioSource AudioSource;
    private AudioSource[] AudioSources;
    private AudioClip MainMenuMusic;
    private AudioClip LevelMusic;
    private AudioClip DeathAudio;


    // Start is called before the first frame update
    void Start()
    {
        hasPlayer = GetPlayer();
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        //push image to the back
        //black.transform.position += new Vector3(0, 0, 10);

        StartCoroutine(InitialFade());

        AudioSources = this.GetComponents<AudioSource>();
        AudioSource = AudioSources[0];
        MainMenuMusic = AudioSources[0].clip;
        LevelMusic = AudioSources[1].clip;
        DeathAudio = AudioSources[2].clip;

        //Fade in Menu music
        StartCoroutine(AudioFadeIn(AudioSource, MainMenuMusic, 1));
        
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

        if (currentLevel == Levels.Menu)
        {
            StartCoroutine(AudioFadeOut(AudioSource, MainMenuMusic, 1));
        }
        
        // pull image to the front
        //black.transform.position += new Vector3(0, 0, -10);

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
            default:
                {
                    SceneManager.LoadSceneAsync($"Level {level.ToString()}");
                    PlayLevelMusic();
                    break;
                }
        }
    }

    void PlayLevelMusic()
    {
        if (currentLevel == Levels.One)
        {
            StartCoroutine(AudioFadeIn(AudioSource, LevelMusic, 1));
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


    // AUDIO FROM HERE //

    public static IEnumerator AudioFadeOut(AudioSource audioSource, AudioClip audioClip, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator AudioFadeIn(AudioSource audioSource, AudioClip audioClip, float FadeTime)
    {
        float startVolume = 0;
        audioSource.PlayOneShot(audioClip);
        while (audioSource.volume < audioSource.volume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
    }

    public void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayerDeathAudio ()
    {
        AudioSource.PlayOneShot(DeathAudio);
    }

}