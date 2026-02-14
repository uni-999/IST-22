using UnityEngine;

public class MovmentScript : MonoBehaviour
{
    public GameObject camera;
    public GameObject snake;
    public GameObject target;

    private float velocity;
    private float defaultCameraYRotation; // исходный поворот камеры по Y
    private float currentCameraYRotation; // текущий поворот камеры по Y

    void Start()
    {
        velocity = 0;
        defaultCameraYRotation = 0;
        currentCameraYRotation = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && velocity < 15f)
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
            Debug.Log(velocity);
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

        snake.gameObject.GetComponent<Animator>().speed = velocity/5;
    }
}
