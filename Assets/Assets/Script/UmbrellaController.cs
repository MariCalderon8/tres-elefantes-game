using UnityEngine;
public class UmbrellaController : MonoBehaviour
{
    public float bounceForce;
    private readonly string paramBouncing = "isBouncing";
    private Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GroundCheck"))
        {
            Rigidbody2D playerRb = collider.gameObject.GetComponentInParent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);
                playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                
                animator.SetBool(paramBouncing, true);
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("GroundCheck"))
        {
            animator.SetBool(paramBouncing, false);
        }
    }
}