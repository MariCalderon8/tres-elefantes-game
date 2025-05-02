using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public int maxHealth = 3; // Vida del player
    public int attackDamage = 1; // Daño al enemigo por ataque

    private Rigidbody2D rb;
    private Animator animator;
    private bool isJumping = false;
    private int currentHealth;
    private bool isAttacking = false;
    private bool isDead = false;

//llamar en esta función los dos componentes del player rb y anim
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Inicializa la vida del player
    }


//llamar las fisicas del personaje.
    void FixedUpdate()
    {
        if (isDead) return;

        float moveInput = Input.GetAxis("Horizontal");
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);


        // Cambiar dirección del sprite
       // if (moveInput > 0) // Moviéndose a la derecha
        {
           // transform.localScale = new Vector3(1, 1, 0); // Mirando a la derecha
        }
       // else if (moveInput < 0) // Moviéndose a la izquierda
        {
           // transform.localScale = new Vector3(-1, 1, 0); // Mirando a la izquierda
        }


        // Animaciones
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            isDead = true;
            animator.SetBool("IsDead", true);
        }
    }



//llamar la condición Ground que sera la que detecta el piso para caminar y saltar.

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
        }

        // Si el player ataca al enemigo
        if (collision.gameObject.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.Z))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage); // Hace daño al enemigo
            }
        }
    

    
    if (collision.gameObject.CompareTag("PlataformaMovil"))
    {
        transform.SetParent(collision.transform); // Hace que el jugador sea hijo de la plataforma
    }
    

    if (collision.gameObject.CompareTag("PlataformaMovil"))
    {
        transform.SetParent(null); // Libera al jugador de la plataforma
    }
    }



//llamar la función de atacar y final del ataque
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage; // Reduce la vida del player
        if (currentHealth <= 0)
        {
            Die(); // Mata al player si su vida llega a 0
        }
    }

    void Die()
    {
        isDead = true;
        // Aquí puedes añadir lógica para la animación de muerte o reiniciar el nivel
        Debug.Log("Player ha muerto");
    }



   
   
}