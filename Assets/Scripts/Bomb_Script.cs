using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;

public class Bomb_Script : MonoBehaviour
{

    public bool isReadyToExplode = false;
    public bool isExploded = false;

    public float currentCharge = 0;
    public float maxCharge = 1f;
    public float chargeRate = 1f;

    public float bombShake = 2.4f;

    public Color startColor;
    public Color readyToExplodeColor;
    public Color explodedColor;

    public float colliderCountdownDeactivation = 2f;
    public float bombCooldown = 5f;

    public TimeManager_Script timeManager_;
    public MeshRenderer meshRenderer;

    public SphereCollider explosionCollider;
    public Animator bombSphere_Animator;
    public ParticleSystem particleSystem;


    public AudioSource bombAudioSource;


    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();

        explosionCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        bombSphere_Animator = transform.GetChild(0).GetComponent<Animator>();

        if((explosionCollider.enabled != false) || (bombSphere_Animator.enabled != false))
        {
            bombSphere_Animator.enabled = false;
            explosionCollider.enabled = false;
        }
        particleSystem = gameObject.GetComponent<ParticleSystem>();

        bombAudioSource = gameObject.GetComponent<AudioSource>();

    }
    // Start is called before the first frame update
    void Start()
    {
        startColor = meshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if((!isExploded) && (isReadyToExplode) && (!timeManager_.isRewinding))
        {
            Explode();
        }
        if((!isExploded) && (!timeManager_.isRewinding) && (currentCharge > 0))
        {
            currentCharge -= Time.deltaTime * chargeRate * 0.5f; //*2 para que se descargue más rápido que lo que se cargue.
            if(currentCharge < 0)
            {
                currentCharge = 0;
            }
        }


        meshRenderer.material.color = Color.Lerp(startColor, readyToExplodeColor, currentCharge);
    }

    private void FixedUpdate()
    {
        
    }

    void RebuildBomb()
    {
        currentCharge = 0;
        isExploded = false;
        isReadyToExplode = false;
        meshRenderer.material.color = startColor;
    }

    void ReadyToExplode()
    {
        isReadyToExplode = true;
        meshRenderer.material.color = readyToExplodeColor;
    }

    void Explode()
    {
        bombAudioSource.Play();
        
        meshRenderer.material.color = explodedColor;
        explosionCollider.enabled = true;
        isExploded = true;
        bombSphere_Animator.gameObject.SetActive(true);
        bombSphere_Animator.enabled = true;
        gameObject.GetComponent<CinemachineCollisionImpulseSource>().GenerateImpulse(bombShake);
        particleSystem.Play();
        StartCoroutine("DeactivateAfterExplosion");
        StartCoroutine("RebuildBombCoroutine");
    }

    IEnumerator DeactivateAfterExplosion()
    {
        yield return new WaitForSeconds(colliderCountdownDeactivation);
        explosionCollider.enabled = false;
        bombSphere_Animator.gameObject.SetActive(false);




    }

    IEnumerator RebuildBombCoroutine()
    {
        
        yield return new WaitForSeconds(bombCooldown);
        RebuildBomb();

    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "Bullet") && (!isExploded))
        {
            // ReadyToExplode();
            currentCharge += Time.deltaTime * chargeRate * 5;
            if(currentCharge >= maxCharge / 2)
            {
               // meshRenderer.material.color = readyToExplodeColor;
            }

            if(currentCharge >= maxCharge)
            {
                Explode();
               
            }


        }

        if((other.tag == "Bomb") && (!isExploded))
        {
            currentCharge = maxCharge;
            Explode();
        }

        //EXPLOSION TRIGGER
        if (other.tag == "Enemy")
        {

          // Debug.Log("Enemigo detectado" + other.name);
            other.transform.parent.GetComponent<EnemyMovement_Script>().DestroyEnemy();

            //Debug.log("Detectado" + other.gameObject.name.ToString());
            //other.GetComponent<EnemyMovement_Script>().DestroyEnemy();
        }

        //if((other.tag == "Bomb") && (isExploded))
        //{
        //    //StartCoroutine("ExplodeOthersCountDown" (other.gameObject));
        //    ExplodeOtherBomb(other.gameObject);
        //}

        

    }

    private void OnParticleTrigger()
    {
       
    }

    //void ExplodeOtherBomb(GameObject otherBomb)
    //{
    //    StartCoroutine("ExplodeOthersCountDown");
    //    otherBomb.gameObject.GetComponent<Bomb_Script>().isReadyToExplode = true;
    //}
    //IEnumerator ExplodeOthersCountDown(GameObject otherBombs)
    //{
    //    yield return new WaitForSeconds(0.8f);
    //    //otherBombs.gameObject.GetComponent<Bomb_Script>().isReadyToExplode = true;

    //}
}
