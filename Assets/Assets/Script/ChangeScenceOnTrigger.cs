using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeSceneOnTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string siguienteEscena;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(siguienteEscena);
        }
    }
}