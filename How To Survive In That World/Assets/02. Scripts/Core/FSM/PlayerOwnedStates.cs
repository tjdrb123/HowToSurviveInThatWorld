using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player가 가질 수 있는 상태를 정의하는 클래스 입니다.
// 현재 아래 상태는 예시입니다.
namespace  PlayerOwnedStates
{
    public class RestAndSleep : State<Player>
    {
        public override void Enter(Player entity)
        {
            // 현재 위치를 마을로 설정,
            entity.CurrentLocation = Locations.Viliage;
            
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
    
    // 다른 상태 추가
}
