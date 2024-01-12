// 노드의 통일성을 위한 Interface Node
// Evaluate : Interface의 Node의 상태와 Node가 어떤 상태인지를 반환하는 메소드
public interface INode
{
    public enum E_NodeState
    {
        ENS_Running,
        ENS_Success,
        ENS_Failure
    }

    public E_NodeState Evaluate();
}
