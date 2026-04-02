using UnityEngine;

public class KillBox : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var otherObject=collision.gameObject;
        if (otherObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().OnDeath();
        }
        else
        {
            Destroy(otherObject);
        }
    }
}
