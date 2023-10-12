using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeTrigger_Script : MonoBehaviour
{

    //public bool isCharging = false;
    public CapsuleCollider capsuleCollider;
    public PlayerMovement_Script playerMovement_Script_;
    public float chargeRate = 0.3f;

    public TimeManager_Script timeManager_;
    //public TMPro.TextMeshProUGUI chargingTEXT;

    private void Awake()
    {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        playerMovement_Script_ = gameObject.transform.parent.GetComponent<PlayerMovement_Script>();
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        //chargingTEXT = GameObject.Find("Charging_Text").GetComponent<TMPro.TextMeshProUGUI>();
    }
    private void Start()
    {
        //chargingTEXT.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if((playerMovement_Script_.currentStamina < playerMovement_Script_.staminaMax))
            {
               playerMovement_Script_.isCharging = true;

                if(!timeManager_.isRewinding)
                {
                    playerMovement_Script_.currentStamina += Time.deltaTime * chargeRate;
                    
                }
                else
                {
                    //SI ESTÄ COMENTADO, SÓLO CARGA CUANDO NO DISPARAS (cerca de enemigos)
                    //playerMovement_Script_.staminaLeft += Time.deltaTime * (chargeRate * 2); //chargeRate es 0,pico ; por lo que irá todavía más lento cuando dispares
                }
               // chargingTEXT.gameObject.SetActive(true);

                if(playerMovement_Script_.currentStamina > playerMovement_Script_.staminaMax)
                {
                    playerMovement_Script_.currentStamina = playerMovement_Script_.staminaMax;
                }
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            // chargingTEXT.gameObject.SetActive(false);
           playerMovement_Script_.isCharging = false;
        }
    }
}
