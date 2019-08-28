using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAppearance : MonoBehaviour
{
    public Text self;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        self.CrossFadeAlpha(255f, 0.1f, true);
        
      
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.15f)
        {
            self.CrossFadeAlpha(0.0f, 0.2f, true);
        }
        self.transform.localPosition += new Vector3 (0,1,0);

        //also self destructs after 1.5 seconds
        if (timer > 1.5f)
        {
            Destroy(gameObject);
        }
    }
}
