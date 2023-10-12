using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menus_Script : MonoBehaviour
{
    public bool menuOnStartup = false;

    public GameManager_Script gameManager_;

    //public GameObject titleScreen;
    //public GameObject UI_Ring;
    //public GameObject levelGenerator;



    private void Awake()
    {
        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();

        //if(menuOnStartup)
        //{
        //    levelGenerator.SetActive(false);
        //    UI_Ring.SetActive(false);

        //}
    }
    // Start is called before the first frame update
    void Start()
    {
        //if(menuOnStartup)
        //{
        //    titleScreen.SetActive(true);
        //    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>().enabled = false;
        //}
        //else
        //{
        //    titleScreen.SetActive(false);
        //    levelGenerator.SetActive(true);
        //    UI_Ring.SetActive(true);
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial_Scene");
    }
    public void PlayGame()
    {

        SceneManager.LoadScene("Game_Scene");

        //titleScreen.SetActive(false);
        //UI_Ring.SetActive(true);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>().enabled = false;
        //levelGenerator.SetActive(true);
        //UI_Ring.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
