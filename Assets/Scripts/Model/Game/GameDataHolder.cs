﻿using System;
using Newtonsoft.Json;

public static class GameDataHolder
{
    public static Game game = new Game();
    public static Random r = new Random();
    public static bool isPaused = true;
    public static float interval = 0.15f;

    public static void SaveGame()
    {
        string result = JsonConvert.SerializeObject(game, new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            Formatting = Formatting.Indented
        });
        System.IO.File.WriteAllText("game_save.txt", result);
    }
}