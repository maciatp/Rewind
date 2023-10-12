using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrap_Script : MonoBehaviour
{
    public bool isOnHold = false;
    public bool isActivated = false;
    public float currentCharge = 0;
    public float dischargeRate = 1;

    public float freezeTimeSpan = 4f;


    public Color startColor;
    public Color holdColor;
    public Color activatedColor;

    public AudioClip onHold_Audio;
    public AudioClip activated_Audio;
    public AudioClip deactivated_Audio;

    public AudioSource audioSource;

    public List<TimeTrap_Script> timeTraps;

    public List<GameObject> enemies;
    //public GameObject enemyAlone;

    public MeshRenderer meshRenderer;
    public ParticleSystem particleSystem_;

    public TimeManager_Script timeManager_;

    private void Awake()
    {
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        particleSystem_ = gameObject.GetComponent<ParticleSystem>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = onHold_Audio;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("TimeTrap").Length; i++)
        {
            timeTraps.Add(GameObject.FindGameObjectsWithTag("TimeTrap")[i].GetComponent<TimeTrap_Script>());
            //Debug.Log("i vale =" + i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((currentCharge > 0) && (!timeManager_.isRewinding) && (!isActivated))
        {
            
            meshRenderer.material.color = Color.Lerp(startColor, holdColor, currentCharge);
            currentCharge -= Time.deltaTime * dischargeRate;
            if(currentCharge < 0)
            {
                currentCharge = 0;
                isOnHold = false;
            }
        }

        
        if((!isActivated) && (timeTraps[0].isOnHold) && (timeTraps[1].isOnHold) && (timeTraps[2].isOnHold) && (timeTraps[3].isOnHold))
        {
            //ACTIVATE TRAP
            //Debug.Log("he activado la trampa!");
            //FreezeEnemies();
            isActivated = true;
            audioSource.clip = activated_Audio;
            audioSource.loop = true;
            audioSource.Play();
            StartCoroutine("FreezedCountdown");
        }

        if(isActivated)
        {
            FreezeEnemies();
        }

    }

    
    void FreezeEnemies()
    {
        isActivated = true;
        meshRenderer.material.color = activatedColor;
        if(!particleSystem_.isPlaying)
        {

        particleSystem_.Play();
        }

        //enemyAlone.GetComponent<EnemyMovement_Script>().Freeze();


        int i = 0;
        foreach(GameObject enemy in enemies)
        {
            if(enemy != null)
            {

                enemy.transform.parent.GetComponent<EnemyMovement_Script>().isFrozen = true;
                i++;
            }

                //Debug.Log("he congelado");
        }
        //StartCoroutine("FreezedCountdown");
        
    }


    IEnumerator FreezedCountdown()
    {
        yield return new WaitForSeconds(freezeTimeSpan);
        audioSource.loop = false;
        audioSource.clip = deactivated_Audio;
        audioSource.Play();
        isActivated = false;
        isOnHold = false;
        meshRenderer.material.color = startColor;
        //currentCharge = 0;
        foreach(GameObject enemy in enemies)
        {
            if(enemy != null)
            {
                enemy.transform.parent.GetComponent<EnemyMovement_Script>().isFrozen = false;
            }
            
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            //activate Trigger
            audioSource.clip = onHold_Audio;
            audioSource.Play();
            meshRenderer.material.color = holdColor;
            isOnHold = true;
            currentCharge += 1f;
            if(currentCharge > 1)
            {
                currentCharge = 1;
            }



        }

        if(other.tag == "Enemy")
        {
           // Debug.Log("Ha entrado un enemigo!" + other.name);
            enemies.Add(other.gameObject);
            //enemyAlone = (other.gameObject);
        }

        //Debug.Log(other.gameObject.name);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //enemyAlone = null;
            enemies.Remove(other.gameObject);
           // enemies.Remove(other.gameObject.GetComponent<EnemyMovement_Script>());
        }
    }

    void OnTriggerStay(Collider other)
    {
        
    }
}
