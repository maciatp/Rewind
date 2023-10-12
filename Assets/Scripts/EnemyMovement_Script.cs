using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement_Script : MonoBehaviour
{
    [SerializeField]
    private float chargeBonus = 0.5f;
    public float speed = 0.4f;
    public bool isFrozen = false;

    public float enemyShake = 3f;
    public Rigidbody enemy_rb;
    public TimeBody_Script timeBody_Script_;
    public TimeManager_Script timeManager_;

    public GameManager_Script gameManager_;

    public Color startColor;
    public Color freezeColor;

    public GameObject player;
    public NavMeshAgent navAgent;
    public GameObject enemyExplosion_FX;

    public MeshRenderer meshrenderer;
    public TrailRenderer trail;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        meshrenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();
        trail = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TrailRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        enemy_rb = gameObject.GetComponent<Rigidbody>();
        timeBody_Script_ = gameObject.GetComponent<TimeBody_Script>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        trail.autodestruct = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if((!timeBody_Script_.isRewinding) && (!isFrozen) && (!gameManager_.isGamePaused))
        {
            GoToPlayer();
        }
        
        if((isFrozen) || (timeBody_Script_.isRewinding) || (gameManager_.isGamePaused))
        {
            Freeze();
        }



    }

    public void Freeze()
    {
        navAgent.enabled = false;
        //Debug.Log("Desactivo navAgent");
        if(meshrenderer.material.color != freezeColor)
        {
        meshrenderer.material.color = freezeColor;

        }
    }

    public void GoToPlayer()
    {
        if(meshrenderer.material.color != startColor)
        {
            meshrenderer.material.color = startColor;
        }

        navAgent.enabled = true;
        // MoveEnemyToPlayer();
        navAgent.SetDestination(player.transform.position);
        //Debug.Log("Estoy yendo al player");
    }

    private void MoveEnemyToPlayer()
    {
        Vector3 playerPos = player.transform.position;

        enemy_rb.velocity = new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y, 0).normalized * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "Bullet") )
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        trail.autodestruct = true;
        trail.time = 1.1f;
        trail.transform.parent = null;
        //ENEMY DESTROYED
        timeManager_.enemies.Remove(gameObject.GetComponent<TimeBody_Script>());


        if (player.GetComponent<PlayerMovement_Script>().currentStamina < player.GetComponent<PlayerMovement_Script>().staminaMax)
        {
           player.GetComponent<PlayerMovement_Script>().currentStamina += chargeBonus;
        }
        GameObject enemy_Explosion = Instantiate(enemyExplosion_FX, gameObject.transform.position, enemyExplosion_FX.transform.rotation, null);
        gameObject.GetComponent<CinemachineCollisionImpulseSource>().GenerateImpulse(enemyShake);

        gameManager_.DeleteEnemy();
        
        Destroy(gameObject);
    }
}
