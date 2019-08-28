using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private bool buttonPressed;
    private float delay;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        delay = 0.8f;
        buttonPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true || Input.GetMouseButtonDown(1) == true || Input.GetMouseButtonDown(2) == true)
        {
            audioSource.Play();
            buttonPressed = true;
        }


        if (buttonPressed == true)
        {
            delay -= Time.deltaTime;
        }
        if (delay < 0)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            GameObject.Find("The Omnipresent Object").GetComponent<FileHandler>().setLevel(sceneIndex);

            SceneManager.LoadScene(sceneIndex);
        }
    }

    public void pressTheButton()
    {
    
    }
}
