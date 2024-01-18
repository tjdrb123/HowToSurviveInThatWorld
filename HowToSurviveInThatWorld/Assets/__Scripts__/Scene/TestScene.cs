
using UnityEngine;

public class TestScene : SceneBase
{
    protected override string AddressableLabel => "TestScene";

    protected override bool Initialize()
    {
        if (!base.Initialize())
        {
            DebugLogger.LogError("BaseScene Initialize Failed.");
            return false;
        }

        Instantiate(Managers.Addressable.GetAsset<GameObject>("air_A"));

        return true;
    }
}
