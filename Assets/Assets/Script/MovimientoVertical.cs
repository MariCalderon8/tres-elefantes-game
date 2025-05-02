using UnityEngine;

public class MovimientoVertical : MonoBehaviour
{
    public float velocidad = 2f; // Velocidad de movimiento
    public float distancia = 5f; // Distancia que se moverá la plataforma
    private Vector3 inicio;
    private Vector3 fin;

    void Start()
    {
        inicio = transform.position;
        fin = new Vector3(inicio.x, inicio.y + distancia, inicio.z);
    }

    void Update()
    {
        // Mueve la plataforma entre los puntos de inicio y fin
        transform.position = Vector3.Lerp(inicio, fin, Mathf.PingPong(Time.time * velocidad, 1.0f));
    }
}
