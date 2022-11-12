using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

namespace BeehiveUtilities
{
    [BepInPlugin("mennowar.mods.beehiveutilities_revived", "Beehive Utilities Revived", "1.0.1")]
    [HarmonyPatch]
    class BeehiveUtilitiesPlugin : BaseUnityPlugin
    {
        private static ConfigEntry<bool> enableMod;
        private static ConfigEntry<bool> disableDistanceCheck;
        private static ConfigEntry<bool> removeBiomeCheck;
        private static ConfigEntry<bool> honeySpawnInFront;
        private static ConfigEntry<int> maxHoney;
        private static ConfigEntry<double> minsCreation;

        private static int creationTimeInSeconds = 0;

        private static bool writeDebugOutput = false;

        private static ManualLogSource log = null;

        public static void Debug(string value)
        {

            if (writeDebugOutput)
            {
                if (log == null)
                {
                    log = BepInEx.Logging.Logger.CreateLogSource("Beehive");
                }

                if (log != null)
                {
                    log.LogMessage(value);
                }
            }
        }

        void Awake()
        {
            enableMod = Config.Bind("1 - Global", "Enable Mod", true, "Enable or disable this mod");
            if (!enableMod.Value) return;

            writeDebugOutput = Config.Bind("1 - Global", "Enable Debug", false, "Enable or disable Debug Output").Value;
            Debug("Beehive Awake called");

            Debug("Beehive Utilities Config:");

            disableDistanceCheck = Config.Bind("2 - General", "Disable Proximity Check", true, "Disables the \"Bees need more space\" check");
            Debug($"Disable Proximity Check: {disableDistanceCheck.Value}");
            
            maxHoney = Config.Bind("2 - General", "Max Honey", 20, "The maximum amount of honey a beehive can generate (default is 10)");
            Debug($"Max Honey: {maxHoney.Value}");

            minsCreation = Config.Bind("2 - General", "Minutes Per Creation", 20.0, "The amount of minutes it takes to generate 1 piece of honey (default is 20)");
            Debug($"Minutes Per Creation: {minsCreation.Value}");
            
            removeBiomeCheck = Config.Bind("2 - General", "Remove Biome Check", true, "Allows beehives to work in any biome");
            Debug($"Remove Biome Check: {removeBiomeCheck.Value}");
            
            honeySpawnInFront = Config.Bind("2 - General", "Spawn Honey In Front", true, "Spawns the honey in front of the hive instead of on top of it. Here is a picture to show which way is the front of the hive, it's the side where the thatch overlaps from the sides on top. https://i.imgur.com/zK5FT47.png");
            Debug($"Spawn Honey In Front: {honeySpawnInFront.Value}");

            creationTimeInSeconds = (int)(minsCreation.Value * 60);

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        // private static string LocaliseString(string text) { return Localization.instance.Localize(text); }

        private static void SetBeeStats(Beehive __instance)
        {
            if (!enableMod.Value) 
                return;

            Debug("-------- Setting BeeHive Status ----------");
            bool active = !EnvMan.instance.IsNight();
            __instance.m_beeEffect.SetActive(active); // no effect at night
            Debug($"Bee-Effect isActive: {active}");

            __instance.m_secPerUnit = creationTimeInSeconds;
            __instance.m_maxHoney = maxHoney.Value;

            Debug($"Seconds per Unit: {__instance.m_secPerUnit}");
            Debug($"Max Honey: {__instance.m_maxHoney}");

            if (honeySpawnInFront.Value)
            {
                __instance.m_spawnPoint.localPosition = new Vector3(0.8f, 0f, 0f);
                Debug("Honey should spawn in front");
            }
            else
            {
                Debug("Honey spawns at default position");
            }
             
            if (disableDistanceCheck.Value)
            {
                __instance.m_maxCover = 1000f;
                Debug($"Max Distance: {__instance.m_maxCover}");
            }
            else
            {
                Debug("Proximity check disabled");
            }

            if (removeBiomeCheck.Value)
            {
                __instance.m_biome = Heightmap.Biome.AshLands | Heightmap.Biome.BiomesMax | Heightmap.Biome.BlackForest | Heightmap.Biome.DeepNorth | Heightmap.Biome.Meadows | Heightmap.Biome.Mistlands | Heightmap.Biome.Mountain | Heightmap.Biome.Plains | Heightmap.Biome.AshLands | Heightmap.Biome.Swamp;
                Debug($"Biome set to everywhere: {__instance.m_biome}");
            }
            else
            {
                Debug("Biome-Check disabled");
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Beehive), "UpdateBees")]
        public static void BeehiveUpdateBees_Patch(Beehive __instance)
        {
            SetBeeStats(__instance);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Beehive), "Awake")]
        public static void BeehiveAwake_Patch(Beehive __instance)
        {
            SetBeeStats(__instance);
        }
    }
}