using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CongratulationScreenEvent_Script : MonoBehaviour
{
   
    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().name == "Game_Scene")
        {
        GameObject.Find("GameManager").GetComponent<GameManager_Script>().NextLevel();

        }
    }

}
