using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{

    //generateBridge() variables
    public Vector3 islandDisplacement;
    public Vector3 bridgePosition;
    public float bridgeLength;
    public Quaternion bridgeOrientation;
    public float displacementAngle;
    public float heightAngle;
    public GameObject bridge;
    public GameObject newBridge;
    public float bridgeWidth;
    public Vector3 currentCoordinates;
    public Vector3 targetCoordinates;

    //timing variables
    public bool timerRunning;
    public float timer;
    public Text timeText;
    public bool buttonPressed;
    public string timeString;
    public int minutes;
    public int seconds;
    public int milliseconds;

    public GameObject finalButtonUIText;
    public Scene currentLevel;
    public float delay;
    public bool levelCompleted;

    public GameObject bridgeRenderObject;
    public GameObject newBridgeRenderObject;

    public GameObject Omni;



    // Start is called before the first frame update
    void Start()
    {
        timerRunning = false;
        timer = 0;
        timeText.text = "";

        delay = 3.0f;
        levelCompleted = false;

        Omni = GameObject.Find("The Omnipresent Object");
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning == true)
        {
            timer += Time.deltaTime;
        }

        finalButtonUIText.SetActive(levelCompleted);
        if (levelCompleted == true)
        {
            if (delay < 0)
            {
                loadNextLevel();
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }

    }


    public void generateBridge(GameObject currentIsland, GameObject nextIsland, float _width)
    {
        //these need to be set anyway, so that the popup can use them as well
        currentCoordinates = currentIsland.transform.position + new Vector3(0, -0.3f, 0);

        if (nextIsland.name.Contains("End"))
        {
            targetCoordinates = nextIsland.transform.Find("End Button").position + new Vector3(0, -0.3f, 0);
        }
        else
        {
            targetCoordinates = nextIsland.transform.Find("Standard Button").position + new Vector3(0, -0.3f, 0);
        }
        bridgeWidth = _width;

        islandDisplacement = targetCoordinates - currentCoordinates;
        bridgeLength = islandDisplacement.magnitude;
        displacementAngle = Mathf.Atan2(islandDisplacement.x, islandDisplacement.z);
        heightAngle = Mathf.Asin(-islandDisplacement.y/ bridgeLength);

        bridgePosition = currentCoordinates + (islandDisplacement / 2);
        bridgeOrientation = Quaternion.Euler(heightAngle * 180 / Mathf.PI, displacementAngle * 180 / Mathf.PI, 0);

        newBridge = Instantiate(bridge, bridgePosition, bridgeOrientation);
        newBridge.transform.localScale = new Vector3(bridgeWidth, 0.2f, bridgeLength);

        newBridge.GetComponent<Renderer>().material.SetVector("_UvTiling", new Vector4(1, bridgeLength / 2.2f, 0, 0));

        float cameraScale = Camera.main.orthographicSize;
        float scaling = 150 / cameraScale;
        UDP_Handling.Xtarget = nextIsland.transform.position.x * scaling;
        UDP_Handling.Ytarget = nextIsland.transform.position.z * scaling;

    }

    public void startTimer()
    {
        timerRunning = true;
    }

    public void stopTimer()
    {
        timerRunning = false;
    }

    public void displayTime()
    {
        minutes = (int)timer / 60;
        seconds = (int)timer % 60;
        milliseconds = (int)(timer * 1000) % 1000;
        timeString = string.Format("{0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        timeText.text = "Time:  " + timeString;
        Omni.GetComponent<FileHandler>().saveToAttemptLog("S");

    }

    public void completeLevel()
    {
        levelCompleted = true;
    }

    public void loadNextLevel()
    {

        Omni.GetComponent<FileHandler>().resetAttemptTally();

        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (Application.CanStreamedLevelBeLoaded(sceneIndex))
        {
            GameObject.Find("The Omnipresent Object").GetComponent<FileHandler>().setLevel(sceneIndex);

            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            GameObject.Find("The Omnipresent Object").GetComponent<FileHandler>().setLevel(1);

            SceneManager.LoadScene(1);
        }

    }
    
}
