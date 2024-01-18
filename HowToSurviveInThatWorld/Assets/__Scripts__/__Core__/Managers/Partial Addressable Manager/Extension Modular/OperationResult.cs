
using UnityEngine.ResourceManagement.AsyncOperations;

public readonly struct OperationResult<T>
{
    #region Fields

    public readonly bool IsSucceeded;
    public readonly object Key;
    public readonly T Value;

    #endregion



    #region Constructor
    
    /*
     *   # 비동기 작업 완료 현황을 위한 구조체
     *     - 기본적인 생성자 (오퍼레이션 상태, 해당 키, 밸류)
     */

    public OperationResult(bool succeeded, object key, T value)
    {
        IsSucceeded = succeeded;
        Key = key;
        Value = value;
    }
    
    /// <param name="value"> in 한정자 사용, 매개변수가 참조로 전달 및 값 수정 불가</param>
    public OperationResult(bool succeeded, object key, in T value)
    {
        IsSucceeded = succeeded;
        Key = key;
        Value = value;
    }

    /// <summary>
    /// # 키가 필요하지 않은 생성자
    ///   - 비동기 작업을 하는 `operation` Handle만이 주어진다.
    ///   - 오퍼레이션 상태를 그대로 저장하고, Result를 Value로 지님.
    /// </summary>
    /// <param name="handle"></param>
    public OperationResult(in AsyncOperationHandle<T> handle) : this()
    {
        IsSucceeded = handle.Status == AsyncOperationStatus.Succeeded;
        Value = handle.Result;
    }

    public OperationResult(object key, in AsyncOperationHandle<T> handle)
    {
        IsSucceeded = handle.Status == AsyncOperationStatus.Succeeded;
        Key = key;
        Value = handle.Result;
    }

    #endregion



    #region Utils

    /// <summary>
    /// # 생성자 안에 있는 내용을 내뱉기 위한 DeConstructor
    ///   - out으로 해당 오퍼레이션 결과를 추출 할 수 있다.
    /// </summary>
    public void Deconstruct(out bool succeeded, out T value)
    {
        succeeded = IsSucceeded;
        value = Value;
    }

    public void Deconstruct(out bool succeeded, out object key, out T value)
    {
        succeeded = IsSucceeded;
        key = Key;
        value = Value;
    }

    /// <summary>
    /// # 암시적 형변환 연산자를 정의.
    ///   - 한 유형 객체를 다른 유형으로 자동으로 변환할 수 있음.
    ///   - 이로 인해 명시적 형변환이 없더라도 컴파일러가 자동 형변환
    ///   - ex) OperationResult<int> result = new OperationResult<int>(true, null, 100);
    ///   -     int value = result;
    ///   -     implicit 연산자가 작동해 result.Value가 반환된다.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator T(in OperationResult<T> result)
        => result.Value;

    #endregion
}