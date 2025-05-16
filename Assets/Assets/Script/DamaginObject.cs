using UnityEngine;

public class DamaginObject : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(damage);
            other.gameObject.GetComponent<PlayerController>().TakeDamage(damage, other.GetContact(0).normal);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamageOnTrigger(damage);
        }
    }
}
