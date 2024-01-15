using System.Collections.Generic;
using UnityEngine;

#region GameField_Enum
public enum Locations
{
    Dungeon = 1,
    Village,
};
#endregion


// 모든 에이전트를 생성, 업데이트 하는 클래스
public class GameController : MonoBehaviour
{
    #region Field

    [SerializeField] private string[] arrayPlayers; // Player들의 이름 배열
    [SerializeField] private GameObject playerPreFab; // Player 타입의 프리팹
    
    // 재생 제어를 위한 모든 에이전트 리스트
    private List<BaseGameEntity> _entities;
    private static bool IsGameStop { get; set; } // 게임 일시정지 기능

    #endregion

    private void Awake()
    {
        _entities = new List<BaseGameEntity>();
        
        // Player 에이전트 생성
        CreateEntities<Player>(playerPreFab, arrayPlayers);
        
        // 그 외 에이전트 생성
    }
    
    private void CreateEntities<T>(GameObject prefab, string[] names) where T : BaseGameEntity
    {
        foreach (var name in names)
        {
            GameObject clone = Instantiate(prefab);
            T entity = clone.GetComponent<T>();
            entity.Setup(name);
            
            _entities.Add(entity);
        }
    }

    private void Update()
    {
        if (IsGameStop) return;
        
        // 모든 에이전트의 Updated()를 호출해 에이전트 구동
        foreach (var entity in _entities)
            entity.Updated();
    }
 
    public static void Stop(BaseGameEntity entity)
    {
        IsGameStop = true;
        
        // 플레이어가 죽으면 Stop
        // 이것을 사용할 때는, 각 에이전트의 상태 메서드 안에서 GameController.Stop(entity); 실행
        // 그 상태에서 return해준다.
    }
}
