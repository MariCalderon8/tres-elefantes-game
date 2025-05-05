using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{

    public void iniciar()
    {
        Debug.Log("Iniciar juego");
        SceneManager.LoadScene("Scene_2");
    }

    public void Salir()
    {
        Application.Quit();
    }
}