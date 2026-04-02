using UnityEngine;
using UnityEngine.EventSystems;

public class CodeLockButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private CodeLock codeLock;
    [SerializeField] private int digitIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (codeLock == null)
        {
            Debug.LogWarning("CodeLockButton: codeLock is not assigned.");
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            codeLock.IncreaseDigit(digitIndex);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            codeLock.DecreaseDigit(digitIndex);
        }
    }
}