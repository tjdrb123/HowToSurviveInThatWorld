
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
