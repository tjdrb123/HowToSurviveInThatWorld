
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

[AddComponentMenu("Input/Input Button UI")]
public class InputButton_UI : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    #region Fields (OnScreenControl)

    [InputControl(layout = "Button")] [SerializeField] private string _ControlPath;
    
    /* Property */
    // - ControlPath를 재정의하는 프로퍼티 메서드
    // - 입력을 오버라이딩 할 인풋 액션을 지정
    protected override string controlPathInternal
    {
        get => _ControlPath;
        set => _ControlPath = value;
    }

    #endregion



    #region Interface Implements Methods

    /// <summary>
    /// # 버튼을 눌렀을 때 Input으로 값을 전달하는 메서드
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        SendValueToControl(Literals.ONE_F);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SendValueToControl(Literals.ZERO_F);
    }

    #endregion
    
}
