using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager_Script : MonoBehaviour
{
    public AudioClip songTitle;
    public AudioClip songGameplay;

    public AudioSource mainAudioSource;

    private void Awake()
    {
        mainAudioSource = gameObject.GetComponent<AudioSource>();



      

       if(SceneManager.GetActiveScene().name == "Title_Scene")
       {
            mainAudioSource.clip = songTitle;
            mainAudioSource.Play();
            //DontDestroyOnLoad(gameObject);
       }
       else if(SceneManager.GetActiveScene().name.Contains("Tutorial"))
       {
            mainAudioSource.Stop();
       }
       else if(SceneManager.GetActiveScene().name == "Game_Scene")
        {
            ResetSong();
          

        }
       


        
    }
    // Start is called before the first frame update
    void Start()
    {
        //mainAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ResetSong()
    {
        if (SceneManager.GetActiveScene().name == "Game_Scene")
        {
            if (Random.value >= 0.75f)
            {
                mainAudioSource.clip = songTitle;
            }
            else
            {
                mainAudioSource.clip = songGameplay;
            }
            mainAudioSource.Play();
        }
    }
}
