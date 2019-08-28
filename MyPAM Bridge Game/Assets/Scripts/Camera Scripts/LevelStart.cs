using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelStart : MonoBehaviour
{
    //automatically moves the player to the first island as if it were controlled by the myPAM, so they don't start by falling into the water because the myPAM was in the wrong place
    //the player is frozen (doesn't fall) and will follow the myPAM, which will move without patient input to the correct place
    //the patient is then given control over the player and will move under the myPAM joystick's assisstance

    private GameObject thePlayer;
    public GameObject startIsland;
    public Vector3 startCoordinates;
    public Vector3 tempCoordinates;
    public Vector3 currentCoordinates;
    public Vector3 travelVector;
    public bool levelStarted;
    public GameObject Omni;
    public Transform positionIndicator;
    public float deltaX;
    public float deltaZ;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindWithTag("Player");
        positionIndicator = transform.Find("Position Indicator");

        levelStarted = false;

        tempCoordinates = startIsland.transform.position;
        startCoordinates =  new Vector3 (tempCoordinates.x, 10, tempCoordinates.z);
        //thePlayer.GetComponent<PlayerMovement>().enabled = false;
        thePlayer.GetComponent<Rigidbody>().useGravity = false;
        currentCoordinates = thePlayer.transform.position;
        travelVector = startCoordinates - currentCoordinates;
        Omni = GameObject.Find("The Omnipresent Object");

        UDP_Handling.Xtarget = startIsland.transform.position.x * 150 / Camera.main.orthographicSize;
        UDP_Handling.Ytarget = startIsland.transform.position.z * 150 / Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        positionIndicator.position = (thePlayer.transform.position);
        if (levelStarted == false)
        {
            deltaX = Mathf.Abs(startCoordinates.x - thePlayer.transform.position.x);
            deltaZ = Mathf.Abs(startCoordinates.z - thePlayer.transform.position.z);
            if (deltaX > 0.35f || deltaZ > 0.35f)
            {
                levelStarted = false;
            }
            else
            {
                levelStarted = true;
            }
        }
        else
        {
            //thePlayer.GetComponent<PlayerMovement>().enabled = true;
            thePlayer.GetComponent<Rigidbody>().useGravity = true;
            positionIndicator.gameObject.SetActive(false);
        }
    }
}
