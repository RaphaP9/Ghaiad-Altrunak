[System.Serializable]
public class DataModeledAbilityLevelGroup 
{
    public int abilityID;
    public string abilityLevel;

    public DataModeledAbilityLevelGroup(int abilityID, string abilityLevel)
    {
        this.abilityID = abilityID;
        this.abilityLevel = abilityLevel;
    }
}