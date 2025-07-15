[System.Serializable]
public class DataModeledAssetStat 
{
    public string originGUID;
    public string assetStatType;
    public string assetStatModificationType;
    public int assetID;

    public DataModeledAssetStat(string originGUID, string assetStatType, string assetStatModificationType, int assetID)
    {
        this.originGUID = originGUID;
        this.assetStatType = assetStatType;
        this.assetStatModificationType = assetStatModificationType;
        this.assetID = assetID;
    }
}
