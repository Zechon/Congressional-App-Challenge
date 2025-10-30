using System;
using System.Collections.Generic;

public static class GameData
{
    public static List<StaffData> HiredStaff = new List<StaffData>();
    public static string Party;
    public static int RunSeed;
    public static int Money = 40000;
    public static float PR = 0.0f; // starting PR (0-1 scale)
    public static int Votes = 0;
}
