using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    // ground check cariables to be set in inspector
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private LayerMask deathLayer;
    [SerializeField]
    private float layerCheckRadius = 0.2f;
    public bool isGrounded { get; private set; }
    public bool isDead { get; private set; }


    private Collider2D col;
    private Rigidbody2D rb;
    private Vector2 layerCheckPos => CalculateLayerCheckPos();

    // Foot position helper function to calculate the ground check position based ont he collider's bounds
    private Vector2 CalculateLayerCheckPos() {
        Bounds bounds = col.bounds;
        return new Vector2(bounds.center.x, bounds.min.y);
    }

    public void Init(Collider2D col, Rigidbody2D rb) {
        this.col = col;
        this.rb = rb;
    }

    // Update is called once per frame
    public bool CheckGround() {
        if (!isGrounded && rb.linearVelocityY <= 0 || isGrounded) {
            isGrounded = Physics2D.OverlapCircle(layerCheckPos, layerCheckRadius, groundLayer);
        }

        return isGrounded;
    }

    public bool CheckDeath() {
        if (!isDead && rb.linearVelocityY <= 0 || isDead)
        {
            isDead = Physics2D.OverlapCircle(layerCheckPos, layerCheckRadius, deathLayer);
        }
        return isDead; 
    }
}
