using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float activationRadius = 5f; // Radio de activación
    public int maxHealth = 5; // Vida del enemigo
    public int damageToPlayer = 1; // Daño al player por toque
    public float moveSpeed = 2f; // Velocidad de movimiento

    private Transform player;
    private bool isActive = false;
    private int currentHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Busca al player
        currentHealth = maxHealth; // Inicializa la vida del enemigo
    }

    // void Update()
    // {
    //     if (Vector2.Distance(transform.position, player.position) <= activationRadius)
    //     {
    //         isActive = true;
    //     }

    //     if (isActive)
    //     {
    //         Vector2 direction = (player.position - transform.position).normalized;
    //         transform.Translate(direction * moveSpeed * Time.deltaTime);

    //         // Cambiar dirección del sprite
    //        // if (direction.x > 0) // Moviéndose a la derecha
    //         {
    //         //    transform.localScale = new Vector3(1, 1, 0); // Mirando a la derecha
    //         }
    //        // else if (direction.x < 0) // Moviéndose a la izquierda
    //         {
    //         //    transform.localScale = new Vector3(-1, 1, 0); // Mirando a la izquierda
    //         }
    //     }
    // }



    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Si colisiona con el player, le hace daño
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
    //         if (playerController != null)
    //         {
    //             playerController.TakeDamage(damageToPlayer);
    //         }
    //     }
    // }

    // public void TakeDamage(int damage)
    // {
    //     currentHealth -= damage; // Reduce la vida del enemigo
    //     if (currentHealth <= 0)
    //     {
    //         Die(); // Destruye el enemigo si su vida llega a 0
    //     }
    // }

    // void Die()
    // {
    //     Destroy(gameObject); // Destruye el enemigo
    // }
}