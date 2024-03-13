public class PlayerData
{
    #region Fields
    
    public HealthPoint Hp { get; private set; }
    public ExperiencePoint Exp { get; private set; }
    public Hunger Hunger { get; private set; }
    public Thirst Thirst { get; private set; }
    public Damage Damage { get; private set; }
    public Defense Defense { get; private set; }
    public MoveSpeed MoveSpeed { get; private set; }
    public Level Level { get; private set; }
    public UserName UserName { get; private set; }
    
    #endregion

    #region Constructor

    public PlayerData(PlayerDataDto playerDataDto)
    {
        Initialize(playerDataDto);
    }

    #endregion
    
    // 로드된 데이터로 초기화
    private void Initialize(PlayerDataDto playerDataDto)
    {
        Hp = new HealthPoint(playerDataDto.hp.curValue, playerDataDto.hp.maxValue);
        Exp = new ExperiencePoint(playerDataDto.exp.curValue, playerDataDto.exp.maxValue);
        
        Hunger = new Hunger(playerDataDto.hunger.value);
        Thirst = new Thirst(playerDataDto.thirst.value);
        Damage = new Damage(playerDataDto.damage.value);
        Defense = new Defense(playerDataDto.defense.value);
        MoveSpeed = new MoveSpeed(playerDataDto.moveSpeed.value);
        Level = new Level(playerDataDto.level.value);
        UserName = new UserName(playerDataDto.userName.value);
    }//
}