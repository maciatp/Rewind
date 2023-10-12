using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Button_Script : MonoBehaviour
{
   public void ResetScene()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>().ResetScene();
    }
}
