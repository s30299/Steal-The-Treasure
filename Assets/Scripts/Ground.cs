using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    public bool OnGround { get; private set; }
    public bool OnWall { get; private set; }
    public bool OnLadder { get; private set; }
    public float Friction { get; private set; }
    public Vector2 ContactNormal { get; private set; }

    [Header("Ground Detection")]
    [SerializeField, Range(0f, 1f)] private float groundNormalMinY = 0.7f;
    [SerializeField, Range(0f, 1f)] private float wallNormalMinX = 0.9f;
    [SerializeField, Range(0f, 0.5f)] private float groundCheckDistance = 0.08f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField, Range(0f, 0.2f)] private float groundGraceTime = 0.05f;

    [Header("Ladder Detection")]
    [SerializeField] private LayerMask ladderLayerMask;

    private Collider2D col;
    private float groundGraceTimer;

    private readonly Collider2D[] ladderResults = new Collider2D[8];
    private readonly RaycastHit2D[] groundHits = new RaycastHit2D[8];

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (groundGraceTimer > 0f)
            groundGraceTimer -= Time.fixedDeltaTime;

        CheckGround();
        CheckLadder();
    }

    private void CheckGround()
    {
        Bounds b = col.bounds;

        Vector2 originLeft = new Vector2(b.min.x + 0.05f, b.min.y + 0.02f);
        Vector2 originCenter = new Vector2(b.center.x, b.min.y + 0.02f);
        Vector2 originRight = new Vector2(b.max.x - 0.05f, b.min.y + 0.02f);

        bool foundGround = false;
        bool foundWall = false;
        Vector2 bestNormal = Vector2.zero;
        float bestNormalY = float.NegativeInfinity;
        float bestFriction = 0f;

        CheckGroundRay(originLeft, ref foundGround, ref foundWall, ref bestNormal, ref bestNormalY, ref bestFriction);
        CheckGroundRay(originCenter, ref foundGround, ref foundWall, ref bestNormal, ref bestNormalY, ref bestFriction);
        CheckGroundRay(originRight, ref foundGround, ref foundWall, ref bestNormal, ref bestNormalY, ref bestFriction);

        if (foundGround)
        {
            OnGround = true;
            ContactNormal = bestNormal;
            Friction = bestFriction;
            groundGraceTimer = groundGraceTime;
        }
        else if (groundGraceTimer <= 0f)
        {
            OnGround = false;
            Friction = 0f;
            ContactNormal = Vector2.zero;
        }

        OnWall = foundWall;
    }

    private void CheckGroundRay(
        Vector2 origin,
        ref bool foundGround,
        ref bool foundWall,
        ref Vector2 bestNormal,
        ref float bestNormalY,
        ref float bestFriction)
    {
        int hitCount = Physics2D.RaycastNonAlloc(origin, Vector2.down, groundHits, groundCheckDistance, groundLayerMask);

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = groundHits[i];
            if (hit.collider == null)
                continue;

            Vector2 normal = hit.normal;

            if (normal.y >= groundNormalMinY)
            {
                foundGround = true;

                if (normal.y > bestNormalY)
                {
                    bestNormalY = normal.y;
                    bestNormal = normal;

                    PhysicsMaterial2D mat = hit.collider.sharedMaterial;
                    bestFriction = mat != null ? mat.friction : 0f;
                }
            }

            if (Mathf.Abs(normal.x) >= wallNormalMinX)
            {
                foundWall = true;
            }
        }
    }

    private void CheckLadder()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useLayerMask = true;
        filter.layerMask = ladderLayerMask;
        filter.useTriggers = true;

        int count = col.Overlap(filter, ladderResults);
        OnLadder = count > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c == null) return;

        Bounds b = c.bounds;

        Vector2 originLeft = new Vector2(b.min.x + 0.05f, b.min.y + 0.02f);
        Vector2 originCenter = new Vector2(b.center.x, b.min.y + 0.02f);
        Vector2 originRight = new Vector2(b.max.x - 0.05f, b.min.y + 0.02f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(originLeft, originLeft + Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(originCenter, originCenter + Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(originRight, originRight + Vector2.down * groundCheckDistance);

        Gizmos.DrawSphere(originLeft, 0.02f);
        Gizmos.DrawSphere(originCenter, 0.02f);
        Gizmos.DrawSphere(originRight, 0.02f);
    }
}