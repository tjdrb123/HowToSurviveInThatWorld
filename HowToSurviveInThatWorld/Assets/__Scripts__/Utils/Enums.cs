
/*
 * Enums (상수형 타입)
 *
 * 클래스가 필요 없이 <GlobalUsingNamespace>를 사용
 * 클래스에 안 넣으셔도 됩니다.
 * 
 * ex)
 * public enum E_Game_State { type... }
 * public enum E_Player_State { types... }
*/

#region PlayerFSM Enum

public enum E_Player_Locations
{
    Dungeon = 1,
    Village,
};

public enum E_Player_States
{
    NormallyState,
    GlobalState,
}

#endregion


#region Coroutine Key

// * 코루틴 키 관련 타입
//   - 어떤 코루틴을 쓸 때 여기에 꼭 해당하는 코루틴의 기능을 작성해주세요.
//   - ex) NetworkLogin 과 같은 타입을 안에 작성하면 됩니다.
public enum E_CoroutineKey
{
    None
}

#endregion



#region Status

public enum E_StatModifier_OperationType
{
    Additive,
    Multiplicative,
    Override
}

#endregion