public interface IDataModel
{
    void SaveData();
    System.Threading.Tasks.Task LoadDataAsync();
}