using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TimeManager_Script : MonoBehaviour
{


    public bool isRewinding = false;
    public float secondsToRewind = 5;
    public float staminaDroppingRate = 2f;

    //How strong the slowmo is
    public float slowdownFactor = 0.05f;
    //How long is the transition to slowmo and inverse
    public float slowdownLenth = 2f;


    


    public Animator rewindEffect_animator;
    public GameObject rewindFX_Cube;
    public Color colorActivated;
    public Color colorDeactivated;


    public PlayerMovement_Script playerMovement_Script_;
    public GameManager_Script gameManager_;

    public AudioManager_Script audioManager_;

    //public TimeBody_Script enemy;
    public List<TimeBody_Script> enemies;





    private void Awake()
    {
        
        rewindEffect_animator = GameObject.Find("RewindEffect_Volume").GetComponent<Animator>();
        if(rewindEffect_animator.gameObject.activeSelf != true)
        {
            rewindEffect_animator.gameObject.SetActive(true);
        }
        
        if(rewindFX_Cube.activeSelf != false)
        {
            rewindFX_Cube.SetActive(false);
        }
        //rewindFX_Cube = GameObject.Find("RewindEffect_Cube");

        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();

        audioManager_ = GameObject.Find("AudioManager").GetComponent<AudioManager_Script>();
    }

    private void Start()
    {
        playerMovement_Script_ = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Script>();
        
        
        
        //SET FX CUBE TO INACTIVE
        rewindFX_Cube.SetActive(false);
    }
    private void Update()
    {
        //Time.timeScale += (1f / slowdownLenth) * Time.unscaledDeltaTime;
        //Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }

    private void FixedUpdate()
    {

        if((isRewinding) && (playerMovement_Script_.currentStamina > 0) && (!gameManager_.isGamePaused))
        {

            audioManager_.gameObject.GetComponent<AudioSource>().pitch = 0.987f; //0.985f;?

            playerMovement_Script_.currentStamina -= Time.deltaTime * staminaDroppingRate;
           

            if(playerMovement_Script_.currentStamina <= 0)
            {
                isRewinding = false;
                playerMovement_Script_.currentStamina = 0;

                


            }
        }

        if((!isRewinding) && (audioManager_.gameObject.GetComponent<AudioSource>().pitch != 1.002f))
        {
            audioManager_.gameObject.GetComponent<AudioSource>().pitch = 1.002f;
        }


        //REWIND FX
        if((isRewinding) && (!gameManager_.isGamePaused))
        {
            rewindEffect_animator.SetBool("isRewinding", true);
            if(rewindFX_Cube.activeSelf != true)
            {
                rewindFX_Cube.SetActive(true);
               // rewindFX_Cube.GetComponent<MeshRenderer>().material.color = colorActivated; 
            }
        }
        else if((!isRewinding) && (!gameManager_.isGamePaused))
        {
            rewindEffect_animator.SetBool("isRewinding", false);
            if(rewindFX_Cube.activeSelf != false)
            {
                rewindFX_Cube.SetActive(false);
                //rewindFX_Cube.GetComponent<MeshRenderer>().material.color = colorDeactivated;
            }

        }
        else if(((isRewinding)) &&(gameManager_.isGamePaused))
        {
            rewindEffect_animator.SetBool("isRewinding", false);
            if (rewindFX_Cube.activeSelf != false)
            {
                rewindFX_Cube.SetActive(false);
                //rewindFX_Cube.GetComponent<MeshRenderer>().material.color = colorDeactivated;
            }
        }

    }



    public void Pause()
    {
        rewindEffect_animator.SetBool("isRewinding", false);
        if (rewindFX_Cube.activeSelf != false)
        {
            rewindFX_Cube.SetActive(false);
            //rewindFX_Cube.GetComponent<MeshRenderer>().material.color = colorDeactivated;
        }
    }






    //public void StartRewind()
    //{
    //    //foreach (TimeBody_Script enemy in enemies)
    //    //{
    //    //    enemy.StartRewind();
    //    //}

    //    //enemy.StartRewind();
    //    isRewinding = true;

    //    //rewindingText.gameObject.SetActive(true);

       
    //}
    //public void StopRewind()
    //{
    //    //foreach(TimeBody_Script enemy in enemies)
    //    //{
    //    //    enemy.StopRewind();
    //    //}

    //    //enemy.StopRewind();
    //    isRewinding = false;

    //    //rewindingText.gameObject.SetActive(false);

        

    //}

}
