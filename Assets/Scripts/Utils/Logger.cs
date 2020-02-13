using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Logs data to file
public static class Logger
{
    // Log
    public static List<string> log = new List<string>();

    // Bool
    public static bool shouldUpdateLog = false;

    // Data
    public static int seconds = 0;
    public static int minutes = 0;
    public static int hours = 0;
    public static string currentElapsedTime = "";
    public static float plantsTotal = 0f;
    public static float plantsAlive = 0f;
    public static float plantsPercentAlive = 0f;
    public static float plantsAveragePercentOfMaxAge = 0f;
    public static float plantsMature = 0f;
    public static float plantsPercentMature = 0f;
    public static float plantsAverageNumberOfMutations = 0f;
    public static float plantsPositiveThrive = 0f;
    public static float plantsPercentPositiveThrive = 0f;
    public static float plantsAverageThrive = 0f;
    public static float plantsCarryingSpawn = 0f;
    public static float plantsPercentCarryingSpawn = 0f;
    public static float plantsSelfPollinated = 0f;
    public static float plantsPercentSelfPollinated = 0f;
    public static float plantsReproductionOnCooldown = 0f;
    public static float plantsPercentReproductionOnCooldown = 0f;


    // Update log
    public static IEnumerator UpdateLog()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if(shouldUpdateLog == false && Lifeforms.plants.Count > 0)
            {
                shouldUpdateLog = true;
            }
            else if(shouldUpdateLog == true && Lifeforms.plants.Count <= 0)
            {
                shouldUpdateLog = false;
            }
            else if(shouldUpdateLog == true)
            {
                CollectDataForLog();
            }
        }
    }

    // Initialize log
    public static void InitializeLog()
    {
        log.Add(@"Elapsed Time,Plants Total,Plants Alive,Plants Alive Percent,Plants Avg Age Percent,Plants Mature,Plants Mature Percent,Plants Avg Mutations,Plants Pos Thrive,Plants Pos Thrive Percent,Plants Avg Thrive,Plants Carrying,Plants Carrying Percent,Plants Self-Pollinated,Plants Self-Pollinated Percent,Plants Repro on CD, Plants Repro on CD Percent");
    }

    // Reset data
    private static void ResetData()
    {
        seconds = 0;
        minutes = 0;
        hours = 0;
        currentElapsedTime = "";
        plantsTotal = 0;
        plantsAlive = 0;
        plantsPercentAlive = 0f;
        plantsAveragePercentOfMaxAge = 0f;
        plantsMature = 0;
        plantsPercentMature = 0f;
        plantsAverageNumberOfMutations = 0f;
        plantsPositiveThrive = 0;
        plantsPercentPositiveThrive = 0f;
        plantsAverageThrive = 0f;
        plantsCarryingSpawn = 0;
        plantsPercentCarryingSpawn = 0f;
        plantsSelfPollinated = 0f;
        plantsPercentSelfPollinated = 0f;
        plantsReproductionOnCooldown = 0;
        plantsPercentReproductionOnCooldown = 0f;
    }

    // Collect data for log
    private static void CollectDataForLog()
    {
        ResetData();
        seconds = Mathf.FloorToInt((Time.time - Lifeforms.timeLastRespawnedLifeforms) % 60);
        minutes = Mathf.FloorToInt((Time.time - Lifeforms.timeLastRespawnedLifeforms) / 60 % 60);
        hours = Mathf.FloorToInt((Time.time - Lifeforms.timeLastRespawnedLifeforms) / 3600);
        currentElapsedTime = $@"{hours}:{(minutes < 10 ? "0" + minutes.ToString() : minutes.ToString())}:{(seconds < 10 ? "0" + seconds.ToString() : seconds.ToString())}";
        plantsTotal = Lifeforms.plants.Count;
        foreach(KeyValuePair<int, Plant> plant in Lifeforms.plants)
        {
            if(plant.Value.isAlive == true)
            {
                plantsAlive++;
                plantsAveragePercentOfMaxAge += plant.Value.percentOfMaxAge;
                if(plant.Value.isMature == true)
                {
                    plantsMature++;
                    if(plant.Value.isCarryingSpawn == true)
                    {
                        plantsCarryingSpawn++;
                        if(plant.Value.mateType == Plant.MateTypeEnum.Self)
                        {
                            plantsSelfPollinated++;
                        }
                    }
                    else if(plant.Value.isCarryingSpawn == false && plant.Value.isReproductionOnCooldown == true)
                    {
                        plantsReproductionOnCooldown++;
                    }
                }
                plantsAverageNumberOfMutations += plant.Value.numberOfMutations;
                plantsAverageThrive += plant.Value.thrivingAmount;
                if(plant.Value.thrivingAmount > 0.0f)
                {
                    plantsPositiveThrive++;
                }
            }
        }
        plantsPercentAlive = plantsAlive / plantsTotal;
        plantsAveragePercentOfMaxAge = plantsAveragePercentOfMaxAge / plantsAlive;
        plantsPercentMature = plantsMature / plantsAlive;
        plantsAverageNumberOfMutations = plantsAverageNumberOfMutations / plantsAlive;
        plantsAverageThrive = plantsAverageThrive / plantsAlive;
        plantsPercentPositiveThrive = plantsPositiveThrive / plantsAlive;
        plantsPercentCarryingSpawn = plantsCarryingSpawn / plantsAlive;
        plantsPercentSelfPollinated = plantsSelfPollinated / plantsCarryingSpawn;
        plantsPercentReproductionOnCooldown = plantsReproductionOnCooldown / plantsAlive;
        WriteDataToLog();
    }

    // Capture data to log
    private static void WriteDataToLog()
    {
        log.Add($@"{currentElapsedTime},{plantsTotal},{plantsAlive},{plantsPercentAlive},{plantsAveragePercentOfMaxAge},{plantsMature},{plantsPercentMature},{plantsAverageNumberOfMutations},{plantsPositiveThrive},{plantsPercentPositiveThrive},{plantsAverageThrive},{plantsCarryingSpawn},{plantsPercentCarryingSpawn},{plantsSelfPollinated},{plantsPercentSelfPollinated},{plantsReproductionOnCooldown},{plantsPercentReproductionOnCooldown}");
    }

    // Write log to file
    public static void WriteLogToFile()
    {
        Debug.Log($@"Writing data log to file");
        System.IO.File.WriteAllLines("data.csv", log);
    }
}
