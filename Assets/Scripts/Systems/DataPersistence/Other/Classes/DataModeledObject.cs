[System.Serializable]
public class DataModeledObject
{
    public string assignedGUID;
    public int objectID;

    public DataModeledObject(string assignedGUID, int objectID)
    {
        this.assignedGUID = assignedGUID;
        this.objectID = objectID;
    }
}
