public interface ISkill
{
    public void Initialize(SkillInfo info);
    public void ActivateSkill();
    SkillInfo SkillInfo { get; } //스킬 정보 접근용 프로퍼티
}
