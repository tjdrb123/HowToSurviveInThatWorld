
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


#region Coroutine Key

// * 코루틴 키 관련 타입
//   - 어떤 코루틴을 쓸 때 여기에 꼭 해당하는 코루틴의 기능을 작성해주세요.
//   - ex) NetworkLogin 과 같은 타입을 안에 작성하면 됩니다.
public enum E_CoroutineKey
{
    ChargeFillAmount,
}

#endregion


#region Node State

public enum E_NodeState
{
    Running,
    Success,
    Failure
}

#endregion


#region Stat Type

public enum E_StatType
{
    None,
    Health,
    Hunger,
    Exp,
    MoveSpeed,
    Damage,
    Defense,
    CriticalChance,
    CriticalDamage,
    Hungry,
}

#endregion


#region Stat Modeifier Operator Type

public enum E_StatModifier_OperatorType
{
    Additive,
    Multiplicative,
    Override
}

#endregion