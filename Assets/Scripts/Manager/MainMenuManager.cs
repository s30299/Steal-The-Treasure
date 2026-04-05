using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]private GameObject firstButton;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject optionsFirstButton;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private GameObject optionsSFX;
    private Slider SFXslider;
    [SerializeField] private GameObject optionsMusic;
    private Slider musicSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("currentLevel"))
        {
            continueButton.SetActive(false);
        }
        else
        {
            firstButton = continueButton;
        }
        InputManager.ShowCursor();
        DisableOptionsMenu();
        EnableMainMenu();
    }

    private void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        SetActiveButton(firstButton);
    }
    private void DisableMainMenu() 
    {
        mainMenu.SetActive(false);
        ClearActiveButton();
    }
    private void EnableOptionsMenu()
    {
        optionsMenu.SetActive(true);
        SetActiveButton(optionsFirstButton);
        if(SFXslider==null || musicSlider == null)
        {
            SFXslider=optionsSFX.GetComponent<Slider>();
            musicSlider=optionsMusic.GetComponent<Slider>();
        }
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        SFXslider.value = PlayerPrefs.GetFloat("effectsVolume", 1);
    }
    private void DisableOptionsMenu() 
    {
        optionsMenu.SetActive(false);
        ClearActiveButton();
    }

    public void Switch()
    {
        if (mainMenu.activeInHierarchy)
        {
            DisableMainMenu();
            EnableOptionsMenu();
        }
        else
        {
            DisableOptionsMenu();
            EnableMainMenu();
        }
    }

    private void SetActiveButton(GameObject button)
    {
        if (InputManager.UsingController() || InputManager.UsingKeyboard())
        {
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
    private void ClearActiveButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
