using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;


public class HighscoreText : MonoBehaviour
{
    public string directory;
    public float highscore;
    public int level;

    public string[] allScores_string;

    // Start is called before the first frame update
    void Start()
    {
        highscore = 0;

        Scene scene = SceneManager.GetActiveScene();
        string levelNumString = scene.name.Split(' ')[1];
        level = int.Parse(levelNumString);


        directory = Application.dataPath + "/Resources/Local Files/Scores/Level " + level.ToString() + " Scores.csv";

        if (!File.Exists(directory))
        {
            highscore = 0;
        }
        else
        {
            allScores_string = File.ReadAllLines(directory);
            foreach (string score_string in allScores_string)
            {
                float score = float.Parse(score_string);
                if (score > highscore)
                {
                    highscore = score;
                }
            }
        }


        gameObject.GetComponent<Text>().text = "Highscore: " + highscore.ToString("F1");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
