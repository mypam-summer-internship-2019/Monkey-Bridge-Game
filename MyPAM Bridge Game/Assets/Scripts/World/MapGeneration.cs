using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;



public class MapGeneration : MonoBehaviour
{

    public GameObject Omni;

    public string directory;
    public int levelNum;
    public string levelFilePath;
    public string prefabFilePath;

    public string chosenPrefab; 
    public GameObject island;
    public GameObject islandClone;
    public GameObject prevClone;

    public string[] allLinesFromFile;
    public string[] splitLine;
    public int lineNum;

    public string[] metaData;

    public Vector3 pos;
    public Quaternion rot;

    

    // Start is called before the first frame update
    void Start()
    {

        Omni = GameObject.Find("The Omnipresent Object");

        directory = Application.dataPath;
        // levelNum = Omni.GetComponent<FileHandler>().level;
        Scene scene = SceneManager.GetActiveScene();
        string levelNumString = scene.name.Split(' ')[1];
        levelNum = int.Parse(levelNumString);
        
        levelFilePath = directory + "/Resources/Local Files/Settings/Levels/Level " + levelNum + " Islands.csv";

        if (!File.Exists(levelFilePath))
        {
            SceneManager.LoadScene(1);
        }

        allLinesFromFile = File.ReadAllLines(levelFilePath);

        metaData = allLinesFromFile[0].Split(',');
        //metaData[0] is the type of island used, e.g. rock or dirt
        //metaData[1] is the scale of the level - it defines the maximum +- x coordinate

        lineNum = 1;
        while (lineNum < allLinesFromFile.Length)
        {
            prevClone = islandClone;

            splitLine = allLinesFromFile[lineNum].Split(',');

            float scale = float.Parse(metaData[1]);
            Camera.main.orthographicSize = scale;

            if (metaData[0] == "Dirt")
            {
                chosenPrefab = "D" + splitLine[0];
            }

            if (lineNum == 1)
            {
                island = Resources.Load("Prefabs/Start Islands/Start " + chosenPrefab) as GameObject;
            }
            else if (lineNum == allLinesFromFile.Length-1)
            {
                island = Resources.Load("Prefabs/End Islands/End " + chosenPrefab) as GameObject;
            }
            else
            {
                island = Resources.Load("Prefabs/General Islands/Island " + chosenPrefab) as GameObject;
            }

            float x = (float.Parse(splitLine[1])) / 10 * scale;
            float z = (float.Parse(splitLine[2])) / 100 * 7 * scale;
            pos = new Vector3(x, 0, z);
            rot = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 359), 0));
            islandClone = Instantiate(island, pos, rot);

            if (lineNum == 1)
            {
                Camera.main.GetComponent<LevelStart>().startIsland = islandClone;
            }
            
            if (lineNum == 2)
            {
                
                prevClone.transform.Find("Start Button").GetComponent<FirstButtonHandling>().nextIsland = islandClone;
                prevClone.transform.Find("Start Button").GetComponent<FirstButtonHandling>().bridgeWidth = float.Parse(splitLine[3]);
            }
            if (lineNum > 2)
            {
                prevClone.transform.Find("Standard Button").GetComponent<GeneralButtonHandling>().nextIsland = islandClone;
                prevClone.transform.Find("Standard Button").GetComponent<GeneralButtonHandling>().bridgeWidth = float.Parse(splitLine[3]);
            }

            lineNum++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
