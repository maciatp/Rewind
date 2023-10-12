using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Script : MonoBehaviour
{
    public bool isBossDamageable = false;
    public float bossTimer = 0;
    public float bossActivatedTime = 2f;
    
    
    public Vector3 startPos;



    public List<BossTrigger_Script> bossTriggers;
    public SphereCollider bossCollider;

    public Color deactivatedColor;
    public Color activatedColor;
    public Color hurtColor;
    public Color explodedColor;

    public AudioClip activatedClip;
    public AudioClip deactivatedClip;

    public Animator bossAnimator;

    public TimeManager_Script timeManager_;


    public AudioSource bossAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();

        bossAnimator = gameObject.GetComponent<Animator>();
        bossAnimator.enabled = false;

        bossCollider = gameObject.transform.GetChild(4).gameObject.GetComponent<SphereCollider>();
        bossCollider.enabled = false;
        startPos = bossCollider.gameObject.transform.position;

        bossTriggers.Add(gameObject.transform.GetChild(0).gameObject.GetComponent<BossTrigger_Script>());
        bossTriggers.Add(gameObject.transform.GetChild(1).gameObject.GetComponent<BossTrigger_Script>());
        bossTriggers.Add(gameObject.transform.GetChild(2).gameObject.GetComponent<BossTrigger_Script>());
        bossTriggers.Add(gameObject.transform.GetChild(3).gameObject.GetComponent<BossTrigger_Script>());


        bossAudioSource = gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if((!isBossDamageable) && (bossTriggers[0].isOnHold) && (bossTriggers[1].isOnHold) && (bossTriggers[2].isOnHold) && (bossTriggers[3].isOnHold))
        {
            ActivateBoss();
        }

        if(isBossDamageable)// && (!timeManager_.isRewinding))
        {
            if(!bossAnimator.enabled)
            {
                bossAnimator.enabled = true;
            }
            bossAnimator.speed = 1;
            bossTimer += Time.deltaTime;
            if((bossTimer >= bossActivatedTime) && (bossAnimator.GetBool("isBossDamageable")))
            {

                BossIsNoDamageable();
                
            }
        }

        if((isBossDamageable) && (timeManager_.isRewinding))
        {
            bossAnimator.speed = 0;
        }
        
       
    }

    private void ActivateBoss()
    {
        bossAudioSource.clip = activatedClip;
        bossAudioSource.Play();
        isBossDamageable = true;
        bossCollider.enabled = true;
        bossAnimator.enabled = true;
        bossAnimator.SetBool("isBossDamageable", true);
        
        //LLamo a la rutina cuando termina la animación de idle
        //StartCoroutine("StopBoss");
    }

    ////LA LLAMO DESDE ANIMATION EVENT. por ahora no
    //public void CoroutineStopBoss()
    //{
    //    StartCoroutine("StopBoss");
    //}

    //IEnumerator StopBoss()
    //{
    //    yield return new WaitForSeconds(bossActivatedTime);
    //    //DEACTIVATE COLLIDER AND ANIMATOR
        
    //    //desactivo el boss al final de la animación de esconderse
    //    isBossDamageable = false;
    //    //bossAnimator.SetBool("isBossDamageable", false);
    //    //sólo debe moverse si !Timemanager.rewinding
        
    //    //DeactivateBoss();

    //}

    //la llamo desde un animation event
    void BossIsNoDamageable()
    {
        isBossDamageable = false;
        bossAnimator.SetBool("isBossDamageable", false);
        bossTimer = 0;

        bossAudioSource.clip = deactivatedClip;
        bossAudioSource.Play();

        foreach (BossTrigger_Script bossTrigger_Script in bossTriggers)
        {
            bossTrigger_Script.DeactivateTrigger();
            //Debug.Log("Debería haber 4 mensajes de éstos");
        }
    }

    //la llamo desde un animation event (?? Seguro? comprobarlo (14/08/2020)
    void DeactivateBoss()
    {
        //isBossDamageable = false;
        bossAnimator.enabled = false;
        bossCollider.enabled = false;
    }

   
}
