using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    Transform targetPosition;
    float cameraSpeed = 10f;
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
       // targetPosition.position = new Vector3(targetPosition.position.x, targetPosition.position.y, -20);

        Vector3 smoothPosition = Vector2.Lerp(transform.position, targetPosition.position, cameraSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
