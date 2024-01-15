// Behavior Tree에 최상위에 위치한 root노드를 실행해주는 Operate 메소드
// 단지 처음 시작을 해주는 역할.
public class BehaviorTreeRunner
{
    private INode _rootNode;

    public BehaviorTreeRunner(INode rootNode)
    {
        _rootNode = rootNode;
    }

    public void Operate()
    {
        _rootNode.Evaluate();
    }
}
