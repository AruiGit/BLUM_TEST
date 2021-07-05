using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    Transform targetPosition;
    float cameraSpeed = 4f;
    void Start()
    {
        targetPosition = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosition == null)
        {
            targetPosition = GameObject.Find("Player").GetComponent<Transform>();
        }

        Vector2 smoothPosition = Vector2.Lerp((Vector2)transform.position, (Vector2)targetPosition.position, cameraSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
