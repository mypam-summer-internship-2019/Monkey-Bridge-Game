using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FirstButtonHandling : MonoBehaviour
{
    private bool buttonPressed;

    public GameObject nextIsland;
    public float bridgeWidth;
    public GameObject Omni;

    public GameObject startLabel;
    public GameObject endLabel;



    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;
        Omni = GameObject.Find("The Omnipresent Object");

        startLabel = GameObject.Find("Start Label");
        endLabel = GameObject.Find("End Label");
        startLabel.SetActive(true);
        endLabel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        startLabel.transform.position = screenPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (buttonPressed == false)
        {
            buttonPressed = true;

            startLabel.SetActive(false);
            endLabel.SetActive(false);

            Camera.main.GetComponent<ButtonHandler>().startTimer();
            Omni.GetComponent<FileHandler>().incrementAttemptTally();
            Omni.GetComponent<FileHandler>().GetComponent<FileHandler>().resetTimer();
            Omni.GetComponent<FileHandler>().GetComponent<FileHandler>().setLevelStarted(true);

            Camera.main.GetComponent<AccuracyCalculator>().startScoring();
            Camera.main.GetComponent<AccuracyCalculator>().displayPopup(false);
            Camera.main.GetComponent<ButtonHandler>().generateBridge(gameObject, nextIsland, bridgeWidth);
        }
    }
}

