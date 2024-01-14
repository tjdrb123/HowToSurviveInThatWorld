using System.Collections.Generic;
using UnityEngine;

#region GameField_Enum
public enum Locations
{
    Dungeon = 1,
    Viliage
};
#endregion


// 모든 에이전트를 생성, 업데이트 하는 클래스
public class GameController : MonoBehaviour
{
    #region Field

    [SerializeField] private string[] arrayPlayers; // Player들의 이름 배열
    [SerializeField] private GameObject _playerPreFab; // Player 타입의 프리팹
    
    [SerializeField] private string[] arrayEnemys; // Enemy들의 이름 배열
    [SerializeField] private GameObject _enemyPreFab; // Enemy 타입의 프리팹
    
    // 재생 제어를 위한 모든 에이전트 리스트
    private List<BaseGameEntity> _entitys;
    public static bool IsGameStop { get; set; } = false; // 게임 일시정지 기능

    #endregion

    private void Awake()
    {
        _entitys = new List<BaseGameEntity>();
        
        // Player 에이전트 생성
        for (int i = 0; i < arrayPlayers.Length; ++i)
        {
            // 에이전트 생성, 초기화 메서드 호출
            GameObject clone  = Instantiate(_playerPreFab);
            Player entity = clone.GetComponent<Player>();
            entity.Setup(arrayPlayers[i]);
            
            // 에이전트들의 재생 제어를 위한 리스트에 저장
            _entitys.Add(entity);
        }
        
        // Enemy 에이전트 생성
        for (int i = 0; i < arrayEnemys.Length; ++i)
        {
            GameObject clone  = Instantiate(_enemyPreFab);
            Enemy entity = clone.GetComponent<Enemy>();
            entity.Setup((arrayEnemys[i]));
            
            _entitys.Add(entity);
        }
    }

    private void Update()
    {
        if (IsGameStop == true) return;
        
        // 모든 에이전트의 Updated()를 호출해 에이전트 구동
        for(int i = 0; i<_entitys.Count; ++i)
            _entitys[i].Updated();
    }
 
    public static void Stop(BaseGameEntity entity)
    {
        IsGameStop = true;
        
        // entity.PrintText ("특정 조건을 완료하여 프로그램 종료");
        // 이것을 사용할 때는, 각 에이전트의 상태 메서드 안에서 GameController.Stop(entity); 실행
        // 그 상태에서 return해준다.
    }
}
