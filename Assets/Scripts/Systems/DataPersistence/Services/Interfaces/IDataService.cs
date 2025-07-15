using System.Threading.Tasks;

public interface IDataService
{
    bool SaveData<T>(T data);
    T LoadData<T>();

    Task<bool> SaveDataAsync<T>(T data);
    Task<T> LoadDataAsync<T>();
}
