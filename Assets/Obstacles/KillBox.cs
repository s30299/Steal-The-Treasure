using UnityEngine;

public class KillBox : MonoBehaviour
{
    public bool doRespawn = true;//do you want do have respawn point or just death
    public GameObject respawnPoint;//set position of the empty object
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
