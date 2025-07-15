public class ShieldData
{
    public int shieldAmount;
    public IShieldSource shieldSource;

    public ShieldData(int shieldAmount, IShieldSource shieldSource)
    {
        this.shieldAmount = shieldAmount;
        this.shieldSource = shieldSource;
    }
}
