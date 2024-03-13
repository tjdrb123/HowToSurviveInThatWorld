
using System;
using UnityEngine;

/// <summary>
/// # Presenter Factory
///   - 프레젠터를 생성해주는 공장 클래스
///   - System.Type을 이용해 검증 및 생성 후 반환
/// </summary>
public static class PresenterFactory
{
    public static Presenter<Component, Component> CreatePresenter(PresenterConfigSO configSO)
    {
        if (configSO == null || configSO.Type == null)
        {
            throw new ArgumentNullException(nameof(configSO), nameof(configSO.Type));
        }

        // ConfigSO.Type이 Presenter를 상속 받았는지 확인
        if (!typeof(Presenter<Component, Component>).IsAssignableFrom(configSO.Type))
        {
            throw new ArgumentException($"Type {configSO.Type} does not inherit from Presenter.");
        }
        
        /* Construct */
        // Activator를 통해 인스턴스를 생성 (Reflection 기능 사용)
        // 생성자 파라미터로 configSO를 전달
        if (Activator.CreateInstance(configSO.Type, new object[] { configSO }) 
            is not Presenter<Component, Component> presenter)
        {
            throw new InvalidOperationException($"Failed to create instance of type {configSO.Type}.");
        }

        return presenter;
    }
}
