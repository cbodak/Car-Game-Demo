using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SimpleCarController : MonoBehaviour {

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontLeftW, frontRightW, backLeftW, backRightW;
    public Transform frontLeftT, frontRightT, backLeftT, backRightT;
    public float maxSteeringAngle = 30;
    public float motorForce = 50;
    public Text forceText;
    public Text boostText;
    public Text scoreText;

    public GameObject prefabCoin;
    public GameObject prefabBoost;

    public GameObject carRoof;
    public GameObject carHood;
    public GameObject carBody;
    public GameObject carDoor1;
    public GameObject carDoor2;
    public GameObject carTrunk;
    public GameObject carBumper1;
    public GameObject carBumper2;

    private bool isBoost;
    private float boostTime;
    private int score;

    public void Start()
    {   
        isBoost = false;
        m_horizontalInput = 0;
        m_verticalInput = 0;
        boostTime = 0;
        score = 0;
        displayForce();
        displayScore();
        spawnItems();
        StartCoroutine(test());
        setColour();
    }

    //test function to wait for 5 seconds
    public IEnumerator test()
    {
        yield return new WaitForSeconds(5);
    }

    //sets the colour of the car based on the button selection in the start menu scene
    public void setColour()
    {

            GameObject colourButton = GameObject.Find("colourHolder");
            carRoof.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carRoof.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carHood.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carHood.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carBody.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carBody.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carDoor1.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carDoor1.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carDoor2.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carDoor2.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carTrunk.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carTrunk.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carBumper1.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carBumper1.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;
            carBumper2.GetComponent<Renderer>().materials[0].color = colourButton.GetComponent<Image>().color;
            carBumper2.GetComponent<Renderer>().materials[1].color = colourButton.GetComponent<Image>().color;

    }

    //Spawns coins at pseudo-random locations throughout the map, and spawns one "boost" cube 
    public void spawnItems()
    {
        System.Random _random = new System.Random();

        int[] posX = new int[10];
        int[] posZ = new int[10];

        for (int i = 0; i < 10; i++)
        {
            posX[i] = _random.Next(-15 , 15);
            posZ[i] = _random.Next(i, 15);
            Vector3 pos = new Vector3(posX[i], 0.65f, posZ[i]);
            Instantiate(prefabCoin, pos, Quaternion.identity);
        }

        Vector3 boostPos = new Vector3(3f, 0.65f, -10f);
        Instantiate(prefabBoost, boostPos, Quaternion.identity);
    }

    //Gets the vertical and horizontal input from the user (based on wasd key presses)
    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    //Sets the steering angles of the wheels based on a maximum of 30 degrees
    public void Steer()
    {
        m_steeringAngle = maxSteeringAngle * m_horizontalInput;
        frontLeftW.steerAngle = m_steeringAngle;
        frontRightW.steerAngle = m_steeringAngle;
    }

    //Sets the acceleration (motor torque) based on the vertical input (w and s keys) and the motor force
    public void Accelerate()
    {
        frontLeftW.motorTorque = m_verticalInput * motorForce;
        frontRightW.motorTorque = m_verticalInput * motorForce;
    }

    //Calls UpdateWheelPose for each wheel
    public void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftW, frontLeftT);
        UpdateWheelPose(frontRightW, frontRightT);
        UpdateWheelPose(backLeftW, backLeftT);
        UpdateWheelPose(backRightW, backRightT);
    }

    //Sets the position and rotation for each wheel as they move
    public void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        //outputs the world pose to _pos and _quat
        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    public void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();

    }

    public void Update()
    {
        displayBoost();

        boostMode();
        boostHandler();
    }

    //Functionn to change the motor force based on whether or not the car is in "boost" mode
    public void boostMode()
    {   

        //If space is pressed and boost is not currently active, turn boost mode on as long as some boost time has been acquired
        if (Input.GetKeyDown("space"))
        {
            if (isBoost == false)
            {
                if (boostTime > 0)
                {
                    isBoost = true;
                    motorForce = 100; //boost mode is double the usual motor force
                    displayForce();
                }
            }

            else
            {
                isBoost = false;
                motorForce = 50;
                displayForce();
            }
        }
    }

    //Handles the reduction of boost time as it is used
    public void boostHandler()
    {
        if (isBoost == true)
        {
            boostTime = boostTime-Time.deltaTime;//Reduce the boost time by the difference in time between frames if it is active
        }

        if (boostTime <= 0)
        {
            isBoost = false;
            boostTime = 0;
        }
    }

    //Adds 20 seconds to the current boost time
    public void boostAdd()
    {
        boostTime += 20;
    }

    //Displays the current boost time in boostText
    public void displayBoost()
    {
        double boostRounded = Math.Round(boostTime, 3);
        string _boostText = "Boost: " + boostRounded;
        boostText.text = _boostText;
    }

    //Handles collisions with the coins and cubes
    void OnTriggerEnter(Collider other)
    {   
        //Increases the score if the object was a coin
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            score++;
            displayScore();
        }

        //Adds boost time if the object was a power-up cube
        else if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            boostAdd();
        }
    }

    //Displays the current score in scoreText
    void displayScore()
    {
        scoreText.text = "Score: " + score; 
    }

    //Test function to display the current motorForce
    void displayForce()
    {
        forceText.text = "Force: " + motorForce;
    }
}
