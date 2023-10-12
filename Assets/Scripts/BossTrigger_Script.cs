using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger_Script : MonoBehaviour
{
    public bool isOnHold = false;

    public float currentCharge = 0;
    public float maxCharge = 1f;

    public float dischargeRate = 1f;

    public Color startColor;
    public Color onHoldColor;
    public Color activatedColor;


    public AudioClip triggered_Sound;
    

    public MeshRenderer meshRenderer;

    public TimeManager_Script timeManager_;

    public AudioSource audioSource_;

    private void Start()
    {
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        audioSource_ = gameObject.GetComponent<AudioSource>();
    }



    private void Update()
    {
        if((isOnHold) && (!timeManager_.isRewinding) && (gameObject.transform.parent.GetComponent<Boss_Script>().isBossDamageable == false))
        {
            currentCharge -= Time.deltaTime * dischargeRate;
            if(currentCharge < 0)
            {
                currentCharge = 0;
                isOnHold = false;
            }

        }
            meshRenderer.material.color = Color.Lerp(startColor, onHoldColor, currentCharge);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            //ACTIVATE TRIGGER

            //Debug.Log("Le he dado al trigger del boss");
            currentCharge += 1f;
            audioSource_.Play();

            if(currentCharge >= 1)
            { 
                currentCharge = 1;
                isOnHold = true;
            }
        }
    }

    public void DeactivateTrigger()
    {
        isOnHold = false;
        currentCharge = 0;
        
    }
}
