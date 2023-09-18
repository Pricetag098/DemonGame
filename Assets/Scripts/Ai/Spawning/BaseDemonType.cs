using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonInfo
{
    [System.Serializable]
    public class BaseDemonType
    {
        public DemonType Base = new DemonType();

        public BaseDemonType()
        {
            Base.Id = DemonID.Base;
            Base.SpawnType = SpawnType.Basic;
        }
    }
}