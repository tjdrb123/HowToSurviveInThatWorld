[System.Serializable]
public class PlayerDataDto // Dto : data transfer object, 데이터 전송 객체 : 중개자 역할
{
    public StatusData hp;
    public StatusData exp;
    public SingleValueData hunger;
    public SingleValueData thirst;
    public SingleValueData damage;
    public SingleValueData defense;
    public SingleValueData moveSpeed;
    public SingleValueData level;
    public SingleStringValueData userName;
    
    [System.Serializable]
    public class StatusData
    {
        public float curValue;
        public float maxValue;
    }
    
    [System.Serializable]
    public class SingleValueData
    {
        public float value;
    }
    
    [System.Serializable]
    public class SingleStringValueData
    {
        public string value;
    }
}
