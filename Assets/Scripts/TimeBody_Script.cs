using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeBody_Script : MonoBehaviour
{

    public bool isRewinding = false;
  //  public float secondsToRewind = 5;

    List<PointInTime> pointsInTime;

    public Rigidbody rb;


    public TimeManager_Script timeManager_;
    public GameManager_Script gameManager_;


    private void Awake()
    {
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();

    }


    // Start is called before the first frame update
    void Start()
    {
       
        rb = gameObject.GetComponent<Rigidbody>();
        pointsInTime = new List<PointInTime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if((timeManager_.isRewinding) && (!gameManager_.isGamePaused))
        {
            Rewind();
            isRewinding = true;
        }
        else if((!timeManager_.isRewinding) && (!gameManager_.isGamePaused))
        {
            Record();
            isRewinding = false;
        }
    }

    void Rewind()
    {
        //hago que la posición sea la posición de la lista 0, y borro el elemento cada frame para que "rebobine"
        if(pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            if(!isRewinding)
            {
            StopRewind();

            }
            

        }
    }

    void Record()
    {
        if(pointsInTime.Count > Mathf.Round((1f / Time.fixedDeltaTime) * timeManager_.secondsToRewind))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }
        //lo meto en el 0 porque así la última es la primera
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }


    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }
    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        //timeManager_.isRewinding = false;
    }
}
