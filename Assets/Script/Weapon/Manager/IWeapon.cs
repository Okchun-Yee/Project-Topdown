public interface IWeapon
{
    /// <summary>
    /// 무기 공격을 실행합니다.
    /// </summary>
    public void Attack();
    /// <summary>
    /// 무기에 적용된 스킬을 사용합니다.
    /// </summary>
    public void UseSkill(int index);

    // WeaponInfo 접근을 위한 프로퍼티 추가
    public WeaponInfo weaponInfo { get; }
}