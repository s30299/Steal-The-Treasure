using UnityEditor;
using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    //[SerializeField] private SceneAsset nextLevel;
    [SerializeField] private Vector2 exitCoords;
    [SerializeField] private bool useExitCoords = false;
    [SerializeField] private string nextLevel;
    private PlayerController playerController;
    private bool inputActive=false;
    public void Update()
    {
        if (inputActive)
        {
            if (playerController.RetrieveInteractInput())
            {
                if (useExitCoords)
                {
                    PlayerPrefs.SetFloat("posX", exitCoords.x);
                    PlayerPrefs.SetFloat("posY", exitCoords.y);
                    PlayerPrefs.Save();
                }
                else
                {
                    PlayerPrefs.DeleteKey("posX");
                    PlayerPrefs.DeleteKey("posY");
                    PlayerPrefs.Save();
                }
                InputManager.DisableInput();
                LevelManager.ChangeLevel(nextLevel);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inputActive = true;
            UIManager.ShowTooltip("Press interact to move to " + nextLevel);
            if (playerController == null)
            {
                playerController=collision.gameObject.GetComponent<PlayerController>();
                
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inputActive = false;
            UIManager.HideTooltip();
        }
    }
}
