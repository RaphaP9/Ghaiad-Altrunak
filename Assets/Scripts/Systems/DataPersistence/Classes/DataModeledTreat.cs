[System.Serializable]
public class DataModeledTreat
{
    public string assignedGUID;
    public int treatID;

    public DataModeledTreat(string assignedGUID, int treatID)
    {
        this.assignedGUID = assignedGUID;
        this.treatID = treatID;
    }
}
