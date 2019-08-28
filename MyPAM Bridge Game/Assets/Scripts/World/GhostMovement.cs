using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GhostMovement : MonoBehaviour
{
    public GameObject Omni;

    public string ghostDataFilepath;
    public string[] allGhostLines;
    public int ghostLine;
    public Vector3 position;

    public string coordinatesFilepath;
    public string bestTimeFilePath;

    public string bestTimeData;
    public string[] bestTimeDataSplit;
    public string bestTimeID;
    public string bestTimeAttemptNum;
    public string bestTimeTime;

    public string[] allCoordinateLines;
    public string[] coordinateLineSplit;
    public string coordinateID;
    public string coordinateAttemptNum;

    public List<string> allGhostData;

    public string[] coordinates;

    public GameObject bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        Omni = GameObject.Find("The Omnipresent Object");
        Scene scene = SceneManager.GetActiveScene();
        string levelNumString = scene.name.Split(' ')[1];
        int levelNum = int.Parse(levelNumString);
        ghostDataFilepath = Application.dataPath + "/Resources/Local Files/Best Times/Ghost Level " + levelNum + ".csv";

        if (!File.Exists(ghostDataFilepath))
        {
            File.WriteAllText(ghostDataFilepath, "Ghost Data");
        }

        allGhostLines = File.ReadAllLines(ghostDataFilepath);
        ghostLine = 0;

        bestTimeFilePath = Application.dataPath + "/Resources/Local Files/Best Times/Best Level " + levelNum + ".csv";
        coordinatesFilepath = Application.dataPath + "/Resources/Local Files/Coordinate Data/Level " + levelNum + ".csv";

        updateGhostData();

        bestTimeText.GetComponent<Text>().text = "Best Time: " + bestTimeTime;
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (File.Exists(bestTimeFilePath))
        {
            if (ghostLine < allGhostLines.Length)
            {
                if (Omni.GetComponent<FileHandler>().levelStarted == true)
                {
                    coordinates = allGhostLines[ghostLine].Split(',');
                    float x = float.Parse(coordinates[3]);
                    float y = float.Parse(coordinates[4]);
                    float z = float.Parse(coordinates[5]);
                    position = new Vector3(x, y, z);
                    gameObject.transform.position = position;
                    ghostLine++;
                }
            }
        }
        else
        {
            gameObject.transform.position = new Vector3(0, -5, 0);
        }

    }
 
    public void updateGhostData()
    {
        if (File.Exists(bestTimeFilePath))
        {
            bestTimeData = File.ReadAllText(bestTimeFilePath);
            bestTimeDataSplit = bestTimeData.Split(',');
            bestTimeID = bestTimeDataSplit[0];
            bestTimeAttemptNum = bestTimeDataSplit[1];
            bestTimeTime = bestTimeDataSplit[2];
            bestTimeTime = float.Parse(bestTimeTime).ToString("F3");

            allCoordinateLines = File.ReadAllLines(coordinatesFilepath);
            foreach (string line in allCoordinateLines)
            {
                coordinateLineSplit = line.Split(',');
                coordinateID = coordinateLineSplit[0];
                coordinateAttemptNum = coordinateLineSplit[1];

                if (coordinateID == bestTimeID && coordinateAttemptNum == bestTimeAttemptNum)
                {
                    allGhostData.Add(line);
                }
            }
            allGhostLines = allGhostData.ToArray();

            File.WriteAllLines(ghostDataFilepath, allGhostLines);
        }
        else { bestTimeTime = "N/A"; }

    }
}
