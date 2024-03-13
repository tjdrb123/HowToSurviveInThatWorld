using UnityEngine;
using UnityEngine.UI;

public class JoystickButton : MonoBehaviour
{
    public Button fixedButton;
    public Button floatingButton;

    public InputUIJoystick joystick;
    
    // Start is called before the first frame update
    void Start()
    {
        fixedButton.onClick.AddListener(() =>
            joystick.SetJoystickMode(InputUIJoystick.E_JoystickType.Fixed));
        
        floatingButton.onClick.AddListener(() =>
            joystick.SetJoystickMode(InputUIJoystick.E_JoystickType.Floating));
    }
}
