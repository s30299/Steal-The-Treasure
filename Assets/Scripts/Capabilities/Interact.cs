using UnityEngine;

public class Interact : Capability
{
    [SerializeField] private float _interactionRange = 1f;
    [SerializeField] private LayerMask _interactionLayers;

    [Header("Cooldown")]
    [SerializeField] private float _interactionCooldown = 0.5f;
    private float _cooldownTimer = 0f;

    private Vector2 _lastDirection = Vector2.right;

    protected override void Update()
    {
        base.Update();

        if (IsLocked)
            return;

        UpdateFacingDirection();

        // 🔹 odliczanie cooldownu
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        // 🔹 tylko jeśli cooldown minął
        if (_cooldownTimer <= 0f && Controller.RetrieveInteractInput())
        {
            if (TryInteract())
            {
                _cooldownTimer = _interactionCooldown;
            }
        }
    }

    private void UpdateFacingDirection()
    {
        Vector2 moveInput = Controller.RetrieveMoveInput();

        if (moveInput.x != 0)
        {
            _lastDirection = new Vector2(Mathf.Sign(moveInput.x), 0f);
        }
    }

    private bool TryInteract()
    {
        Vector2 origin = Controller.Anchor.position;
        Vector2 direction = _lastDirection;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, _interactionRange, _interactionLayers);

        if (hit.collider != null)
        {
            // 🔹 Collectible
            var collectible = hit.collider.GetComponent<Collectible>();
            if (collectible != null)
            {
                collectible.Collect();
                return true;
            }

            // 🔹 CodeLock button
            var interactable = hit.collider.GetComponent<CodeLockWorldButton>();
            if (interactable != null)
            {
                interactable.Collect(); // poprawione
                return true;
            }
        }

        return false;
    }
}