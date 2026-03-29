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

    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

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