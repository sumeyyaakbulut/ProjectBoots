using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    

    bool isTransitioning = false;//only true when we hit something
    bool collisionDissabled = false;

    void Start()
    {
        audioSource=GetComponent<AudioSource>();//because we need to cache the audio source
       
    }
    void Update()
    {
        RespondToDebugKeys();  
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDissabled = !collisionDissabled;//toggle collision
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionDissabled) { return; }
     
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //ActionToTake();
                break;
            case "Finish":
               StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning= true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;//is disabled movement script
        Invoke("ReloadLevel",levelLoadDelay); //reload scene ,after a second the game scene is loading

    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneInex = currentSceneIndex + 1;
        if (nextSceneInex==SceneManager.sceneCountInBuildSettings)//how many scenes, how many level,
        {
            //total number of scenes in the game
            nextSceneInex = 0;
        }
        SceneManager.LoadScene(nextSceneInex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
   
}
