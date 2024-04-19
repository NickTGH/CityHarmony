using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameFileData data);
    void SaveData(ref GameFileData data);
}
