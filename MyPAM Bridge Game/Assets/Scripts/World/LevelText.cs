using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        string levelNumString = scene.name.Split(' ')[1];
        level = int.Parse(levelNumString);

        gameObject.GetComponent<Text>().text = "Level " + level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
