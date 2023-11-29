using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonInfo
{
    [Serializable]
    public enum BuffType
    {
        Damage,
        Health,
        MoveSpeed,
        AttackSpeed
    }
    [Serializable]
    public enum DemonID
    {
        Null,
        Base,
        LittleGuy,
        Chaos
    }
    public enum SpawnerType
    {
        Null,
        Basic,
        Special,
        Boss
    }
    public enum SpawnType
    {
        Null,
        Default,
        Ritual
    }
}

