using System;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class MovmentScript : MonoBehaviour
{
    public GameObject mainSkin;
    public Texture textureOne;
    public Texture textureTwo;
    public Texture textureThree;
    public Texture textureFour;
    public Texture textureFive;
    public Texture textureSix;

    public GameObject camera;
    public GameObject snake;
    public GameObject target;
    public GameObject trapPosotion;
    public float velocity;

    public bool isboosted;
    public bool haveTrap;

    public GameObject text;

    public GameObject trapPrefab;

    private float defaultCameraYRotation; // исходный поворот камеры по Y
    private float currentCameraYRotation; // текущий поворот камеры по Y
    public Vector3 checkpointPosition;
    public Quaternion checkpointRotation;
    public bool isRaceOver;

    void Start()
    {
        SetMenuColor();
        velocity = 0;
        defaultCameraYRotation = 0;
        currentCameraYRotation = 0;
        isboosted = false;
        haveTrap = false;
        checkpointPosition = new Vector3(-102.2f, 26.3f, 44.9f);
        isRaceOver = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && velocity < 15f && !isRaceOver)
        {
            velocity += Time.deltaTime * 2.15f;
        }
        else if (!Input.GetKey(KeyCode.W) && velocity > 0)
        {
            velocity -= Time.deltaTime * 1.25f;

            if (velocity < 0)
            {
                velocity = 0;
            }
        }
        if (Input.GetKey(KeyCode.S) && velocity > 0)
        {
            velocity -= Time.deltaTime * 1.25f;
        }

        if (velocity > 0)
        {
            //Debug.Log(velocity);
            snake.gameObject.transform.position += new Vector3(target.gameObject.transform.position.x - snake.gameObject.transform.position.x, 0, target.gameObject.transform.position.z - snake.gameObject.transform.position.z) * 0.01f * velocity;
        }

        float cameraTargetRotation = 0; 

        if (Input.GetKey(KeyCode.A))
        {
            snake.transform.Rotate(0, -50f * Time.deltaTime, 0);
            cameraTargetRotation = -7.5f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            snake.transform.Rotate(0, 50f * Time.deltaTime, 0);
            cameraTargetRotation = 7.5f;
        }

        currentCameraYRotation = Mathf.Lerp(currentCameraYRotation, cameraTargetRotation, Time.deltaTime * 5f);

        Vector3 cameraRotation = camera.transform.localEulerAngles;
        cameraRotation.y = defaultCameraYRotation + currentCameraYRotation;
        camera.transform.localEulerAngles = cameraRotation;

        snake.gameObject.GetComponent<Animator>().speed = velocity/2.5f;

        if (isboosted == true)
        {
            camera.GetComponent<Camera>().fieldOfView += Time.deltaTime * 15f;
            if(camera.GetComponent<Camera>().fieldOfView > 100)
            {
                isboosted = false;
            }
        }

        if (camera.GetComponent<Camera>().fieldOfView > 75 && isboosted == false)
        {
            camera.GetComponent<Camera>().fieldOfView -= 5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q) && haveTrap == true)
        {
            
            Instantiate(trapPrefab, new Vector3(trapPosotion.transform.position.x, 25.805f, trapPosotion.transform.position.z), Quaternion.identity);
            haveTrap = false;
        }

        text.GetComponent<TextMeshProUGUI>().text = "Скорость: " + Math.Round(velocity, 1).ToString() + " км/сек";
    }
    public void SetMenuColor()
    {
        int number = PlayerPrefs.GetInt("NumberSkin");
        switch (number)
        {
            case 1:
                mainSkin.GetComponent<Renderer>().material.mainTexture = textureOne;
                break;
            case 2:
                mainSkin.GetComponent<Renderer>().material.mainTexture = textureTwo;
                break;
            case 3:
                mainSkin.GetComponent<Renderer>().material.mainTexture = textureThree;
                break;
            case 4:
                mainSkin.GetComponent<Renderer>().material.mainTexture = textureFour;
                break;
            case 5:
                mainSkin.GetComponent<Renderer>().material.mainTexture = textureFive;
                break;
            case 6:
                mainSkin.GetComponent<Renderer>().material.mainTexture = textureSix;
                break;
        }
    }
}