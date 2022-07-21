using UnityEngine;
using System.Collections;

public class FlyCameraController : MonoBehaviour
{

    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/


    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 25.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float camSens = 0.65f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    public bool invertY = true;
    public float scrollWheelSens = 100f;

    Vector3 minLoc = new Vector3(-30, 3, -140);
    Vector3 maxLoc = new Vector3(190, 100, 10);

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            var mouseMoveY = invertY ? -1 * Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
            var mouseMoveX = Input.GetAxis("Mouse X");

            var mouseMove = new Vector3(mouseMoveY, mouseMoveX, 0) * camSens;
            transform.eulerAngles = transform.eulerAngles + mouseMove;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //Mouse  camera angle done.  

        //Keyboard commands
        float f = 0.0f;

        Vector3 p = GetBaseInput();
        var scroll = Input.GetAxis("Mouse ScrollWheel") * scrollWheelSens;
        if (!((transform.position.y == minLoc.y && scroll > 0) || (transform.position.y == maxLoc.y && scroll < 0)))
        {
            p += new Vector3(0, 0, 1) * scroll;
        }

        if (p.sqrMagnitude > 0)
        { // only move while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            if (scroll == 0)
            { //If player wants to move on X and Z axis only
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(p);
            }

            Vector3 alignVector = transform.position;
            if (transform.position.x < minLoc.x)
            {
                alignVector.x = minLoc.x;
            } else if (transform.position.x > maxLoc.x)
            {
                alignVector.x = maxLoc.x;
            }
            if (transform.position.y < minLoc.y)
            {
                alignVector.y = minLoc.y;
            }
            else if (transform.position.y > maxLoc.y)
            {
                alignVector.y = maxLoc.y;
            }
            if (transform.position.z < minLoc.z)
            {
                alignVector.z = minLoc.z;
            }
            else if (transform.position.z > maxLoc.z)
            {
                alignVector.z = maxLoc.z;
            }
            transform.position = alignVector;
        }
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }

        return p_Velocity;
    }
}