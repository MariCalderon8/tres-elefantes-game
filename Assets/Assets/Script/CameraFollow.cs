using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;           // El jugador u objeto a seguir
    public Vector2 offset = new Vector2(0f, 1f); // Desplazamiento opcional
    public float smoothSpeed = 5f;     // Suavidad del seguimiento

    void LateUpdate()
    {
        if (target == null) return;

        // Posición deseada solo en X e Y (mantiene Z fijo)
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
