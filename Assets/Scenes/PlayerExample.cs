using UnityEngine;

public class PlayerExample : MonoBehaviour
{
    [Header("Movimiento Básico")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    
    [Header("Configuración de Salto")]
    public float jumpForce = 12f;           // Fuerza inicial del salto
    public float fallMultiplier = 2.5f;     // Multiplicador de gravedad al caer
    public float lowJumpMultiplier = 2f;    // Multiplicador cuando sueltas el botón de salto
    public float jumpBufferTime = 0.1f;     // Tiempo de buffer para registrar el salto antes de tocar suelo
    public float coyoteTime = 0.15f;        // Tiempo que puedes saltar después de dejar el suelo
    public int airJumps = 0;                // Número de saltos adicionales en el aire (0 = sin doble salto)
    
    [Header("Estadísticas del Personaje")]
    public int maxHealth = 3;
    public int attackDamage = 1;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    // Referencias a componentes
    private Rigidbody2D rb;
    private Animator animator;
    private int currentHealth;
    
    // Estado del personaje
    private bool wasGrounded;
    private bool isGrounded;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isDead = false;
    private int airJumpsLeft;
    
    // Variables para el buffer de salto y coyote time
    private float jumpBufferCounter;
    private float coyoteTimeCounter;
    
    // Variables para entrada
    private bool jumpInput = false;
    private bool jumpHeld = false;
    private float moveInput;

    // Parámetros del Animator
    private readonly string paramMovement = "Move";
    private readonly string paramSpeeding = "isRunning";
    private readonly string paramJump = "isJumping";
    private readonly string paramAttack = "isAttacking";
    private readonly string paramFalling = "isFalling";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        airJumpsLeft = airJumps;
        
        // Verificar componentes necesarios
        if (groundCheck == null)
        {
            Debug.LogError("El transform groundCheck no está asignado. Por favor asigna un punto de verificación de suelo.");
        }
        
        // Aplicar configuración de física para mejor control
        if (rb != null)
        {
            rb.freezeRotation = true;
            
            // Ajustar la amortiguación lineal para movimiento más preciso
            rb.linearDamping = 0;
        }
    }
    
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        jumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.W);
        
        if (Input.GetKeyDown(KeyCode.Z) && !isAttacking)
        {
            isAttacking = true;
            // animator.SetBool("IsAttacking", true);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        
        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            airJumpsLeft = airJumps; // Restaurar saltos aéreos
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
        
        Move();
        
        Jump();
        
        ApplyJumpPhysics();
        
        UpdateAnimations();
    }

    void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        
        // Aplicar velocidad horizontal sin afectar la velocidad vertical
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        
        // Actualizar animación de movimiento
        // animator.SetFloat(paramMovement, Mathf.Abs(moveInput));
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f;
        // animator.SetBool("isRunning", isRunning);
        
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
        // Salto normal con buffer de salto y coyote time
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            // Aplicar fuerza de salto
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Anular velocidad vertical antes de saltar
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            // Actualizar estado y animaciones
            isJumping = true;
            // animator.SetBool(paramJump, true);
            // animator.SetBool(paramFalling, false);
            
            // Resetear contadores
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
        // Doble salto (o múltiples saltos en aire)
        else if (jumpBufferCounter > 0f && !isGrounded && airJumpsLeft > 0)
        {
            // Aplicar fuerza de salto para salto aéreo
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Anular velocidad vertical antes de saltar
            rb.AddForce(Vector2.up * jumpForce * 0.8f, ForceMode2D.Impulse); // Un poco menos de fuerza que el salto normal
            
            // Actualizar estado y animaciones
            isJumping = true;
            // animator.SetBool(paramJump, true);
            // animator.SetBool(paramFalling, false);
            
            // Decrementar saltos aéreos disponibles y resetear buffer
            airJumpsLeft--;
            jumpBufferCounter = 0f;
        }
    }
    
    void ApplyJumpPhysics()
    {
        // Si está cayendo, aplicar mayor gravedad
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Si está subiendo pero no se mantiene el botón de salto, aplicar gravedad baja
        else if (rb.linearVelocity.y > 0 && !jumpHeld)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
    
    void UpdateAnimations()
    {
        // Si acaba de tocar el suelo, desactivar animaciones de salto/caída
        if (isGrounded && !wasGrounded)
        {
            isJumping = false;
            // animator.SetBool(paramJump, false);
            // animator.SetBool(paramFalling, false);
        }
        // Si está en el aire
        else if (!isGrounded)
        {
            // Subiendo (velocidad vertical positiva) -> animación de salto
            if (rb.linearVelocity.y > 0.1f)
            {
                // animator.SetBool(paramJump, true);
                // animator.SetBool(paramFalling, false);
            }
            // Cayendo (velocidad vertical negativa) -> animación de caída
            else if (rb.linearVelocity.y < -0.1f)
            {
                // animator.SetBool(paramJump, false);
                // animator.SetBool(paramFalling, true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el player ataca al enemigo
        if (collision.gameObject.CompareTag("Enemy") && isAttacking)
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            transform.SetParent(collision.transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovil"))
        {
            transform.SetParent(null);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
        // animator.SetBool("IsAttacking", false);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player ha muerto");
    }
    
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}