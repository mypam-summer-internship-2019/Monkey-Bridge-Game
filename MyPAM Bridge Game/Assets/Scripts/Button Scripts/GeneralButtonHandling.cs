using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralButtonHandling : MonoBehaviour
{
    private bool buttonPressed;

    public GameObject nextIsland;
    public float bridgeWidth;

    // Start is called before the first frame update
    void Start()
    {
        buttonPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (buttonPressed == false)
        {
            buttonPressed = true;

            Camera.main.GetComponent<ButtonHandler>().generateBridge(gameObject, nextIsland, bridgeWidth);
            Camera.main.GetComponent<AccuracyCalculator>().displayPopup(true);

            gameObject.SetActive(false);
        }    
    }
}
