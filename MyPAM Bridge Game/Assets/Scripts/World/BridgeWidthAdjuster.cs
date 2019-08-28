using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeWidthAdjuster : MonoBehaviour
{
    public int failNum;
   
    // Start is called before the first frame update
    void Start()
    {
        failNum = GameObject.Find("The Omnipresent Object").GetComponent<FileHandler>().attemptNumber - 1;
        if (gameObject.name.Contains("Start"))
        {
            gameObject.GetComponentInChildren<FirstButtonHandling>().bridgeWidth += (failNum / 2);
        }
        else if (gameObject.name.Contains("Island"))
        {
            gameObject.GetComponentInChildren<GeneralButtonHandling>().bridgeWidth += (failNum / 2);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
