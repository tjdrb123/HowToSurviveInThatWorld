
using UnityEngine;

public class TestScene : SceneBase
{
    protected override string AddressableLabel => "TestScene";
    [SerializeField] private GameObject _inventory;

    protected override bool Initialize()
    {
        if (!base.Initialize())
        {
            DebugLogger.LogError("BaseScene Initialize Failed.");
            return false;
        }
        Destroy(_inventory);
        Instantiate(Managers.Addressable.GetAsset<GameObject>("air_A"));

        return true;
    }
}
