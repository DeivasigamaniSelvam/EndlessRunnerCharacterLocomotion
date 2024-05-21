using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public Animator animator;
    public static PlayerManager instance;
    public AudioSource audiosource;
    public AudioClip[] audioclip;
    public static bool gameOver;
    public GameObject gameOverPanel, ingameui;

    public static bool isGameStarted;

    public GameObject startbutton, Levelcompleted;
    public static int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI newRecordText;
    public TextMeshProUGUI LevelcompText;

    public static bool isGamePaused;
    //public GameObject[] characterPrefabs;


    private void Awake()
    {
        instance = this;
        //int index = PlayerPrefs.GetInt("SelectedCharacter");
        //GameObject go = Instantiate(characterPrefabs[index], transform.position, Quaternion.identity);
    }


    public void run()
    {
        
    }

    public void PlayAudiobyIndex(int index)
    {
        audiosource.clip = audioclip[index];
        audiosource.Play();
    }

    public void restartlevel()
    {
        SceneManager.LoadScene(0);
    }
    public void start()
    {
        isGameStarted = true;
    }

    void Start()
    {
        //Invoke("run", 1.0f);
        score = 0;
        Time.timeScale = 1;
        gameOver = isGameStarted = isGamePaused= false;
        if(PlayerPrefs.HasKey("levelcompletedingame"))
        {
            level =  PlayerPrefs.GetInt("levelcompletedingame", level);
            LevelcompText.text = "Level: "+level.ToString();
        }
        else
        {
            LevelcompText.text = "Level: " + level.ToString();
        }
    }

    void Update()
    {
        //Update UI
        gemsText.text = "Coins: "+PlayerPrefs.GetInt("TotalGems", 0).ToString();
        scoreText.text = "Score: :"+score.ToString();
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            newRecordText.text = "HiScore: " + score;
            PlayerPrefs.SetInt("HighScore", score);
        }
        //Game Over
        if (gameOver)
        {
            //Time.timeScale = 0;
            
            
            gameOverPanel.SetActive(true);
            //Destroy(gameObject);
        }

        //Start Game
        if (SwipeManager.tap  && !isGameStarted)
        {
            animator.SetTrigger("run");
            startbutton.SetActive(false);
            ingameui.SetActive(true);
            isGameStarted = true;
            StartCoroutine(time());
        }
    }

    int timer,level=1;
    int[] timedetails = new int[10] {10,20,30,40,50,80,90,100,140,1500};
    IEnumerator time()
    {
    while (true && timer < timedetails[level-1])
    {
        timeCount();
        yield return new WaitForSeconds(1);
    }
       
    }
    void timeCount()
    {
        timer += 1;
        Debug.LogError(timer);
        Debug.LogError(timer >= timedetails[level - 1]);
        if (timer >= timedetails[level - 1])
        {
            Levelcompleted.SetActive(true);
            animator.SetTrigger("win");
            level++;
            Debug.LogError("l "+level);
            LevelcompText.text = "Level: " + level.ToString();
            PlayerPrefs.SetInt("levelcompletedingame", level);
            StopCoroutine(time());
            if (!isGamePaused && !gameOver)
            {
                //Time.timeScale = 0;
                isGamePaused = true;
            }
            PlayAudiobyIndex(1);
        }
    }


}
