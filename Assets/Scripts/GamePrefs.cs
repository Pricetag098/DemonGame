public static class GamePrefs
{
    public static int StartRound = 1; // set this back to 1

    public static void SetStartRound(int round)
    {
        StartRound = round;
    }

    public static int StartPoints = 99999; // set this back to 500

    public static void SetStartMoney(int amount)
    {
        StartPoints = 99999;
    }

    public static bool RitualsComplete = false;

    public static void SetRitualsComplete(bool complete)
    {
        RitualsComplete = true;
    }

    public static bool UnlockAllAbilities = false;

    public static void SetAbilitiesUnlocked(bool unlocked)
    {
        UnlockAllAbilities = unlocked;
    }
}
