using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMovement : MonoBehaviour
{

    public float timer;
    public float y;
    public float initialY;

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
        timer = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        y = Mathf.Sin(timer);
        transform.position = new Vector3(0, initialY + 1f * y, 0);
    }
}
