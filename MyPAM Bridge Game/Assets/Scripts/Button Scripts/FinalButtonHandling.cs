using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalButtonHandling : MonoBehaviour
{

    //the button on the final island does not activate a bridge. Instead, it loads the next level (after a short delay where the patient is informed of their success).

    private bool buttonPressed;
    public AudioSource audioSource;
    public AudioClip levelComplete;
    public AudioClip highscoreSFX;

    public GameObject endLabel;


    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;
        audioSource = gameObject.GetComponent<AudioSource>();

        endLabel = GameObject.Find("End Label");

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        endLabel.transform.position = screenPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (buttonPressed == false)
        {
            buttonPressed = true;

            Camera.main.GetComponent<ButtonHandler>().stopTimer();
            Camera.main.GetComponent<ButtonHandler>().displayTime();
            Camera.main.GetComponent<ButtonHandler>().completeLevel();
            Camera.main.GetComponent<AccuracyCalculator>().displayPopup(true);
            Camera.main.GetComponent<AccuracyCalculator>().displayFinalScore();

            if (Camera.main.GetComponent<AccuracyCalculator>().newHighscore == true)
            {
                audioSource.PlayOneShot(highscoreSFX);
            }
            else
            {
                audioSource.PlayOneShot(levelComplete);
            }

        }


    }
}
