using System;
using UnityEngine;

[Serializable]
public class PlayerAnimationData
{
    #region Fields

    [SerializeField] private string crouchParameterName = "Crouch";
    [SerializeField] private string deathParameterName = "Death";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string jumpParameterName = "Jump";
    [SerializeField] private string punchParameterName = "Punch";
    [SerializeField] private string recoilRifleParameterName = "Recoil_Rifle";
    [SerializeField] private string reloadRifleParameterName = "Reload_Rifle";
    [SerializeField] private string rollParameterName = "Roll";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string strafeLParameterName = "Strafe_L";
    [SerializeField] private string strafeRParameterName = "Strafe_R";
    [SerializeField] private string walkBParameterName = "Walk_B";
    [SerializeField] private string walkFParameterName = "Walk_F";

    #endregion

    #region Getter

    public int CrouchParameterHash { get; private set; }
    public int DeathParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int JumpParameterHash { get; private set; }
    public int PunchParameterHash { get; private set; }
    public int RecoilRifleParameterHash { get; private set; }
    public int ReloadRifleParameterHash { get; private set; }
    public int RollParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int StrafeLParameterHash { get; private set; }
    public int StrafeRParameterHash { get; private set; }
    public int WalkBParameterHash { get; private set; }
    public int WalkFParameterHash { get; private set; }

    #endregion

    #region Constructor

    public PlayerAnimationData()
    {
        Initialize();
    }

    #endregion
    
    public void Initialize()
    {
        CrouchParameterHash = Animator.StringToHash(crouchParameterName);
        DeathParameterHash = Animator.StringToHash(deathParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        JumpParameterHash = Animator.StringToHash(jumpParameterName);
        PunchParameterHash = Animator.StringToHash(punchParameterName);
        RecoilRifleParameterHash = Animator.StringToHash(recoilRifleParameterName);
        ReloadRifleParameterHash = Animator.StringToHash(reloadRifleParameterName);
        RollParameterHash = Animator.StringToHash(rollParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        StrafeLParameterHash = Animator.StringToHash(strafeLParameterName);
        StrafeRParameterHash = Animator.StringToHash(strafeRParameterName);
        WalkBParameterHash = Animator.StringToHash(walkBParameterName);
        WalkFParameterHash = Animator.StringToHash(walkFParameterName);
    }
}