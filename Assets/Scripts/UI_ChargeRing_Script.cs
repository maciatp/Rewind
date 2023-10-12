using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChargeRing_Script : MonoBehaviour
{
    public Vector2 offset;
    public Image ringImage;

    public Color normalColor;
    public Color chargingColor;
    public Color rewindingColor;
    public Color fullColor;
    public Color chargeLowColor;



    public Animator chargeRing_Animator;
    public Transform playerTransform;
    public PlayerMovement_Script playerMovement_Script_;
    public TimeManager_Script timeManager_;

    public Camera cam;


    private void Awake()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {

        chargeRing_Animator = gameObject.GetComponent<Animator>();
        chargeRing_Animator.enabled = false;

        cam = Camera.main;
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement_Script_ = playerTransform.gameObject.GetComponent<PlayerMovement_Script>();
        ringImage = gameObject.GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {

        ////WORLD TO VIEWPORT transforma una posición del mundo a un rango de (0,0) a (1,1) 
        //Vector2 pos = cam.WorldToViewportPoint(playerTransform.position);
        ////VIEWPORT TO SCREEN POINT transforma de (0,0) (1,1) a píxeles
        //ringImage.transform.position = cam.ViewportToScreenPoint(pos + offset);



        ringImage.fillAmount = playerMovement_Script_.currentStamina / playerMovement_Script_.staminaMax;
        
        if((!timeManager_.isRewinding) && (!playerMovement_Script_.isCharging))
        {
            ringImage.color = normalColor;
        }
        
        if((timeManager_.isRewinding) &&  (ringImage.color != rewindingColor)) //(!playerMovement_Script_.isCharging) &&
        {
            
            ringImage.color = rewindingColor;
        }
        if((playerMovement_Script_.isCharging) && (ringImage.color != chargingColor)) //(!timeManager_.isRewinding) && 
        {
            ringImage.color = chargingColor;
        }
       
        if((ringImage.fillAmount <= playerMovement_Script_.staminaMinToShoot/playerMovement_Script_.staminaMax) && (ringImage.color != chargeLowColor))
        {
            ringImage.color = chargeLowColor;
        }
        if((ringImage.fillAmount >= 1f) && (ringImage.color != fullColor))
        {
            ringImage.color = fullColor;
            chargeRing_Animator.enabled = true;
        }else
        {
            chargeRing_Animator.enabled = false;
           // ringImage.color = rewindingColor;
        }
        if(ringImage.fillAmount < 0.99f)
        {

        }

    }

    private void FixedUpdate()
    {
        //WORLD TO VIEWPORT transforma una posición del mundo a un rango de (0,0) a (1,1) 
        Vector2 pos = cam.WorldToViewportPoint(playerTransform.position);
        //VIEWPORT TO SCREEN POINT transforma de (0,0) (1,1) a píxeles
        ringImage.transform.position = cam.ViewportToScreenPoint(pos + offset);
    }
}
