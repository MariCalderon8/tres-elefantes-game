using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
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
    public int airJumps = 1;

    [Header("Configuración de Daño")]
    public bool canMove = true;
    [SerializeField] private Vector2 bounceVelocity;
    [SerializeField] private float lostControlTime;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public float flickerDuration = 0.5f;
    public float flickerSpeed = 0.1f;

    [Header("Estadísticas del Personaje")]
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;

    public int attackDamage = 1;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    //Referencias a componentes
    private Rigidbody2D rb;
    private Animator animator;

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
    private readonly string paramGrounded = "isGrounded";

    //llamar en esta función los dos componentes del player rb y anim
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            animator.SetBool(paramAttack, true);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        animator.SetBool(paramGrounded, isGrounded);
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            airJumpsLeft = airJumps; // Restaurar saltos aéreos
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        if (canMove)
        {
            Move();
        }
        Jump();
        ApplyJumpPhysics();
        UpdateAnimations();
    }

    public void Bounce(Vector2 damagePoint)
    {
        rb.linearVelocity = new Vector2(-bounceVelocity.x * damagePoint.x, bounceVelocity.y);
    }

    void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        animator.SetFloat(paramMovement, Mathf.Abs(moveInput));
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f;
        animator.SetBool(paramSpeeding, isRunning);

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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Anular velocidad vertical antes de saltar
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            isJumping = true;
            animator.SetBool(paramJump, true);
            // animator.SetBool(paramFalling, false);

            // Resetear contadores
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Doble salto
        else if (jumpBufferCounter > 0f && !isGrounded && airJumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Anular velocidad vertical antes de saltar
            rb.AddForce(Vector2.up * jumpForce * 0.8f, ForceMode2D.Impulse); // Un poco menos de fuerza que el salto normal

            // Actualizar estado y animaciones
            isJumping = true;
            animator.SetBool(paramJump, true);
            // animator.SetBool(paramFalling, false);

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
        // Cuando se mantiene presionado el botón de salto
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
                //enemy.TakeDamage(attackDamage); 
            }
        }

    }



    //llamar la función de atacar y final del ataque
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }


    public void TakeDamage(int damage, Vector2 damagePoint)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(FlickerWhite());
            StartCoroutine(LostControl());
            Bounce(damagePoint);
        }

    }

    public void TakeDamageOnTrigger(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(FlickerWhite());
            StartCoroutine(LostControl());
        }

    }

    private IEnumerator FlickerWhite()
    {
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        Color flickerColor = new Color(1f, 1f, 1f, 0.7f); // Blanco translúcido

        while (elapsed < flickerDuration)
        {
            spriteRenderer.color = flickerColor;
            yield return new WaitForSeconds(flickerSpeed);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed * 2;
        }

        spriteRenderer.color = originalColor;
    }

    private IEnumerator LostControl()
    {
        canMove = false;
        yield return new WaitForSeconds(lostControlTime);
        canMove = true;
    }

    public IEnumerator Die()
    {
        isDead = true;
        animator.SetBool("isDead", isDead);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game Over");
    }

}
