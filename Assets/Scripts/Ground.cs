using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }
    public bool OnLadder { get; private set; }
    public Collider2D CurrentLadder { get; private set; }
    public float Friction { get; private set; }
    public Vector2 ContactNormal { get; private set; }

    [Header("Ground Detection")]
    [SerializeField, Range(0f, 1f)] private float groundNormalMinY = 0.7f;
    [SerializeField, Range(0f, 1f)] private float wallNormalMinX = 0.9f;
    [SerializeField, Range(0f, 0.5f)] private float groundCheckDistance = 0.08f;
    [SerializeField] private LayerMask groundLayerMask = ~0;
    [SerializeField, Range(0f, 0.2f)] private float groundGraceTime = 0.05f;

    [Header("Ladder Detection")]
    [SerializeField] private string ladderTag = "Ladder";

    private Collider2D col;
    private float groundGraceTimer;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void ForceOnGround()
    {
        OnGround = true;
        groundGraceTimer = groundGraceTime;
    }

    private void FixedUpdate()
    {
        if (groundGraceTimer > 0f)
            groundGraceTimer -= Time.fixedDeltaTime;

        if (!OnGround && groundGraceTimer <= 0f)
        {
            CheckGroundByRaycast();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        OnWall = false;
        Friction = 0f;
        ContactNormal = Vector2.zero;
        groundGraceTimer = groundGraceTime;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        bool foundGround = false;
        bool foundWall = false;
        float bestNormalY = -1f;
        Vector2 bestNormal = Vector2.zero;

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i);
            Vector2 normal = contact.normal;

            if (normal.y > bestNormalY)
            {
                bestNormalY = normal.y;
                bestNormal = normal;
            }

            if (normal.y >= groundNormalMinY)
                foundGround = true;

            if (Mathf.Abs(normal.x) >= wallNormalMinX)
                foundWall = true;
        }

        OnGround = foundGround;
        OnWall = foundWall;
        ContactNormal = bestNormal;

        if (OnGround)
        {
            groundGraceTimer = groundGraceTime;
            RetrieveFriction(collision.collider);
        }
        else
        {
            Friction = 0f;
        }

        if (!OnGround)
            CheckGroundByRaycast();
    }

    private void CheckGroundByRaycast()
    {
        Vector2 origin = GetRayOrigin();

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayerMask);

        if (hit.collider != null && hit.normal.y >= groundNormalMinY)
        {
            OnGround = true;
            ContactNormal = hit.normal;

            PhysicsMaterial2D mat = hit.collider.sharedMaterial;
            Friction = mat != null ? mat.friction : 0f;
        }
        else if (groundGraceTimer <= 0f)
        {
            OnGround = false;
            Friction = 0f;
        }
    }

    private Vector2 GetRayOrigin()
    {
        Bounds b = col.bounds;
        return new Vector2(b.center.x, b.min.y + 0.02f);
    }

    private void RetrieveFriction(Collider2D hitCollider)
    {
        if (hitCollider == null)
        {
            Friction = 0f;
            return;
        }

        PhysicsMaterial2D mat = hitCollider.sharedMaterial;
        Friction = mat != null ? mat.friction : 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EvaluateTrigger(other, true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EvaluateTrigger(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EvaluateTrigger(other, false);
    }

    private void EvaluateTrigger(Collider2D other, bool withContact)
    {
        if (!other.CompareTag(ladderTag))
            return;

        OnLadder = withContact;
        CurrentLadder = withContact ? other : null;
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c == null) return;

        Bounds b = c.bounds;
        Vector2 origin = new Vector2(b.center.x, b.min.y + 0.02f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + Vector2.down * groundCheckDistance);
        Gizmos.DrawSphere(origin, 0.02f);
    }
}