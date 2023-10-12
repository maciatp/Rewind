using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSphere_Script : MonoBehaviour
{

    public float currentBossHealth;
    public float maxBossHealth = 10;
    public float chargeRate = 1;
    public float shakeRate = 1;

    public float waitToLoadNextLevel = 3;

    public Color deactivatedColor;
    public Color activatedColor;
    public Color hurtColor;
    public Color explodedColor;

    public MeshRenderer meshRenderer;
    public Boss_Script boss_Script_;
    public TimeManager_Script timeManager_;

    public ParticleSystem particleSystem;

    public AudioSource bossSphereAudiosource;


    private void Start()
    {
        currentBossHealth = maxBossHealth;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();

        boss_Script_ = gameObject.transform.parent.GetComponent<Boss_Script>();
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        particleSystem = gameObject.GetComponent<ParticleSystem>();

        bossSphereAudiosource = gameObject.GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(boss_Script_.isBossDamageable)
        {

            meshRenderer.material.color = Color.Lerp(activatedColor, hurtColor, currentBossHealth); //* 0.1f);

            if(!timeManager_.isRewinding)
            {
                currentBossHealth += Time.deltaTime * chargeRate;
                
                if(currentBossHealth > maxBossHealth)
                { 
                    currentBossHealth = maxBossHealth;
                }

            }

        }
        else
        {
            meshRenderer.material.color = Color.Lerp(deactivatedColor, activatedColor, Time.deltaTime);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "Bullet") && (boss_Script_.isBossDamageable))
        {
            currentBossHealth -= 1;
            if(currentBossHealth <= 0)
            {
                DestroyBoss();
                //Debug.Log("Te has cargado al jefe");
            }
        }
    }

    private void DestroyBoss()
    {
        bossSphereAudiosource.Play();
        gameObject.transform.parent.gameObject.GetComponent<Boss_Script>().bossAudioSource.Stop();

        currentBossHealth = 0;
        meshRenderer.material.color = explodedColor;
        particleSystem.gameObject.transform.position = gameObject.transform.parent.GetComponent<Boss_Script>().startPos;
        particleSystem.Play();
        meshRenderer.enabled = false;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        GameObject.Find("DefocusEffect_Volume").GetComponent<Animator>().SetBool("mustDefocus", true);
        gameObject.GetComponent<CinemachineCollisionImpulseSource>().GenerateImpulse(shakeRate);

        GameObject.Find("GameManager").GetComponent<GameManager_Script>().isBossBeaten = true;
       // if(SceneManager.GetActiveScene().name == "Game_Scene")
        {
            GameObject.Find("GameManager").GetComponent<GameManager_Script>().ActivateCongratulationScreen();

        }

        StartCoroutine("LoadNextLevel");

        
    }


    IEnumerator LoadNextLevel()
    {

        yield return new WaitForSecondsRealtime(waitToLoadNextLevel);
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene().name == "Tutorial3_Scene")
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
            PlayerPrefs.SetInt("MaxEnemies", 600);
            PlayerPrefs.DeleteAll();
            //Destroy(GameObject.FindGameObjectWithTag("AudioManager").gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            //LEVEL MANAGER CREATE LEVEL 
            //GAME MANAGER DECREASE MAX ENEMIES
            GameObject.Find("GameManager").GetComponent<GameManager_Script>().NextLevel();
        }
    }
}
