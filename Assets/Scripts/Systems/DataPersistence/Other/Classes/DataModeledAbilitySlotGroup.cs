[System.Serializable]
public class DataModeledAbilitySlotGroup 
{
    public string abilitySlot;
    public int abilityID;

    public DataModeledAbilitySlotGroup(string abilitySlot, int abilityID)
    {
        this.abilitySlot = abilitySlot;
        this.abilityID = abilityID;
    }
}
