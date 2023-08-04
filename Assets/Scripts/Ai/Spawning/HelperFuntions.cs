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
    public static List<T> AddToList<T>(List<T> originlist, List<T> itemsToAdd)
    {
        List<T> list = new List<T>(originlist);

        foreach(T item in itemsToAdd)
        {
            list.Add(item);
        }

        return list;
    }

    public static bool TimerGreaterThan(float timer, float tick)
    {
        return timer > tick;
    }

    public static bool IntGreaterThanOrEqual(int int1, int int2)
    {
        return int1 >= int2;
    }
}