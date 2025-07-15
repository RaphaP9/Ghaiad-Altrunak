public class HealData
{
    public int healAmount;
    public IHealSource healSource;

    public HealData(int healAmount, IHealSource healSource)
    {
        this.healAmount = healAmount;
        this.healSource = healSource;
    }
}
