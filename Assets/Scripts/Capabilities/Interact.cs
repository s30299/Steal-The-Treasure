using UnityEngine;

public class Interact : Capability
{
    [SerializeField] private float _interactionRange = 1f;
    [SerializeField] private LayerMask _interactionLayers;

    private Vector2 _lastDirection = Vector2.right;

    protected override void Update()
    {
        base.Update();

        if (IsLocked)
        {
            return;
        }

        UpdateFacingDirection();

        if (Controller.RetrieveInteractInput())
        {
            TryInteract();
        }
    }

    private void UpdateFacingDirection()
    {
        Vector2 moveInput = Controller.RetrieveMoveInput();
        if (moveInput.x != 0)
        {
            _lastDirection = new Vector2(moveInput.x, 0f);
        }
    }

    private void TryInteract()
    {
        Vector2 origin = Controller.Anchor.position;
        Vector2 direction = _lastDirection;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, _interactionRange, _interactionLayers);

        if (hit.collider != null)
        {
            var collectible = hit.collider.GetComponent<Collectible>();
            if (collectible != null)
            {
                collectible.Collect();
            }
        }
    }


}