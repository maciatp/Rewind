using Cinemachine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement_Script : MonoBehaviour
{

    public InputActions inputActions;
    public Gamepad gamepad;// = Gamepad.current;

    [SerializeField] bool isUsingTouchControls = false;
    [SerializeField] Joystick move_Joystick;
    [SerializeField] Joystick aim_Joystick;

    public bool isUsingGamepad = false;
    [SerializeField]
    private float moveSpeed = 3;
    [SerializeField]
    private Vector2 moveInput;
    [SerializeField]
    private Vector3 moveDirection;
    [SerializeField]
    private Vector2 aimDirection;
    [SerializeField]
    private bool isShootingButtonPressed = false;
    [SerializeField]
    private float shootCooldown = 0.1f;
    [SerializeField]
    private bool canShoot = true;
    public float currentStamina = 0f;
    public float staminaMinToShoot = 1f;
    public float staminaMax = 7f;
    public float chanceOfSpawningEnemiesWhenShooting = 0.3f;
    public bool isCharging = false;

    public float shootShake = 1.2f;

    public TMPro.TextMeshProUGUI staminaText;

    public List<EnemySpawner_Script> enemySpawners_;


    public TimeManager_Script timeManager_;
    public GameManager_Script gameManager_;

    public Rigidbody rb;
    public Camera cam;

    public GameObject bullet;
    public Transform bulletDirection;



    public AudioSource playerAudioSource;


    

    private void Awake()
    {
        cam = Camera.main;
        inputActions = new InputActions();
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        rb = gameObject.GetComponent<Rigidbody>();
        //staminaText = GameObject.Find("Stamina_Text").GetComponent<TMPro.TextMeshProUGUI>();

        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();


        playerAudioSource = gameObject.GetComponent<AudioSource>();
    }
   
    
   
    // Start is called before the first frame update
    void Start()
    {
        gamepad = Gamepad.current;

        //Debug.Log(gamepad);


        if (gamepad == null)
        {
           // Debug.Log("Juego con teclado");
            isUsingGamepad = false;
        }
        else
        {
            isUsingGamepad = true;
           // Debug.Log("uso mando");
        }

        currentStamina = staminaMax;

        // enemySpawners_ = GameObject.FindGameObjectsWithTag("EnemySpawners").Clone();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("EnemySpawner").Length; i++)
        {
            enemySpawners_.Add(GameObject.FindGameObjectsWithTag("EnemySpawner")[i].GetComponent<EnemySpawner_Script>());
           // Debug.Log("i vale =" + i);
        }

        // LO SIGUIENTE SE VA ACTIVANDO SEgÚN el target build SEA STANDALONE, IOS, ANDROID...

#if UNITY_EDITOR
        GameObject.Find("GameScreen").transform.Find("UI_Touch").gameObject.SetActive(false);
        isUsingTouchControls = false;
#endif
#if UNITY_STANDALONE
        GameObject.Find("GameScreen").transform.Find("UI_Touch").gameObject.SetActive(false);
        isUsingTouchControls = false;
#endif

#if UNITY_IOS || UNITY_ANDROID
        //UI TOUCH CONTROLS
        if(!GameObject.Find("GameScreen").transform.Find("UI_Touch").gameObject.activeSelf)
        {

            GameObject.Find("GameScreen").transform.Find("UI_Touch").gameObject.SetActive(true);
        }
        if(GameObject.Find("UI").transform.Find("Canvas").transform.Find("TitleScreen"))
        {
            GameObject.Find("Quit_Button").gameObject.SetActive(false);

        }
        if(GameObject.Find("UI_Touch").gameObject != null)
        {
            isUsingTouchControls = true;
            move_Joystick = GameObject.Find("UI_Touch").transform.GetChild(0).GetComponent<Joystick>();
            aim_Joystick = GameObject.Find("UI_Touch").transform.GetChild(1).GetComponent<Joystick>();
        }
#endif

#if UNITY_ANDROID
        //UI TOUCH CONTROLS
        GameObject.Find("GameScreen").transform.Find("UI_Touch").gameObject.SetActive(true);
        if(GameObject.Find("UI_Touch").gameObject != null)
        {
            isUsingTouchControls = true;
            move_Joystick = GameObject.Find("UI_Touch").transform.GetChild(0).GetComponent<Joystick>();
            aim_Joystick = GameObject.Find("UI_Touch").transform.GetChild(1).GetComponent<Joystick>();
        }
#endif

    }

    void Update()
    {
        

        //IF UI TOUCH ACTIVE
        if(isUsingTouchControls)
        {
            moveDirection.x = move_Joystick.Horizontal;
            moveDirection.y = move_Joystick.Vertical;


            if ((aim_Joystick.Direction.magnitude >= 0.03) && (!gameManager_.isGamePaused))
            {

                float angle = Mathf.Atan2(aim_Joystick.Vertical, aim_Joystick.Horizontal) * Mathf.Rad2Deg - 90;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                isShootingButtonPressed = true;
            }
            else
            {
                isShootingButtonPressed = false;
            }

        }
        //NOT TOUCH CONTROLS
        else
        {

        //Igualo el input de los controles al vector de movimiento del player
        moveDirection.x = moveInput.x;// * moveSpeed * Time.deltaTime;
        moveDirection.y = moveInput.y;// * moveSpeed * Time.deltaTime;
        }


      
        //MOUSE POINTING

        if(!isUsingGamepad && !isUsingTouchControls)
        {

            // Cojo la ScreenPointPos del ratón con input actions, la ScreenPos del Player, hago un vector, calculo el ángulo ATAN2, y seteo la rotación del player
            Vector2 mouseScreenPosition = inputActions.PlayerControls.MousePosition.ReadValue<Vector2>();
                  
            Vector2 playerScreenPos = cam.WorldToScreenPoint(gameObject.transform.position);        

            Vector2 targetDirection = (mouseScreenPosition - playerScreenPos);
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg -90;// -90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //Debug.Log(targetDirection );
        }

        // staminaText.text = staminaLeft.ToString("0.00");
       // Debug.Log(cam.transform.position);

        if (gamepad == null)
        {
            //Debug.Log("Juego con teclado");

            isUsingGamepad = false;
        }
        else
        {
            isUsingGamepad = true;
            //Debug.Log("uso mando");
        }

    }

    private void FixedUpdate()
    {

        if(!gameManager_.isGamePaused)
        {

            //MOVE
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y).normalized * moveSpeed;
            //MovePlayer();


            if ((isShootingButtonPressed) && (currentStamina > staminaMinToShoot))
            {

                

                //SHOOT
                Shoot();
                timeManager_.isRewinding = true;
            }
            else if (!isShootingButtonPressed)
            {
                timeManager_.isRewinding = false;
            }

            //AIM
            //Aim();
        }



        


    }

    

    void Shoot()
    {
        if (!canShoot) return;

        playerAudioSource.Play();

        if (gameManager_.currentEnemies > (gameManager_.maxEnemies * 0.55f))
        {
            chanceOfSpawningEnemiesWhenShooting /= 10;
        }
        else if(gameManager_.currentEnemies > (gameManager_.maxEnemies * 0.75f))
        {
            chanceOfSpawningEnemiesWhenShooting /= 20;
        }

        if (Random.value > 1f - chanceOfSpawningEnemiesWhenShooting) //hago la inversa porque así en el editor se ve el porcentaje que tiene que superar para spawnear enemigo
        {
            //GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner_Script>().canSpawn = true;
            //GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner_Script>().SpawnEnemy();
            int i = 0;
            foreach(EnemySpawner_Script enemySpawner_Script in enemySpawners_)
            {
                enemySpawners_[i].canSpawn = true;
                enemySpawners_[i].SpawnEnemy();
                i++;
               // Debug.Log("He spawneado un enemigo disparando! "+ enemySpawners_[i].gameObject.name);
            }
            
        }


        gameObject.GetComponent<CinemachineCollisionImpulseSource>().GenerateImpulse(shootShake);

        //Debug.Log("He disparado");
        GameObject bullet_GO = Instantiate(bullet, bulletDirection.position, bulletDirection.rotation, null);
        StartCoroutine("CanShoot");

        
        
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSecondsRealtime(shootCooldown);
        canShoot = true;
    }


    void MovePlayer()
    {
        //CALCULO Nueva posición
        Vector3 newPosition = new Vector3(this.transform.position.x + moveDirection.x, this.transform.position.y, (this.transform.position.z + moveDirection.z));

        //Aplico nueva posición
        this.transform.position = newPosition;


    }

   

    //CONTROLS

    void OnMove(InputValue joystickValue)
    {
        //Igualo el context de los controles a moveInput
        moveInput = joystickValue.Get<Vector2>();


    }


    void OnAim(InputValue joystickValue)
    {
        aimDirection = joystickValue.Get<Vector2>();



        if((aimDirection.magnitude >= 0.03) && (!gameManager_.isGamePaused))
        {

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }

    void OnShoot(InputValue buttonValue)
    {
        if(!gameManager_.isGamePaused)
        {
            if (buttonValue.isPressed)
            {
                //shoot is pressed
                isShootingButtonPressed = true;


                //TimeManager Start Rewind
                //timeManager_.StartRewind();

            }
            else
            {
                isShootingButtonPressed = false;
                //shoot is released
                //timeManager_.StopRewind();
            }
        }
        

       

    }

    void OnReset(InputValue buttonValue)
    {
        ResetScene();
    }

    public void ResetScene()
    {
       // if ((!gameManager_.isGamePaused))// && (!gameManager_.isGameFinished))
        {
            GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager_Script>().ResetSong();
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnPause()
    {
        if(gameManager_.isGamePaused)
        {
            gameManager_.ResumeGame();
        }
        else
        {

        gameManager_.PauseGame();
        }
    }

    //void OnRewind(InputValue buttonValue)
    //{
    //    if(buttonValue.isPressed)
    //    {        
    //        timeManager_.StartRewind();
    //    }
    //    else
    //    {
    //        timeManager_.StopRewind();
    //    }
    //}

    private void OnEnable()
    {
        inputActions.Enable();
       
        //MIRAR CUANDO HAY CONTROLLER y desactivar controles mouse
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

}
