public interface IDataModelManager
{
    void SaveAllData();
    void SaveUpdatedData();
    void RegisterUpdatedData(IDataModel dataModel);
    void LoadData();
    void DeleteAllData();
    void DeleteData(IDataModel dataModel);
}
