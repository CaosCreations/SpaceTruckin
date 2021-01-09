using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDataModelManager
{
    void SavePersistentDataForUpdatedDataModels();
    void SavePersistentDataForAllDataModels();
    void RegisterUpdatedPersistentData(object persistentData);
}
