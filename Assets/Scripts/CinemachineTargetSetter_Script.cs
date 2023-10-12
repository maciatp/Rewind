using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetSetter_Script : MonoBehaviour
{
    public GameObject playerInScene;


    private void Start()
    {
        playerInScene = GameObject.FindGameObjectWithTag("Player").gameObject;

        gameObject.GetComponent<CinemachineVirtualCamera>().Follow = playerInScene.transform;// = playerInScene;
    }

}
