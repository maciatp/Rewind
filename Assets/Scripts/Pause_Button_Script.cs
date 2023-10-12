using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Button_Script : MonoBehaviour
{
    

    public void DeactivatePauseButton()
    {
        gameObject.SetActive(false);
    }
}
