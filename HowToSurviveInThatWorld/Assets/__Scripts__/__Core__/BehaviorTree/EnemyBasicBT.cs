using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBasicBT : MonoBehaviour
{
    #region Global Variable

    [Header("Distance")] 
    [SerializeField] 
    private float _detectDistance;
    
    private DataContext _enemyData;
    private BehaviorTreeRunner _btRunner;
    

    

    #endregion
    
    

    #region Unity Event Method

    private void Awake()
    {
        InitializeData();
    }

    private void Update()
    {
        throw new NotImplementedException();
    }

    #endregion
    
    

    # region Setting BT
    
    private INode SettingBT()
    {
        return new SelectorNode
        (
            new List<INode>()
            {
                new Inverter
                (
                    new SelectorNode    // ## 적 감지 & 순찰 분기 노드
                    (
                        new List<INode>()
                        {
                            new ActionNode(Test1), // 범위 안에 적이 있는가?
                            new SelectorNode    // ## 순찰 판정 분기 노드
                            (
                                new List<INode>()
                                {
                                    new UntilFail
                                    (
                                        new ActionNode(Test1) // 올바른 목적지가 있는가?
                                    ),
                                    new SequenceNode
                                    (
                                        new List<INode>()
                                        {
                                            new ActionNode(Test1), // 목적지에 도착헸는가?
                                            new ActionNode(Test2) // 목적지에 도착했는가?
                                        }
                                    ),
                                    new ActionNode(Test1) // 이동
                                }
                            )
                        }
                    )
                ),
                new SequenceNode    // ## 공격실행 판정 분기 노드
                (
                    new List<INode>()
                    {
                        new ActionNode(Test1),
                        new ActionNode(Test2),
                        new ActionNode(Test3)
                    }
                ),
                new ActionNode(Test1) // 추적
            }
        );
    }
    
    #endregion
    
    

    # region Action(Leaf) Nodes

    #region Detect Player Node

    // 범위안에 적이 있는가? 
    // 범위 거리 체크할 Distance,범위 안에 있다면 플레이어의 위치 정보를 가져올 Transform
    // 이 쪽에서 플레이어가 존재시에 Transform에 플레이어 정보를 저장한다.
    // 만약 Transform != null 이라면 할당하지 않고, null 일때에만 할당한다.
    // 플레이어가 감지 범위 밖으로 나간다면 Transform을 null으로 만들어준다.
    // 이런식으로 최대한 Physics2D.OverlapCircleAll 를 적게 사용해야 불필요한 연산을 줄일 수 있다.

    #endregion

    #region Correct Destination Check & Patrol/Idle Node

    // 올바른 목적지 체크
    // 목적지 도착했는가?
    // 대기시간이 남아있는가?
    // 이동로직

    #endregion

    #region Attack Check/Excute Node

    // 공격중인가?
    // 공격범위 안에 적이 있는가?
    // 공격 실행

    #endregion

    #region Tracking Node

    // 추적 로직

    #endregion

    private INode.E_NodeState Test1()
    {
        return INode.E_NodeState.ENS_Running;
    }
    
    private INode.E_NodeState Test2()
    {
        return INode.E_NodeState.ENS_Running;
    }
    
    private INode.E_NodeState Test3()
    {
        return INode.E_NodeState.ENS_Running;
    }
    #endregion



    #region Action Logics

    private void TemporaryMethod()
    {
        
    }

    #endregion
    
    
    
    #region Initializer

    private void InitializeData()
    {
        _enemyData = DataContext.CreatDataContext(this.gameObject);
        _btRunner = new BehaviorTreeRunner(SettingBT());
    }

    #endregion
}
