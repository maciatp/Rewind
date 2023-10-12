using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom_Script : MonoBehaviour
{
    public float zoomInFactor = 3.8f;
    public float originalOrthoSize = 0;


    public CinemachineVirtualCamera cmVirtualCamera;
    public EnemySpawner_Script enemySpawner;

    public GameObject groundFollowTarget;

    private void Awake()
    {
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner_Script>();
        enemySpawner.enabled = false;
        cmVirtualCamera = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
       originalOrthoSize = cmVirtualCamera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            cmVirtualCamera.m_Lens.OrthographicSize = zoomInFactor;
            cmVirtualCamera.m_LookAt = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") 
        {
            cmVirtualCamera.m_Lens.OrthographicSize = originalOrthoSize;
            cmVirtualCamera.m_LookAt = groundFollowTarget.transform;

            //enemySpawner.enabled = true;
        }
    }
}
