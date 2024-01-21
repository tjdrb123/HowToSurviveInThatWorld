using Newtonsoft.Json;

[System.Serializable]
public class PlayerData
{
    #region  Fields

    //Status
    
    //[JsonProperty("Hp")]
    public HealthPoint Hp { get; private set; }
    
    //[JsonProperty("Exp")]
    public ExperiencePoint Exp { get; private set; }
    
    
    // Singly Attribute
    //[JsonProperty("Hunger")]
    public Hunger Hunger { get; private set; }
    
    //[JsonProperty("Thirst")]
    public Thirst Thirst { get; private set; }
    
    //[JsonProperty("Damage")]
    public Damage Damage { get; private set; }
    
    //[JsonProperty("Defense")]
    public Defense Defense { get; private set; }
    
    //[JsonProperty("MoveSpeed")]
    public MoveSpeed MoveSpeed { get; private set; }
    
    //[JsonProperty("Level")]
    public Level Level { get; private set; }
    
    //[JsonProperty("UserName")]
    public UserName UserName { get; private set; }

    #endregion

    
    
    #region Constructor

    public PlayerData()
    {
        //Initialize();
    }

    public PlayerData(float curHp, float maxHp, float curExp, float maxExp, float hunger, float thirst, 
        float dmg, float def, float moveSpd, float level, string userName)
    {
        Initialize(curHp, maxHp, curExp, maxExp, hunger, thirst, dmg, def, moveSpd, level, userName);
    }

    #endregion



    #region Initialize

    private void Initialize()
    {
        
    }
    
    // 로드된 데이터로 초기화
    private void Initialize(float curHp, float maxHp, float curExp, float maxExp, float hunger, float thirst,
                            float damage, float defense, float moveSpeed, float level, string userName)
    {
        Hp = new HealthPoint(curHp, maxHp);
        Exp = new ExperiencePoint(curExp, maxExp);

        Hunger = new Hunger(hunger);
        Thirst = new Thirst(thirst);
        Damage = new Damage(damage);
        Defense = new Defense(defense);
        MoveSpeed = new MoveSpeed(moveSpeed);
        Level = new Level(level);
        UserName = new UserName(userName);
    }

    #endregion
}
