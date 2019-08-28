using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailureHandling : MonoBehaviour
{
    //If the player falls below a certain y value the patient is informed that they failed this level and it will be reset. The level is then reset from the beginning.

    public GameObject levelFailedText;
    private float resetTime;
    private Scene currentLevel;
    private AudioSource audioSource;
    public AudioClip failSound;
    public AudioClip waterSound;
    private bool[] playOnce = new bool[] { true, true };
    public float patientAssistMultiplier;
    private GameObject[] bridges;
    public GameObject Omni;
    public float failureHeight;

    // Start is called before the first frame update
    void Start()
    {
        resetTime = 3.0f;
        levelFailedText.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>();
        bridges = GameObject.FindGameObjectsWithTag("Island");
        Omni = GameObject.Find("The Omnipresent Object");

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < failureHeight)
        {
            levelFailedText.SetActive(true);
            resetTime -= Time.deltaTime;
            if (playOnce[0] == true)
            {
                playOnce[0] = false;
                audioSource.PlayOneShot(failSound);
                Omni.GetComponent<FileHandler>().saveToAttemptLog("F");
            }

        }
        if (resetTime <= 0)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        if (transform.position.y < 0  && playOnce[1] == true)
        {
            playOnce[1] = false;
            audioSource.PlayOneShot(waterSound);
        }
    }
}
