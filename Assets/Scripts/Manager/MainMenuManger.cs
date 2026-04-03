using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManger : MonoBehaviour
{
    [SerializeField]private GameObject firstButton;
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

}
