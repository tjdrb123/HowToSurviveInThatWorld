using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickStrategyFactory
{
    public static JoystickStrategy CreateStrategy(
        InputUIJoystick.E_JoystickHandleType joystickHandleType,
        InputUIJoystick joystick)
    {
        return joystickHandleType switch
        {
            InputUIJoystick.E_JoystickHandleType.OnlyHandle => new Joystick_OnlyHandle(joystick),
            InputUIJoystick.E_JoystickHandleType.HandleAndBg => new Joystick_HandleAndBg(joystick),
            InputUIJoystick.E_JoystickHandleType.DirectionAndBg => new Joystick_DirectionAndBg(joystick),
            InputUIJoystick.E_JoystickHandleType.HandleAndBgDirection => new Joystick_All(joystick),
            _ => throw new ArgumentOutOfRangeException($"{joystickHandleType} invalid joystick handle type.")
        };
    }
}
