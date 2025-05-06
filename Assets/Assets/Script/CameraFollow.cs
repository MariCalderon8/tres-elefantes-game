using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;                     
    public Vector2 offset = new Vector2(0f, 1f);
    public float smoothSpeed = 5f;               // Suavidad del seguimiento

    public Vector2 minLimits; // Límite inferior izquierdo del nivel
    public Vector2 maxLimits; // Límite superior derecho del nivel

    private float camHalfHeight;
    private float camHalfWidth;

    private void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // Limitar dentro de los bordes definidos
        float clampedX = Mathf.Clamp(desiredPosition.x, minLimits.x + camHalfWidth, maxLimits.x - camHalfWidth);
        float clampedY = Mathf.Clamp(desiredPosition.y, minLimits.y + camHalfHeight, maxLimits.y - camHalfHeight);
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
