using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    //[Header("Target")]
    /*[SerializeField]*/ private Transform target;

    [Header("Smooth Follow")]
    [SerializeField] private float smoothTime = 0.2f;

    [Header("Max Distance")]
    [SerializeField] private float maxDistance = 3f;

    [Header("Offset")]
    [SerializeField] private Vector3 offset = new(0f, 1.5f, -10f);
    [SerializeField] private float sideOffset=2f;
    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {target = player.transform; }
    }

    private void LateUpdate()
    {
        if (target == null) return;
        switch (Controller.GetMoveDirection())
        {
            case MovementDirection.Left:offset.x=-sideOffset; break;
            case MovementDirection.Right:offset.x=sideOffset; break;
            case MovementDirection.Standing:offset.x=0; break;
        }

        Vector3 desiredPosition = target.position + offset;

        // aktualna pozycja kamery
        Vector3 currentPosition = transform.position;

        // dystans między kamerą a targetem
        float distance = Vector3.Distance(currentPosition, desiredPosition);

        // jeśli kamera za daleko → szybciej nadgania
        if (distance > maxDistance)
        {
            transform.position = Vector3.MoveTowards(currentPosition, desiredPosition, distance - maxDistance);
        }

        // smooth follow (opóźnienie)
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}