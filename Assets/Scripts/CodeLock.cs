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

    private int[] correctCode = {1, 2, 3};

    public void IncreaseDigit(int index)
    {
        Debug.LogWarning("IncreaseDigit called with index: " + index);
        if (index == 0) digit1 = (digit1 + 1) % 10;
        if (index == 1) digit2 = (digit2 + 1) % 10;
        if (index == 2) digit3 = (digit3 + 1) % 10;

        UpdateUI();
        CheckCode();
    }

    void UpdateUI()
    {
        digit1Text.text = digit1.ToString();
        digit2Text.text = digit2.ToString();
        digit3Text.text = digit3.ToString();
    }

    void CheckCode()
    {
        if (digit1 == correctCode[0] &&
            digit2 == correctCode[1] &&
            digit3 == correctCode[2])
        {
            Debug.Log("Unlocked!");
        }
    }
}