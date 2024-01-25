
// 
using UnityEngine;

public class NormallyState : State<PlayerFSM>
{
    public override void Enter(PlayerFSM entity)
    {
        
    }

    public override void ExecuteUpdate(PlayerFSM entity)
    {
        Debug.Log("NormallyState ExecuteUpdate");
    }

    public override void ExecuteFixedUpdate(PlayerFSM entity)
    {
        Debug.Log("NormallyState ExecuteFixedUpdate");
    }

    public override void Exit(PlayerFSM entity)
    {
        
    }
}
