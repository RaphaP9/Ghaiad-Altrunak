public class ExecuteDamageData
{
    private const int EXECUTE_DAMAGE = 999;

    public bool isCrit;
    public IDamageSource damageSource;
    public bool triggerHealthTakeDamageEvents;

    public int executeDamage;

    public ExecuteDamageData(bool isCrit, IDamageSource damageSource, bool triggerHealthTakeDamageEvents)
    {
        this.isCrit = isCrit;
        this.damageSource = damageSource;
        this.triggerHealthTakeDamageEvents = triggerHealthTakeDamageEvents;
        executeDamage = EXECUTE_DAMAGE;
    }
}
