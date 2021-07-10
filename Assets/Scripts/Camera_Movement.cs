using System.Collections;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    Transform targetPosition;
    float cameraSpeed = 4f;
    Vector2 offset;
    void Start()
    {
        targetPosition =GameObject_Manager.instance.player.GetComponent<Transform>();
        offset = new Vector2(0, 0);
        GameObject_Manager.instance.camera = this.gameObject;
    }

    void Update()
    {
        if (targetPosition == null)
        {
            targetPosition = GameObject_Manager.instance.player.GetComponent<Transform>();
        }

        Vector2 smoothPosition = Vector2.Lerp((Vector2)transform.position, (Vector2)targetPosition.position, cameraSpeed * Time.deltaTime);
        transform.position = new Vector2(smoothPosition.x+offset.x, smoothPosition.y+offset.y);
    }

    public IEnumerator CameraShake(float magnitude, float duration)
    {
        float shakeTime = 0f;
        while(shakeTime < duration)
        {
            offset.x = Random.Range(-1f * magnitude, 1f * magnitude);
            offset.y = Random.Range(-1f * magnitude, 1f * magnitude);

            shakeTime += Time.deltaTime;
            yield return null;
        }
        offset.x = 0;
        offset.y = 0;
    }    
}
