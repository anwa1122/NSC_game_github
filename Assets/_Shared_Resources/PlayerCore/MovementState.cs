using UnityEngine;


public enum MoveMode { StartMoving, StopMoving }
public class MovementState : MonoBehaviour
{
    public static MovementState Instance;
    public PlayerNormalMove playerNormalMove;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void ChangeMovementState(MoveMode moveMent)
    {
        if (moveMent == MoveMode.StartMoving)
        {
            playerNormalMove.enabled = true;
        }
        else if (moveMent == MoveMode.StopMoving)
        {
            playerNormalMove.enabled = false;
        }
    }
}
