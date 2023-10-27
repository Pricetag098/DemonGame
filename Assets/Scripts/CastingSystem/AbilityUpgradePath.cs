using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UpgradePaths/Ability")]

public class AbilityUpgradePath : ScriptableObject
{
    public List<Ability> abilities;
}
