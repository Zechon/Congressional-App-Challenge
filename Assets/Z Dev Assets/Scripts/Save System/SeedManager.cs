using System;
using UnityEngine;

public enum Difficulty { Easy = 1, Normal = 2, Hard = 3 }

public static class SeedManager
{
    public static int Seed { get; private set; }
    public static Difficulty CurrentDifficulty { get; private set; }

    private static System.Random masterRng;

    public static void GenerateNewSeed(Difficulty diff)
    {
        int randomCore = UnityEngine.Random.Range(100000, 999999);
        Seed = ((int)diff * 1000000) + randomCore;
        Initialize(Seed);
    }

    public static void LoadSeed(int seed)
    {
        Seed = seed;
        int diffDigit = Mathf.FloorToInt(seed / 1000000f);
        CurrentDifficulty = (Difficulty)Mathf.Clamp(diffDigit, 1, 3);

        int randomCore = seed % 1000000;
        masterRng = new System.Random(randomCore);
    }

    private static void Initialize(int seed)
    {
        int diffDigit = Mathf.FloorToInt(seed / 1000000f);
        CurrentDifficulty = (Difficulty)Mathf.Clamp(diffDigit, 1, 3);

        int randomCore = seed % 1000000;
        masterRng = new System.Random(randomCore);
    }

    public static int NextInt(int min, int max) => masterRng.Next(min, max);
    public static float NextFloat() => (float)masterRng.NextDouble();
    public static System.Random GetSubRng(int offset) => new System.Random((Seed % 1000000) + offset);
}
