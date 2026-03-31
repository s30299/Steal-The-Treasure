using UnityEngine;

public class Interact : Capability
{
    [Header("Interaction")]
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionRadius = 0.8f;
    [SerializeField] private LayerMask interactionLayers;

    protected override void Update()
    {
        base.Update();

        if (IsLocked)
            return;

        if (Controller.RetrieveInteractInput())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Vector2 center = interactionPoint != null
            ? interactionPoint.position
            : Controller.transform.position;

        Collider2D hit = Physics2D.OverlapCircle(center, interactionRadius, interactionLayers);

        if (hit == null)
        {
            Debug.Log("Interact: nothing found.");
            return;
        }

        Debug.Log($"Interact hit: {hit.name}");

        Collectible collectible = hit.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectible.Collect();
            return;
        }

        Debug.Log($"Interact: {hit.name} has no Collectible component.");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center;

        if (interactionPoint != null)
            center = interactionPoint.position;
        else
            center = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, interactionRadius);
    }
}