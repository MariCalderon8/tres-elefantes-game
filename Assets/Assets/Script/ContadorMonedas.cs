using UnityEngine;
using UnityEngine.UI;

public class ContadorMonedas : MonoBehaviour
{
    public int monedasRecolectadas = 0; // Contador de monedas
    public Text textoContador; // Referencia al texto de la UI
    public ContadorMonedas contadorMonedas; // Referencia al contador

    void Start()
    {
        ActualizarTexto();
    }

    public void RecolectarMoneda()
    {
        monedasRecolectadas++; // Incrementa el contador
        ActualizarTexto(); // Actualiza el texto de la UI
    }

    private void ActualizarTexto()
    {
        if (textoContador != null)
        {
            textoContador.text = "Monedas: " + monedasRecolectadas;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Jugador"))
        {
            // Notifica al contador que se recolectó una moneda
            if (contadorMonedas != null)
            {
                contadorMonedas.RecolectarMoneda();
            }

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
    
}