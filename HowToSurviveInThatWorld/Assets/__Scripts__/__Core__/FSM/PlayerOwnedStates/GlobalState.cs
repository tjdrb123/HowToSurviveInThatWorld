
/* 플레이어의 현재 상태와 별개로, 모든 상태에서 지속적으로 업데이트 되어야 하는 상태입니다. ex) 스태미너 감소
 따라서 Enter() 와 Exit()는 내용이 없습니다. */

using UnityEngine;

public class GlobalState : State<PlayerFSM>
{
    public override void Enter(PlayerFSM entity)
    {
    }

    public override void ExecuteUpdate(PlayerFSM entity)
    {
        Debug.Log("GlobalState ExecuteUpdate");
    }

    public override void ExecuteFixedUpdate(PlayerFSM entity)
    {
        Debug.Log("GlobalState ExecuteFixedUpdate");
    }

    public override void Exit(PlayerFSM entity)
    {
    }
}