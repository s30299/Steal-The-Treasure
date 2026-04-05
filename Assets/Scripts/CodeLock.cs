using UnityEngine;
using TMPro;

public class CodeLock : MonoBehaviour
{
    public TMP_Text digit1Text;
    public TMP_Text digit2Text;
    public TMP_Text digit3Text;

    private int digit1 = 0;
    private int digit2 = 0;
    private int digit3 = 0;

    public int[] correctCode = { 10, 10, 10 };

    private void Start()
    {
        UpdateUI();
        correctCode[0] = PlayerPrefs.GetInt("Code1");
        correctCode[1] = PlayerPrefs.GetInt("Code2");
        correctCode[2] = PlayerPrefs.GetInt("Code3");
    }
    
    public void IncreaseDigit(int index)
    {
        // Debug.LogWarning("IncreaseDigit called with index: " + index);

        if (index == 0) digit1 = (digit1 + 1) % 10;
        if (index == 1) digit2 = (digit2 + 1) % 10;
        if (index == 2) digit3 = (digit3 + 1) % 10;

        UpdateUI();
        CheckCode();
    }

    public void DecreaseDigit(int index)
    {
        // Debug.LogWarning("DecreaseDigit called with index: " + index);

        if (index == 0) digit1 = (digit1 - 1 + 10) % 10;
        if (index == 1) digit2 = (digit2 - 1 + 10) % 10;
        if (index == 2) digit3 = (digit3 - 1 + 10) % 10;

        UpdateUI();
        CheckCode();
    }

    private void UpdateUI()
    {
        digit1Text.text = digit1.ToString();
        digit2Text.text = digit2.ToString();
        digit3Text.text = digit3.ToString();
    }

    private void CheckCode()
    {
        if (digit1 == correctCode[0] &&
            digit2 == correctCode[1] &&
            digit3 == correctCode[2])
        {
            UIManager.ShowWinScreen();
        }
    }
}