using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSelector : MonoBehaviour
{
    public int index = 0;

    public TextMeshProUGUI text;

    //public List<int> options= new List<int>();

    [SerializeField] private List<RoundPoints> roundSettings = new List<RoundPoints>();

    private void Start()
    {
        GamePrefs.SetStartRound(1);
        GamePrefs.SetStartMoney(500);
        GamePrefs.SetAbilitiesUnlocked(false);
        GamePrefs.SetRitualsComplete(false);
        GamePrefs.SetRoundLimit(false);
    }

    public void ButtonClick(int value)
    {
        index += value;

        if (index > roundSettings.Count - 1) { index = 0; }
        else if (index < 0) { index = roundSettings.Count - 1; }

        text.text = roundSettings[index].round.ToString();

        GamePrefs.SetStartRound(roundSettings[index].round);
        GamePrefs.SetStartMoney(roundSettings[index].points);
    }

    public void CompleteAllRituals(bool toggle)
    {
        //Debug.Log("All rituals complete at start"  + toggle);

        GamePrefs.SetRitualsComplete(toggle);
    }

    public void UnlockAllAbilities(bool toggle)
    {
        //Debug.Log("All Abilities unlocked at start" + toggle);

        GamePrefs.SetAbilitiesUnlocked(toggle);
    }

    public void AddRoundLimit(bool toggle)
    {
        //Debug.Log("All Abilities unlocked at start" + toggle);

        GamePrefs.SetRoundLimit(toggle);
    }

    [System.Serializable]
    public class RoundPoints
    {
        public int round;
        public int points;
    }

}
