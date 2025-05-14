using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public string objectType; 
    private CollectableController controller;

    private void Start()
    {
        controller = GameObject.FindObjectOfType<CollectableController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            controller.collectObject(gameObject, objectType);
        }
    }
}
