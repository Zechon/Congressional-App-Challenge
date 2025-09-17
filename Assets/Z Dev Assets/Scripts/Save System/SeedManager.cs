using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public enum Difficulty { Easy = 1, Normal = 2, Hard = 3 }

public static class SeedManager
{
    public static int Seed { get; private set; }
    public static Difficulty CurrentDifficulty { get; private set; }

    private static System.Random masterRng;
    private static Dictionary<int, System.Random> subRngs = new Dictionary<int, System.Random>();

    public static void GenerateSeed(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        int randomPart = UnityEngine.Random.Range(100000, 999999);
        Seed = int.Parse(((int)difficulty).ToString() + randomPart.ToString());
        InitializeFromSeed(Seed);
    }

    public static void UseSeed(int seed)
    {
        Seed = seed;
        int diffDigit = int.Parse(seed.ToString()[0].ToString());

        CurrentDifficulty = diffDigit switch
        {
            1 => Difficulty.Easy,
            2 => Difficulty.Normal,
            3 => Difficulty.Hard,
            _ => Difficulty.Normal
        };

        InitializeFromSeed(seed);
    }

    public static void InitializeFromSeed(int seed)
    {
        masterRng = new System.Random(seed);
    }

    public static int NextInt(int min, int max) => masterRng.Next(min, max);

    public static float NextFloat() => (float)masterRng.NextDouble();

    public static System.Random GetSubRng(int offset)
    {
        return new System.Random(Seed + offset);
    }
    //  SubRNG 1 = State Generation
    //  SubRNG 2 = Candidate Generation
    //  SubRNG 3 = Candidate Clothing Gen
}
