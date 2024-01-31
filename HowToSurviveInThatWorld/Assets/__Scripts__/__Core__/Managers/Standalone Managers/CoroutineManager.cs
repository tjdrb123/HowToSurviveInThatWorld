using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// # Coroutine Manager => 스탠드 얼론 (독자적으로 활동 가능한 싱글톤 매니저)
///   - 해당 매니저 사용 방법
///   - Utils -> Enums.cs -> public enum E_CoroutineKey
///   - 코루틴이 해당하는 키워드로 새로운 타입을 만들어 작성
///     - 예시) LoginGoogleAsync,
///   - 이 후 그 키값은 Managers.Coroutine.StartCrt(E_CoroutineKey.LoginGoogleAsync, LoginGoogleAsync)처럼 사용.
/// </summary>

public class CoroutineManager : SingletonPersist<CoroutineManager>
{
    #region Fields

    // Coroutine Info Class
    private class CoroutineInfo
    {
        public Coroutine Coroutine;
        public IEnumerator Enumerator;
    }

    // Dictionary Member (Key : EnumType)
    private readonly Dictionary<E_CoroutineKey, CoroutineInfo> _coroutines = new();
    
    // Exception Event
    public event Action OnCoroutineException;

    #endregion



    #region Managed Coroutine

    public Coroutine StartCrt(E_CoroutineKey coroutineKey, IEnumerator coroutine, float delaySeconds = Literals.ZERO_F, Action onComplete = null)
    {
        if (_coroutines.TryGetValue(coroutineKey, out CoroutineInfo existingCoroutine))
        {
            if (existingCoroutine.Enumerator != null)
            {
                // 실행 중인 코루틴일 경우, 실행중인 코루틴 반환
                return existingCoroutine.Coroutine;
            }
        }

        IEnumerator coroutineToStart = (delaySeconds > 0)
            ? DelayedStartCoroutine(coroutine, onComplete, delaySeconds, coroutineKey)
            : CoroutineWrapper(coroutine, onComplete, coroutineKey);
        Coroutine coroutineInstance = StartCoroutine(coroutineToStart);

        _coroutines[coroutineKey] = new CoroutineInfo
        {
            Coroutine = coroutineInstance,
            Enumerator = coroutine,
        };

        return coroutineInstance;
    }

    public void StopCrt(E_CoroutineKey coroutineKey)
    {
        if (!_coroutines.TryGetValue(coroutineKey, out CoroutineInfo runningCoroutine)) return;
        
        StopCoroutine(runningCoroutine.Coroutine);
        _coroutines.Remove(coroutineKey);
    }

    public void StopCrt(Coroutine coroutine)
    {
        if (coroutine == null) return;

        // 딕셔너리에서 해당 Coroutine 객체를 찾아 제거
        var keyToRemove = _coroutines.FirstOrDefault(kvp => kvp.Value.Coroutine == coroutine).Key;
        if (keyToRemove.Equals(default(E_CoroutineKey))) return;
        
        StopCoroutine(coroutine);
        _coroutines.Remove(keyToRemove);
    }

    #endregion
    
    
    
    #region Get Coroutine

    public List<E_CoroutineKey> GetRunningCoroutinesId()
    {
        return (from info in _coroutines 
            where info.Value.Enumerator != null select info.Key).ToList();
    }

    public Coroutine GetRunningCoroutine(E_CoroutineKey coroutineKey)
    {
        return _coroutines.TryGetValue(coroutineKey, out CoroutineInfo coroutineInfo) 
            ? coroutineInfo.Coroutine : null;
    }
    
    #endregion



    #region Coroutine Utility

    private IEnumerator DelayedStartCoroutine(IEnumerator coroutine, Action onComplete, float delaySeconds, E_CoroutineKey coroutineKey)
    {
        yield return new WaitForSeconds(delaySeconds);
        yield return StartCoroutine(CoroutineWrapper(coroutine, onComplete, coroutineKey));
    }

    private IEnumerator CoroutineWrapper(IEnumerator coroutine, Action onComplete, E_CoroutineKey coroutineKey)
    {
        yield return RunCoroutineWithExceptionHandler(coroutine);
        
        onComplete?.Invoke();
        
        // 코루틴 완료 후 해당 코루틴 정보를 업데이트
        _coroutines.Remove(coroutineKey);
    }


    private IEnumerator RunCoroutineWithExceptionHandler(IEnumerator coroutine)
    {
        while (true)
        {
            object current;
            try
            {
                if (!coroutine.MoveNext()) yield break;
                current = coroutine.Current;
            }
            catch (Exception exception)
            {
                DebugLogger.LogError($"Coroutine Exception: {exception}");
                OnCoroutineException?.Invoke();
                yield break;
            }

            yield return current;
        }
    }

    #endregion
}
