using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[Serializable]
public class GameData
{
    
}

public class GameManager : MonoBehaviour
{
    private GameData gameData;

    void Start()
    {
        // Load saved data on game start
        LoadGameData();
    }

    void Update()
    {
        // Example: Increase score when a certain event happens
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    gameData.playerScore += 10;
        //}

        // Example: Set game completion flag
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    gameData.isGameCompleted = true;
        //}

        // Save game data periodically or based on events
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    SaveGameData();
        //}



    }

    void SaveGameData()
    {
        // Save data using JSON serialization
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + "/gameData.json", json);
    }

    void LoadGameData()
    {
        // Load data using JSON deserialization
        string filePath = Application.persistentDataPath + "/gameData.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            // If the file doesn't exist, create a new GameData object
            gameData = new GameData();
        }

        // Apply loaded data to the game
        // For example, update UI with the loaded score
    }
}
