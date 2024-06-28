using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Buffs;

namespace Buffs_Last;

public class ModEntry : Mod
{
    public static void Log(string v)
    {
        _log.Log(v, LogLevel.Debug);
    }
    
    public static IMonitor _log = null!;
    
    public override void Entry(IModHelper helper)
    {
        _log = Monitor;
        Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
    }

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        Dictionary<string, BuffData> buffs = Game1.content.Load<Dictionary<string, BuffData>>("Data/Buffs");
        Dictionary<string, bool> persistentBuffs = Game1.content.Load<Dictionary<string, bool>>("aceynk.PersistentBuffs/PersistentBuffIds");
        
        foreach (string key in buffs.Select(v => v.Key))
        {
            Log(key);
            persistentBuffs[key] = true;
        }
        
        persistentBuffs["food"] = true;
    }
}