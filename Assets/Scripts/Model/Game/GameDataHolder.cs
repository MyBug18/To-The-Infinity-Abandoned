using System;
using Newtonsoft.Json;

public static class GameDataHolder
{
    public static readonly Game game = new Game();
    public static readonly Random random = new Random();
    public static bool isPaused = true;
    public static int gameSpeed = 1; // smaller, faster.
    public static float interval => 0.15f * gameSpeed;

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
