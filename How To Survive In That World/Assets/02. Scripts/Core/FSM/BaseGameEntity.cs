using UnityEngine;


// 에이전트가 상속받는 기반 클래스, ex) 모든 HGZ 거주자들이 상속받는다.
public abstract class BaseGameEntity : MonoBehaviour
{
    #region Field

    // 정적 변수
    private static int m_iNextValidID;
    private string _entityName; // 에이전트 이름
    
    // BaseGameEntity를 상속받는 모든 게임오브젝트는 ID 번호를 부여받고,
    // 이 번호는 0부터 시작해서 1씩 증가
    private int _id;

    #endregion

    #region Properties

    protected int ID
    {
        private set
        {
            _id = value;
            m_iNextValidID++;
        }
        get => _id;
    }

    #endregion
    
    public virtual void Setup(string name)
    {
        // 고유 번호 설정
        ID = m_iNextValidID;
        // 이름 설정
        _entityName = name;
    }
    
    // GameController 클래스에서 모든 에이전트의 Updated()를 호출해 에이전트를 구동한다.
    public abstract void Updated();
}
