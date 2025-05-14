using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneLevel2 : MonoBehaviour
{
    private CollectableController controller;
    void Start()
    {
        controller = GameObject.FindObjectOfType<CollectableController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (controller.gemCollected)
            {
                SceneManager.LoadScene("Scene_4");
            }
            if (controller.coffinCollected)
            {
                SceneManager.LoadScene("Scene_5");
            }
        }
    }
}
