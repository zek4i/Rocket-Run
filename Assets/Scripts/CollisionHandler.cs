using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delaySpeed = 1f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    bool isTransitioning = false;
    bool collisionDisabled = false;
    AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); //caching
    }
    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            NextLevelUnder();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //toggle collision
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if(isTransitioning || collisionDisabled) { return; } //if we are not transitioning do the rest
        //if we collide w smth we dont need to know bout it
        switch(other.gameObject.tag)
        {
            case "Friendly": //in switch the downside is we need to use string
                Debug.Log("HIT AAAAAAAA!!");
                break;

            case "Finish":
                StartSuccessSequence();
                break;
                //add fuel in the future maybe
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence() //stopping movement after collision and relaoding level
    {
        isTransitioning = true; //we are transitioning to reload level here
        audioSource.Stop();
        audioSource.PlayOneShot(crash);
        crashParticles.Play(); //playing the crash particles
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delaySpeed);
    }

    void StartSuccessSequence() //loading next level if successs
    {
        isTransitioning = true; //we are transitioning to next level here
        audioSource.Stop(); //stopping all audio after crash
        audioSource.PlayOneShot(success);
        successParticles.Play(); //playing the success particles
        GetComponent<Movement>().enabled = false;
        Invoke("NextLevelUnder" , delaySpeed);
    }
    void ReloadLevel() //reloading level logic
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex; //returns the scene index
        SceneManager.LoadScene(CurrentSceneIndex); //making code more readable
    }

    void NextLevelUnder() //next level logic
    {
        int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int NextSceneIndex = CurrentSceneIndex + 1;
        if(NextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            NextSceneIndex = 0; //loading back to lv 1 if no of next scene equal to total no of scene
        } 
        SceneManager.LoadScene(NextSceneIndex);

        //SceneManager.LoadScene(CurrentSceneIndex + 1); this was before the return back to scene 1
    }
}   
