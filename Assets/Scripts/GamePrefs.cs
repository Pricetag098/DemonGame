public static class GamePrefs
{
    public static int StartRound = 1; // set this back to 1

    public static void SetStartRound(int round)
    {
        StartRound = round;
    }

    public static int StartPoints = 500; // set this back to 500

    public static void SetStartMoney(int amount)
    {
        StartPoints = amount;
    }

    public static bool RitualsComplete = false;

    public static void SetRitualsComplete(bool complete)
    {
        RitualsComplete = complete;
    }

    public static bool roundLimit = false;

    public static void SetRoundLimit(bool hasRoundLimit)
    {
        roundLimit = hasRoundLimit;
    }

    public static bool UnlockAllAbilities = false;

    public static void SetAbilitiesUnlocked(bool unlocked)
    {
        UnlockAllAbilities = unlocked;
    }
}
