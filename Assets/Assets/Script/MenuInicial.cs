using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{

    public void iniciar()
    {
        SceneManager.LoadScene("Scene_2");
    }

    public void Salir()
    {
        Application.Quit();
    }
}