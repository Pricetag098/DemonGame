using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance<T>
{
    void LoadData(T data);
    void SaveData(ref T data);
}
