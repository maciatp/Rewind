using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_Script : MonoBehaviour
{
    public GameObject enemy;

    //public int maxEnemies = 100;

    public bool canSpawn = true;
    public float cooldown = 10f;

    public TimeManager_Script timeManager_;
    public GameManager_Script gameManager_;
    public TimeTrap_Script timeTrap_;

    private void Awake()
    {
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("TimeTrap"))
        { 
            timeTrap_ = GameObject.FindGameObjectWithTag("TimeTrap").GetComponent<TimeTrap_Script>();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();  
    }

    public void SpawnEnemy()
    {
        
        if (!canSpawn) return;
        if((timeTrap_) && (timeTrap_.isActivated)) return;
        if (gameManager_.isGameFinished) return;
        //if (GameObject.FindGameObjectsWithTag("Enemy").Length > maxEnemies) return;
        
        GameObject enemy_ = Instantiate(enemy, transform.position, transform.rotation, null);
        timeManager_.enemies.Add(enemy_.GetComponent<TimeBody_Script>());
        gameManager_.AddEnemy();
        StartCoroutine("CanSpawn");
    }

    IEnumerator CanSpawn()
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldown);
        canSpawn = true;
    }
}
