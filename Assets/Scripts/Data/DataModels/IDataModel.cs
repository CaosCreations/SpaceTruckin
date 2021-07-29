using System.Threading.Tasks;

public interface IDataModel
{ 
    void SaveData();
    Task LoadDataAsync();
}
