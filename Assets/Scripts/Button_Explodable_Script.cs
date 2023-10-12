using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button_Explodable_Script : MonoBehaviour
{
    public float currentCharge = 0;
    public float maxCharge = 2;

    public Color startColor;
    public Color explodeColor;

    public MeshRenderer meshRenderer;
    public TimeManager_Script timeManager_;

    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.material.color = Color.Lerp(startColor, explodeColor, currentCharge);

        if((currentCharge > 0) && (!timeManager_.isRewinding))
        {
            currentCharge -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            currentCharge += Time.deltaTime * 6;
            if(currentCharge >= maxCharge)
            {
                Explode();
            }
        }
    }


    void Explode()
    {
        //LOAD NEXT LEVEL
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
}
