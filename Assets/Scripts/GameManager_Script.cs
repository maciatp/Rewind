using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager_Script : MonoBehaviour
{

    public float currentEnemies = 0;
    public float maxEnemies = 600;
    public int currentLevel = 0;

    public float difficultyIncreaseEachLevel = 0.8f;

    public bool isBossBeaten = false;

    public bool isGamePaused = false;
    public bool isGameFinished = false;

    public Animator defocusEffect_Animator;

    public Animator uiLevelCompleted_Animator;

    public Animator uiGameOver_Animator;

    public GameObject ui_PauseScreen;

    public GameObject ui_PauseButton;
    //public GameObject pauseScreen;

    public GameObject ui_ThanksScreen;

    public GameObject ui_BossRing;

    public TMPro.TextMeshProUGUI level_text;
    public TMPro.TextMeshProUGUI currentLevel_text;
    public TMPro.TextMeshProUGUI totalLevels_text;


   

    // Start is called before the first frame update
    void Start()
    {
        
        defocusEffect_Animator = GameObject.Find("DefocusEffect_Volume").GetComponent<Animator>();

        defocusEffect_Animator.SetBool("mustDefocus", false);

        level_text = GameObject.Find("GameScreen").transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        currentLevel_text = GameObject.Find("GameScreen").transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>();
        totalLevels_text = GameObject.Find("GameScreen").transform.GetChild(4).GetComponent<TMPro.TextMeshProUGUI>();


        ui_PauseButton = GameObject.Find("GameScreen").transform.GetChild(5).gameObject;
        ui_PauseButton.SetActive(false);

        ui_PauseScreen = GameObject.Find("GameScreen").transform.GetChild(6).gameObject;
        ui_PauseScreen.SetActive(false);

        ui_BossRing = GameObject.Find("GameScreen").transform.GetChild(7).gameObject;
        ui_BossRing.SetActive(false);
        

        if(SceneManager.GetActiveScene().name != "Title_Scene")
        {
            uiLevelCompleted_Animator = GameObject.Find("CongratulationScreen").GetComponent<Animator>();
            uiLevelCompleted_Animator.enabled = false;
            uiLevelCompleted_Animator.gameObject.SetActive(false);

            uiGameOver_Animator = GameObject.Find("GameOverScreen").gameObject.GetComponent<Animator>();
            uiGameOver_Animator.enabled = false;
            uiGameOver_Animator.gameObject.SetActive(false);


            // ui_PauseScreen = GameObject.Find("PauseScreen").gameObject;
            //ui_PauseScreen.SetActive(false);

            ui_PauseButton.SetActive(true);
            

        }
           
        if((SceneManager.GetActiveScene().name == ("Game_Scene")) || (SceneManager.GetActiveScene().name == "Tutorial3_Scene"))
        {
            ui_BossRing.gameObject.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name != "Game_Scene")
        {

            level_text.gameObject.SetActive(false);
            currentLevel_text.gameObject.SetActive(false);
            totalLevels_text.gameObject.SetActive(false);
        }

        if(SceneManager.GetActiveScene().name == "Game_Scene")
        {
            ui_ThanksScreen = GameObject.Find("ThanksForPlayingScreen").gameObject;
            ui_ThanksScreen.GetComponent<Animator>().enabled = false;
            ui_ThanksScreen.SetActive(false);


            level_text.gameObject.SetActive(true);
            currentLevel_text.gameObject.SetActive(true);
            totalLevels_text.gameObject.SetActive(true);


            if(!PlayerPrefs.HasKey("MaxEnemies"))
            {
                PlayerPrefs.SetInt("MaxEnemies", 600);
                Debug.Log("MaxEnemies Es" + maxEnemies);
            }
            maxEnemies = PlayerPrefs.GetInt("MaxEnemies");

            if(!PlayerPrefs.HasKey("CurrentLevel"))
            {
                PlayerPrefs.SetInt("CurrentLevel", 1);
            }
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            currentLevel_text.text = currentLevel.ToString();

        }


    }

    // Update is called once per frame
    void Update()
    {
        if((isBossBeaten) && (!defocusEffect_Animator.GetBool("mustDefocus")))
        {
            defocusEffect_Animator.SetBool("mustDefocus", true);
        }
    }
    private void FixedUpdate()
    {
        
    }


    public void ActivateCongratulationScreen()
    {

        StartCoroutine("CongratulationScreen");
        
    }

    IEnumerator CongratulationScreen()
    {
        yield return new WaitForSecondsRealtime(0.55f);
        Time.timeScale = 0.1f;
        uiLevelCompleted_Animator.gameObject.SetActive(true);
        uiLevelCompleted_Animator.enabled = true;

    }
    

    public void AddEnemy()
    {
        currentEnemies++;
        // Debug.Log(currentEnemies);
        if ((currentEnemies >= maxEnemies) && (SceneManager.GetActiveScene().name == "Game_Scene"))
        {
            Debug.Log("GAME OVER");
            GameOver();
        }

       
    }
    public void DeleteEnemy()
    {
        currentEnemies--;
        if(currentEnemies < 0)
        {
            currentEnemies = 0;
        }
    }




    public void LoadTutorial()
    {
        maxEnemies = 600;
        SceneManager.LoadScene("Tutorial_Scene");

    }
    public void PlayGame()
    {
        
        Time.timeScale = 1f;
        maxEnemies = 600;
        currentLevel = 0;
        PlayerPrefs.SetInt("MaxEnemies", Mathf.RoundToInt(maxEnemies));
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

       // Destroy(GameObject.FindGameObjectWithTag("AudioManager").gameObject);


        SceneManager.LoadScene("Game_Scene");
        //titleScreen.SetActive(false);
        //UI_Ring.SetActive(true);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>().enabled = false;
        //levelGenerator.SetActive(true);
        //UI_Ring.SetActive(true);

    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();


        Application.Quit();
    }

    public void PauseGame()
    {
        if((SceneManager.GetActiveScene().name != "Title_Scene") && (!isGameFinished))
        {
            
            GameObject.Find("TimeManager").GetComponent<TimeManager_Script>().Pause();
            GameObject.Find("AudioManager").GetComponent<AudioSource>().Pause();
            ui_PauseScreen.SetActive(true);
            isGamePaused = true;
            Time.timeScale = 0;
        }
        
    }

    public void ResumeGame()
    {
        if((SceneManager.GetActiveScene().name != "Title_Scene") && (!isGameFinished))
        {

            GameObject.Find("AudioManager").GetComponent<AudioSource>().Play();
            GameObject.Find("GameScreen").transform.Find("Pause_Button").gameObject.SetActive(true);
            ui_PauseScreen.SetActive(false);
            isGamePaused = false;
            Time.timeScale = 1f;
        }
            
    }

    public void BackToMenu()
    {
        //RETURN TO MENU
        Time.timeScale = 1;
        isGameFinished = false;
        PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("MaxEnemies", 600);
        //PlayerPrefs.SetInt("CurrentLevel", 0);

        SceneManager.LoadScene(0);
    }


    public void NextLevel()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevel++;
        maxEnemies = PlayerPrefs.GetInt("MaxEnemies");

        PlayerPrefs.SetInt("MaxEnemies", Mathf.RoundToInt((maxEnemies * difficultyIncreaseEachLevel)));
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        Time.timeScale = 1;
        
        if(currentLevel > 10)
        {
            isGameFinished = true;
            currentLevel_text.gameObject.SetActive(false);
            uiLevelCompleted_Animator.gameObject.SetActive(false);
            GameObject.Find("GameScreen").gameObject.SetActive(false);

            //GAME ENDED
            Debug.Log("Has terminado el juego!");
            
            ui_ThanksScreen.SetActive(true);
            ui_ThanksScreen.GetComponent<Animator>().enabled = true;
            Time.timeScale = 0;




           // SceneManager.LoadScene("Credits_Scene");
        }

        if(currentLevel <= 10)
        {
            SceneManager.LoadScene("Game_Scene");

        }


    }

    public void OpenTwitter()
    {
        Application.OpenURL("https://twitter.com/Macionik");
    }   
    public void OpenLinkedin()
    {
        Application.OpenURL("https://www.linkedin.com/in/maciatp/");
    }

    void GameOver()
    {
        isGameFinished = true;
        Time.timeScale = 0.08f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>().enabled = false;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        StartCoroutine("GameOverTimer");
    }

    IEnumerator GameOverTimer()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        uiGameOver_Animator.gameObject.SetActive(true);
        uiGameOver_Animator.enabled = true;
        

    }


    public void TryAgain()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>().ResetScene();
    }
}
