
public static class DataModelsUtils
{
    public static void SaveAllData(IDataModel[] dataModels)
    {
        foreach (IDataModel dataModel in dataModels)
        {
            dataModel.SaveData();
        }
    }

    public static void SaveData(IDataModel model)
    {
        model.SaveData();
    }
}