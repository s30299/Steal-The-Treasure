using UnityEngine;

public class KillBox : MonoBehaviour
{
    public bool doRespawn = true;
    public GameObject respawnPoint;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var otherObject=collision.gameObject;
        if (otherObject.CompareTag("Player") && doRespawn)
        {
            otherObject.transform.SetPositionAndRotation(respawnPoint.transform.position,otherObject.transform.rotation);
        }
        else
        {
            Destroy(otherObject);
        }
    }
}
