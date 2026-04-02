using UnityEngine;

public class CodeLockWorldButton : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CodeLock codeLock;

    [Header("Settings")]
    [Tooltip("0 = digit1, 1 = digit2, 2 = digit3")]
    [SerializeField] private int digitIndex = 0;

    [Tooltip("true = +1, false = -1")]
    [SerializeField] private bool increase = true;

    public void Collect()
    {
        Debug.LogWarning($"start");
        if (codeLock == null)
        {
            Debug.LogWarning($"[{name}] Missing CodeLock reference.");
            return;
        }

        if (digitIndex < 0 || digitIndex > 2)
        {
            Debug.LogWarning($"[{name}] Invalid digitIndex: {digitIndex}");
            return;
        }

        if (increase)
        {
            codeLock.IncreaseDigit(digitIndex);
            Debug.Log($"[{name}] +1 digit {digitIndex}");
        }
        else
        {
            codeLock.DecreaseDigit(digitIndex);
            Debug.Log($"[{name}] -1 digit {digitIndex}");
        }
    }
}