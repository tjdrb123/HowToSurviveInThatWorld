// Player가 가질 수 있는 상태를 정의하는 클래스 입니다.
// 현재, 아래 상태는 예시입니다.
namespace  PlayerOwnedStates
{
    public class RestAndSleep : State<Player>
    {
        public override void Enter(Player entity)
        {
            // 마을에서 실행되는 상태
            entity.CurrentLocation = Locations.Village;
            
            // 뭐 체력이 10으로 되는 예시이고, 전투모드라는 bool 값이 있다면 그것을 false로 해줘도 된다.
            entity.Health = 10;
        }

        public override void Execute(Player entity)
        {
            // 만약에 스태미너가 0이 아니면 줄어들고,
            // 스태미너가 0으로 되entity.changeState(상태) 코드를 통해서, 상태를 변경해준다.
        }

        public override void Exit(Player entity)
        {
            
        }
    }
    
    public class FightHard : State<Player>
    {
        public override void Enter(Player entity)
        {
            // 전투모드 시작?
        }

        public override void Execute(Player entity)
        {
            // 예를 들어서 피로가 20 아래라면, 경고 팝업
        }

        public override void Exit(Player entity)
        {
            // 전투 끝, 다시 파밍상태로 변경
        }
    }

    public class StateGlobal : State<Player>
    {
        // 플레이어의 현재 상태와 별개로, 모든 상태에서 지속적으로 업데이트 되어야 하는 상태입니다. ex) 스태미너 감소
        // 따라서 Enter() 와 Exit()는 없습니다.
        public override void Enter(Player entity)
        {
        }

        public override void Execute(Player entity)
        {
            //
        }

        public override void Exit(Player entity)
        {
        }
    }

    
    // 다른 상태 추가
}
