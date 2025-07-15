[System.Serializable]
public class DataModeledNumericStat 
{
    public string originGUID;
    public string numericStatType;
    public string numericStatModificationType;
    public float value;

    public DataModeledNumericStat(string originGUID, string numericStatType, string numericStatModificationType, float value)
    {
        this.originGUID = originGUID;
        this.numericStatType = numericStatType;
        this.numericStatModificationType = numericStatModificationType;
        this.value = value;
    }
}
