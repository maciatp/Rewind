using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VignettingFX_Script : MonoBehaviour
{

    public float vignettingWeight = 0;

    public Volume vignettingVolume;

    public GameManager_Script gameManager_;

    private void Awake()
    {
        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();

        vignettingVolume = gameObject.GetComponent<Volume>();
    }


    private void Update()
    {
        vignettingVolume.weight = gameManager_.currentEnemies / gameManager_.maxEnemies;
    }

}
