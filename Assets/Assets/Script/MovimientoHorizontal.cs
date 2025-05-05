using UnityEngine;

public class MovimientoHorizontal : MonoBehaviour
{
    public float velocidad; // Velocidad de movimiento
    public float distancia; // Distancia que se moverá la plataforma
    private Vector3 inicio;
    private Vector3 fin;

    void Start()
    {
        inicio = transform.position;
        fin = new Vector3(inicio.x + distancia, inicio.y, inicio.z);
    }

    void Update()
    {
        // Mueve la plataforma entre los puntos de inicio y fin
        transform.position = Vector3.Lerp(inicio, fin, Mathf.PingPong(Time.time * velocidad, 1.0f));
    }
}