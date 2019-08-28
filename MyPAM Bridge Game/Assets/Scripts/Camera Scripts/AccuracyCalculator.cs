using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccuracyCalculator : MonoBehaviour
{

    private Vector3 startCoordinates;
    private Vector2 startCoordinates2D;
    private Vector3 targetCoordinates;
    private Vector2 targetCoordinates2D;
    private GameObject thePlayer;
    private Vector3 playerPosition;
    private Vector2 playerPosition2D;
    private float distanceFromStartToPlayer;
    private float angleToPlayer;
    private float angleInRad;
    private float distanceFromTargetLine;
    private float distanceAlongBridge;
    public float bridgeScore;
    public float totalScore;
    private float totalDistance;
    private int totalSamples;
    private int sample;
    private bool takeSamples;

    public GameObject bridgeScoreText;
    private GameObject canvas;
    private GameObject textInstance;

    private RectTransform canvasRect;
    private RectTransform score;

    public Text finalScoreText;

    public GameObject highscoreText;
    public GameObject particles;
    public bool newHighscore;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = GameObject.FindWithTag("Player");
        //sample starts at 1 since we want to measure distance after 0.1m not after 0.0m. Everything else starts at 0 for obvious reasons when the level first loads
        sample = 1;
        totalScore = 0;
        totalSamples = 0;
        totalDistance = 0;

        canvas = GameObject.Find("FixedCanvas");
        canvasRect = canvas.GetComponent<RectTransform>();

        finalScoreText.text = "";

        takeSamples = false;

        particles.SetActive(false);
        newHighscore = false;
    }

    // Update is called once per frame
    void Update()
    {
        //get the coordinates of the start, end, and player
        startCoordinates = gameObject.GetComponent<ButtonHandler>().currentCoordinates;
        targetCoordinates = gameObject.GetComponent<ButtonHandler>().targetCoordinates;
        playerPosition = thePlayer.transform.position;

        //changes these coordinates into 2D coordinates, ignoring the y axis
        startCoordinates2D = new Vector2(startCoordinates.x, startCoordinates.z);
        targetCoordinates2D = new Vector2(targetCoordinates.x, targetCoordinates.z);
        playerPosition2D = new Vector2(playerPosition.x, playerPosition.z);

        //gets hypotenuse and angle from known values, then converts angle from degrees to radians
        distanceFromStartToPlayer = (playerPosition - startCoordinates).magnitude;
        angleToPlayer = Vector2.Angle(playerPosition2D - startCoordinates2D, targetCoordinates2D - startCoordinates2D);
        angleInRad = angleToPlayer*(Mathf.PI)/180 ;

        //get the opposite side of the triangle - the perpendicular distance from player to target line
        distanceFromTargetLine = distanceFromStartToPlayer * Mathf.Sin(angleInRad);

        //get the adjacent side of the triangle - the distance the player has travelled along the target line
        distanceAlongBridge = distanceFromStartToPlayer * Mathf.Cos(angleInRad);

        if (takeSamples == true)
        {
            //the sample number increments with every sample
            //a sample is taken every 0.1 units (after 0.1, then after 0.2, then after 0.3... etc)
            if (distanceAlongBridge > sample * 0.1f)
            {
                takeSample();
                sample++;
            }
        }

    }

    public void takeSample()
    {
        //if the bridge is wide enough (or the player has to use the width of the sphere to stay on the bridge), the player could get negative values and lose points
        //we want a more positive score for having a smaller distance to the target line, hence subtracting the distance from a fixed value
        bridgeScore += (1.5f - distanceFromTargetLine);

        //total values are used in the display of results in the level end screen
        totalDistance += distanceFromTargetLine;
        totalScore += (1.5f - distanceFromTargetLine);
        totalSamples++;
    }

    public void displayPopup(bool _popupIsScore)
    {
        //called when the next button is pressed
        //create a bubble with the score for that bridge

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, playerPosition);
        textInstance = Instantiate(bridgeScoreText);
        textInstance.transform.SetParent(canvas.transform);
        score = textInstance.GetComponent<RectTransform>();
        score.anchoredPosition = screenPoint - canvasRect.sizeDelta / 2f;

        if (_popupIsScore == true)
        {
            if (bridgeScore > 0)
            {
                textInstance.GetComponent<Text>().text = "+" + bridgeScore.ToString("F2");
            }
            else
            {
                textInstance.GetComponent<Text>().text = bridgeScore.ToString("F2");
            }
            textInstance.GetComponent<Text>().fontSize = 18;
        }
        else
        {
            textInstance.GetComponent<Text>().text = "GO!";
            textInstance.GetComponent<Text>().fontSize = 36;
        }

        //then reset the values for sample to 1 and bridgeScore to 0, since a new bridge is going to be sampled 
        sample = 1;
        bridgeScore = 0;
    }

    public void startScoring()
    {
        takeSamples = true;
    }

    public void displayFinalScore()
    {
        float prevHighScore = highscoreText.GetComponent<HighscoreText>().highscore;
        if (totalScore > prevHighScore)
        {
            finalScoreText.text = "NEW HIGH SCORE: " + totalScore.ToString("F2") + "!";
            particles.SetActive(true);
            newHighscore = true;
        }
        else
        {
            finalScoreText.text = "Score: " + totalScore.ToString("F2");
        }

        GameObject.Find("The Omnipresent Object").GetComponent<FileHandler>().setLevelScore(totalScore);

    }
}

