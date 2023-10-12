using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger_EnemySpawner_Script : MonoBehaviour
{
    public EnemySpawner_Script enemySpawner_;

    private void Awake()
    {
        enemySpawner_ = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner_Script>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            enemySpawner_.enabled = true;
        }
    }

}
