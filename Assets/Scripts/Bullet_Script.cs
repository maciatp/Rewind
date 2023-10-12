using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    [SerializeField]
    private float speed = 4f;
    [SerializeField]
    private float bulletDuration = 5f;
    public float bulletShake = 1f;
    public GameObject bulletExplosion_FX;
    public TrailRenderer trail;

    public TimeManager_Script timeManager_;
    private void Awake()
    {
        trail = gameObject.transform.GetChild(0).GetComponent<TrailRenderer>();
        timeManager_ = GameObject.Find("TimeManager").GetComponent<TimeManager_Script>();
    }
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("DestroyBulletAfterTime");
    }

    IEnumerator DestroyBulletAfterTime()
    {
        yield return new WaitForSeconds(bulletDuration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!timeManager_.isRewinding)
        {

        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player")
        {
            //Debug.Log("Destruyo Bala");
            //DESTROY BULLET
            //PLAY EXPLOSION_FX
            GameObject bulletExplosion_GO = GameObject.Instantiate(bulletExplosion_FX, new Vector3(transform.position.x, transform.position.y, 0.05f) , bulletExplosion_FX.transform.rotation, null);
            trail.autodestruct = true;
            trail.transform.SetParent(null);
            gameObject.GetComponent<CinemachineCollisionImpulseSource>().GenerateImpulse(bulletShake);
            Destroy(gameObject);
        }
        
    }
}
