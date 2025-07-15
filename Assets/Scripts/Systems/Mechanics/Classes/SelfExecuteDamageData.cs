public class SelfExecuteDamageData
{
    private const int EXECUTE_DAMAGE = 999;

    public bool isCrit;
    public bool triggerHealthTakeDamageEvents;
    public bool triggerShieldTakeDamageEvents;

    public int executeDamage;

    public SelfExecuteDamageData(bool isCrit, bool triggerHealthTakeDamageEvents, bool triggerShieldTakeDamageEvents)
    {
        this.isCrit = isCrit;
        this.triggerHealthTakeDamageEvents = triggerHealthTakeDamageEvents;
        this.triggerShieldTakeDamageEvents = triggerShieldTakeDamageEvents;
        executeDamage = EXECUTE_DAMAGE;
    }
}
