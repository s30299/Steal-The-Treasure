using UnityEngine;

public interface IInputRetriever
{
    Vector2 RetrieveMoveInput();
    bool RetrieveJumpInput();
    bool RetrieveInteractInput();
    bool RetrieveAttackInput();
    bool RetrieveDashInput();
    bool RetrieveLeftMouseInput();
    bool RetrieveRightMouseInput();
}
