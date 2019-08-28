using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;



public class FileHandler : MonoBehaviour
{
    public string directory;


    public GameObject thePlayer;
    public Camera theCamera;

    public Vector3 playerCoordinates;
    public float timer;
    public string time;
    public DateTime startTime;

    public long session;
    public int level;
    public int attemptNumber;

    public Vector3 targetCoordinates;
    public Vector3 originCoordinates;
    public float levelScore;
    public string path;
    public bool levelStarted;

    public int line;
    public int levelNum;




    // Start is called before the first frame update
    void Start()
    {
        directory = Application.dataPath + "/Resources/Local Files/";
        timer = 0;
        session = DateTime.Now.Ticks;
        attemptNumber = 0;
        startTime = DateTime.Now;
        saveToSessionLog();
        line = 0;
    }


    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        string levelNumString = scene.name.Split(' ')[1];
        try
        {
            levelNum = int.Parse(levelNumString);
        }
        catch
        {
            levelNum = 0;
        }
    }

    private void FixedUpdate()
    {
        thePlayer = GameObject.FindWithTag("Player");
        theCamera = Camera.main;

        playerCoordinates = thePlayer.transform.position;

        if (levelStarted == true)
        {
            timer += Time.deltaTime;

            float pX = playerCoordinates.x;
            float pY = playerCoordinates.y;
            float pZ = playerCoordinates.z;
            float tX = targetCoordinates.x;
            float tY = targetCoordinates.y;
            float tZ = targetCoordinates.z;
            float oX = originCoordinates.x;
            float oY = originCoordinates.y;
            float oZ = originCoordinates.z;

            saveData(findCoordinatesFile(), session + "," + attemptNumber + "," + timer + "," + pX + "," + pY + "," + pZ + "," + tX + "," + tY + "," + tZ + "," + oX + "," + oY + "," + oZ + "\r\n");

        }
    }

    private void OnApplicationQuit()
    {
        string filepath = directory + "/Logs/Session Log.csv";
     //   Debug.Log(startTime);
      //  Debug.Log(DateTime.Now);
      //  Debug.Log(DateTime.Now.Subtract(startTime));
      //  Debug.Log(DateTime.Now.Subtract(startTime).ToString());
        File.AppendAllText(filepath, "," + DateTime.Now + "," + DateTime.Now.Subtract(startTime).ToString()+"\r\n");
      //  Debug.Log("----------------------");
    }


    public string findCoordinatesFile()
    {
        string fileString = "Level " + levelNum.ToString();
        string filepath = directory + "/Coordinate Data/" + fileString + ".csv";
        if (!File.Exists(filepath))
        {
            //   File.WriteAllText(filepath,"Session ID,Attempt,Min,Sec,Mil,X,Y,Z\r\n");
            File.WriteAllText(filepath, "Session ID,Attempt,Time,p_X,p_Y,p_Z,o_X,o_Y,o_Z,t_X,t_Y,t_Z\r\n");
        }   
        return filepath;
    }

    public void saveToAttemptLog(string success)
    {
        levelStarted = false;
        thePlayer.GetComponent<PlayerMovement>().enabled = false;
        string fileString = "Session " + session.ToString();
        string filepath = directory + "/Logs/" + fileString + ".csv";
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "Level,Attempt,Begin,End,Duration,S/F\r\n");
        }
        File.AppendAllText(filepath, levelNum + "," + attemptNumber + "," + (DateTime.Now.Subtract(TimeSpan.FromSeconds(timer))).ToString() + "," + DateTime.Now.ToString() + "," + timer + "," + success + "\r\n");

        if (success == "S")
        {
            fileString = "Level " + levelNum.ToString();
            filepath = directory + "/Best Times/Best " + fileString + ".csv";
            if (!File.Exists(filepath))
            {
                File.WriteAllText(filepath, session + "," + attemptNumber + "," + timer);
            }
            else
            {
                float currentBestTime = float.Parse((File.ReadAllLines(filepath)[0]).Split(',')[2]);
                if (timer < currentBestTime)
                {
                    File.WriteAllText(filepath, session + "," + attemptNumber + "," + timer);
                }
            }    
        }
    }

    public void saveToSessionLog()
    {
        string fileString = "Session Log";
        string filepath = directory + "/Logs/" + fileString + ".csv";
        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "Session ID,Start Date/Time,End Date/Time,Duration\r\n");
        }
        File.AppendAllText(filepath, session + "," + startTime);
    }


    public void saveData(string path, string dataToSave)
    {
        File.AppendAllText(path, dataToSave);
    }

    public void saveLine(string lineToSave)
    {
        //Debug.Log(" ");
        //Debug.Log(lineToSave);
        //Debug.Log(" ");

    }

    public void incrementAttemptTally()
    {
        attemptNumber++;
        saveLine(attemptNumber.ToString());
    }

    public void resetAttemptTally()
    {
        attemptNumber = 0;
        saveLine(attemptNumber.ToString());
    }

    public void setCoordinates(Vector3 target, Vector3 origin)
    {
        targetCoordinates = target;
        originCoordinates = origin;
        saveLine(target.ToString());
        saveLine(origin.ToString());
    }

    public void setLevelScore(float score)
    {
        string highScoreFilePath = directory + "Scores/Level " + levelNum + " Scores.csv";
        if (!File.Exists(highScoreFilePath))
        {
            File.WriteAllText(highScoreFilePath, "");
        }
        levelScore = score;
        File.AppendAllText(highScoreFilePath, levelScore.ToString() + "\r\n");
    }

    public void setLevel(int levelIndex)
    {
        level = levelIndex;
        saveLine(level.ToString());
    }

    public void resetTimer()
    {
        timer = 0;
    }

    public void setLevelStarted(bool started)
    {
        levelStarted = started;
    }
}
