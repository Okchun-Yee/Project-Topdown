interface IWeapon
{
     /// <summary>이 무기의 쿨다운(공격 간격)</summary>
    public float Cooldown { get; }
    public void Attack();
}