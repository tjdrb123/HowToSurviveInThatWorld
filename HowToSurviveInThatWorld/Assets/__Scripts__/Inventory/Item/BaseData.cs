using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBaseData", menuName = "ItemDataSO/ItemBase", order = 0)]
public class ItemDataSo : ScriptableObject
{
    public int KeyNumber; // 아이템 고유 번호
    public string Name; //아이템 이름 
    public string Description; // 아이템 정보
    public int PlusValue; // 아이템 사용 및 장착시 플러스 점수
    public Sprite ItemImage; //아이템이 쌓이는지 안쌓이는지 확인
    public bool ISStack; //쌓이는 값인지 아닌지 확인
}