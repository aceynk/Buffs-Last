using GenericModConfigMenu;
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
	public static ModConfig Config;

	public override void Entry(IModHelper helper)
	{
		_log = Monitor;
		Helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
		Helper.Events.GameLoop.GameLaunched += InitConfig;

		Config = Helper.ReadConfig<ModConfig>();
	}

	private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
	{
		Dictionary<string, BuffData> buffs = Game1.content.Load<Dictionary<string, BuffData>>("Data/Buffs");
		Dictionary<string, bool> persistentBuffs = Game1.content.Load<Dictionary<string, bool>>("aceynk.PersistentBuffs/PersistentBuffIds");
		
		string ExcludedBuffs = "(dwarfStatue|statue_of_blessings)";
		
		foreach (string key in buffs.Select(v => v.Key))
		{
			bool IsIncludedBuff = !Config.ExcludeEndless
			                      || !System.Text.RegularExpressions.Regex.IsMatch(key, ExcludedBuffs);
			persistentBuffs[key] = IsIncludedBuff;
		}
		
		persistentBuffs["food"] = true;
		
		Log("Successfully processed all Buffs.");
	}
	
	private void InitConfig(object? sender, GameLaunchedEventArgs e)
	{
		var menu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

		if (menu is null)
		{
			return;
		}
        
		menu.Register(
			mod: ModManifest,
			reset: () => Config = new ModConfig(),
			save: () => Helper.WriteConfig(Config)
		);
        
		menu.AddSectionTitle(
			mod: ModManifest,
			text: () => Helper.Translation.Get("GMCM.MainTitle")	
		);
        
		menu.AddBoolOption(
			mod: ModManifest,
			name: () => Helper.Translation.Get("GMCM.ExcludeEndless.Name"),
			tooltip: () => Helper.Translation.Get("GMCM.ExcludeEndless.Desc"),
			getValue: () => Config.ExcludeEndless,
			setValue: v => Config.ExcludeEndless = v
		);
	}
}