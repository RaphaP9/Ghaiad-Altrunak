public class DamageData
{
    public int damage;
    public bool isCrit;
    public IDamageSource damageSource;
    public bool canBeDodged;
    public bool canBeImmuned;

    public bool canBeInvulnerabled;
    public bool triggerInvulnerability;

    public DamageData(int damage, bool isCrit, IDamageSource damageSource, bool canBeDodged, bool canBeImmuned, bool canBeInvulnerabled, bool triggerInvulnerability)
    {
        this.damage = damage;
        this.isCrit = isCrit;
        this.damageSource = damageSource;
        this.canBeDodged = canBeDodged;
        this.canBeImmuned = canBeImmuned;
        this.canBeInvulnerabled = canBeInvulnerabled;
        this.triggerInvulnerability = triggerInvulnerability;
    }
}
