public class SelfExecuteDamageData
{
    private const int EXECUTE_DAMAGE = 999;

    public bool isCrit;
    public bool triggerHealthTakeDamageEvents;

    public int executeDamage;

    public SelfExecuteDamageData(bool isCrit, bool triggerHealthTakeDamageEvents)
    {
        this.isCrit = isCrit;
        this.triggerHealthTakeDamageEvents = triggerHealthTakeDamageEvents;
        executeDamage = EXECUTE_DAMAGE;
    }
}
