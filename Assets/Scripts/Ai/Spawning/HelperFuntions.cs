using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFuntions
{
    public static List<T> ShuffleList<T>(List<T> list)
    {
        var newShuffledList = new List<T>();
        var listCount = list.Count;

        for (int i = 0; i < listCount; i++)
        {
            var randomElementInList = Random.Range(0, list.Count);
            newShuffledList.Add(list[randomElementInList]);
            list.Remove(list[randomElementInList]);
        }

        return newShuffledList;
    }
}
