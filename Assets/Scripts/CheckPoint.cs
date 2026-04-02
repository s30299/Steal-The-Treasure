using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetFloat("posX",transform.position.x);
            PlayerPrefs.SetFloat("posY",transform.position.y);
            PlayerPrefs.Save();
        }
    }
}
