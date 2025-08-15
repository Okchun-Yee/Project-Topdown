public interface ISkill
{
    public void Initialize(SkillInfo info);
    public void ActivateSkill(int index = -1);
    
    void SubscribeSkillEvents();
    void UnsubscribeSkillEvents();
    SkillInfo SkillInfo { get; } //스킬 정보 접근용 프로퍼티
}
