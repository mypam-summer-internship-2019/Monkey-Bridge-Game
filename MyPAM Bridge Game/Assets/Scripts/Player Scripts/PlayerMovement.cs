using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector3 rawPos;
    private Vector3 mappedPos;
    private Vector3 currentPos;
    private Vector3 coordinates;
    private GameObject Omni;
    private float UDP_x;
    private float UDP_y;
    public Vector3 lastPosition;
    public Vector3 positionDifference;
    public Vector3 rotation;
    public float cameraScale;
    public float scaling;

    // Start is called before the first frame update
    void Start()
    { 
        Omni = GameObject.Find("The Omnipresent Object");
        lastPosition = transform.position;
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        getCoordinatesFromMYPAM();
        transform.position =  new Vector3 (mappedPos.x,currentPos.y,mappedPos.z);

        positionDifference = transform.position - lastPosition;
        transform.Rotate((new Vector3(positionDifference.z, 0, -positionDifference.x) * 60), Space.World);

        lastPosition = transform.position;
    }

    public void getCoordinatesFromMouse()
    {
        rawPos = Input.mousePosition;
        rawPos.z = currentPos.y;
        mappedPos = Camera.main.ScreenToWorldPoint(rawPos);
    }

    public void getCoordinatesFromMYPAM()
    {
        // getX is from -150 to +150
        // getY is from -105 to +105
        // the game world space is dependant on the level. Some levels will be zoomed in or out to change the difficulty
        //the mapping should therefore relate to the variable scale of the world
        //y = 0.02x2 - 1.2x + 23.5
        cameraScale = Camera.main.orthographicSize;
        scaling = 150 / cameraScale;
        UDP_x = (float) UDP_Handling.X2pos / scaling;
        UDP_y = (float) UDP_Handling.Y2pos / scaling;
        mappedPos = new Vector3(UDP_x, 0, UDP_y);
    }
}
    