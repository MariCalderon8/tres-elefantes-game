using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float runSpeed ;
    public float jumpForce;
    public int maxHealth = 3; // Vida del player
    public int attackDamage = 1; // Daño al enemigo por ataque

    // Detección de suelo
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private int currentHealth;
    
    private bool wasGrounded;
    private bool isGrounded;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDead = false;

    // Parámetros del Animator
    private readonly string paramMovement = "Move";
    private readonly string paramSpeeding = "isRunning";
    private readonly string paramJump = "isJumping";
    private readonly string paramAttack = "isAttacking";
    private readonly string paramFalling = "isFalling";

//llamar en esta función los dos componentes del player rb y anim
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; // Inicializa la vida del player
    }

    void FixedUpdate()
    {
        if (isDead) return;
        // Almacena el estado anterior para detectar cambios
        wasGrounded = isGrounded;
        // Actualiza el estado del suelo usando Physics2D.OverlapCircle
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        Move();
        Jump();
        UpdateAnimations();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        
        animator.SetFloat(paramMovement, Mathf.Abs(moveInput));
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool("isRunning", isRunning);
        
        // Mirar en la dirección del movimiento
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void Jump()
    {
        if (isGrounded && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W)))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetBool(paramJump, true);
            animator.SetBool(paramFalling, false);
            // audioSource.PlayOneShot(jumpSound);
        }
    }
    
    void UpdateAnimations()
    {
        // Si acaba de tocar el suelo, desactivar animaciones de salto/caída
        if (isGrounded && !wasGrounded)
        {
            animator.SetBool(paramJump, false);
            animator.SetBool(paramFalling, false);
        }
        // Si está en el aire
        else if (!isGrounded)
        {
            // Subiendo (velocidad vertical positiva) -> animación de salto
            if (rb.linearVelocity.y > 0.1f)
            {
                animator.SetBool(paramJump, true);
                animator.SetBool(paramFalling, false);
            }
            // Cayendo (velocidad vertical negativa) -> animación de caída
            else if (rb.linearVelocity.y < -0.1f)
            {
                animator.SetBool(paramJump, false);
                animator.SetBool(paramFalling, true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el player ataca al enemigo
        if (collision.gameObject.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.Z))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage); // Hace daño al enemigo
            }
        }
    

    
    // if (collision.gameObject.CompareTag("PlataformaMovil"))
    // {
    //     transform.SetParent(collision.transform); // Hace que el jugador sea hijo de la plataforma
    // }
    

    // if (collision.gameObject.CompareTag("PlataformaMovil"))
    // {
    //     transform.SetParent(null); // Libera al jugador de la plataforma
    // }
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

//llamar las fisicas del personaje.
    // void FixedUpdate()
    // {


    //     // Animaciones
    //     animator.SetFloat("Speed", Mathf.Abs(moveInput));

    //     if (Input.GetButtonDown("Jump") && !isJumping)
    //     {
    //         rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    //         isJumping = true;
    //         animator.SetBool("IsJumping", true);
    //     }

    //     if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
    //     {
    //         isAttacking = true;
    //         animator.SetBool("IsAttacking", true);
    //     }

    //     if (Input.GetKeyDown(KeyCode.X))
    //     {
    //         isDead = true;
    //         animator.SetBool("IsDead", true);
    //     }
    // }