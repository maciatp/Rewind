using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth_Script : MonoBehaviour
{

    public Vector2 offset;
    public Image ringImage;

    public Color good_Color;
    public Color alert_Color;
    public Color deactivated_Color;
   


    public Animator chargeRing_Animator;
    public Transform bossTransform;
    public Boss_Script boss_Script_;
    public TimeManager_Script timeManager_;

    public Camera cam;



    // Start is called before the first frame update
    void Start()
    {

        //chargeRing_Animator = gameObject.GetComponent<Animator>();
        //chargeRing_Animator.enabled = false;

        cam = Camera.main;
        //timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();

        if(GameObject.FindGameObjectWithTag("BossHead"))
        {

        bossTransform = GameObject.FindGameObjectWithTag("BossHead").transform;
        boss_Script_ = bossTransform.gameObject.GetComponent<Boss_Script>();
        ringImage = gameObject.GetComponent<Image>();
        }

    }

    // Update is called once per frame
    void Update()
    {


        ringImage.fillAmount = 1 - (boss_Script_.bossTimer / boss_Script_.bossActivatedTime);
        if((ringImage.fillAmount <= 0.5f) && (ringImage.fillAmount > 0))
        {
            ringImage.color = alert_Color;
        }
        else if((ringImage.fillAmount > 0.5f)&&(ringImage.fillAmount < 1))
        {
            ringImage.color = good_Color;
        }
        if((ringImage.fillAmount == 0) || (ringImage.fillAmount == 1))
        {
            ringImage.color = deactivated_Color;
        }

    }

    private void FixedUpdate()
    {
        //WORLD TO VIEWPORT transforma una posición del mundo a un rango de (0,0) a (1,1) 
        Vector2 pos = cam.WorldToViewportPoint(bossTransform.position);
        //VIEWPORT TO SCREEN POINT transforma de (0,0) (1,1) a píxeles
        ringImage.transform.position = cam.ViewportToScreenPoint(pos + offset);
    }
}
