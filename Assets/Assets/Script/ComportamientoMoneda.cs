using UnityEngine;

public class ComportamientoMoneda : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el jugador colisionó con la moneda
        if (collision.gameObject.CompareTag("Player"))
        {
            // Destruye la moneda
            Destroy(gameObject);

            // Aquí puedes añadir lógica adicional, como aumentar un contador de monedas
            Debug.Log("¡Moneda recolectada!");
        }
    }
}