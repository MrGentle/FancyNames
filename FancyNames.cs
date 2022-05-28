using BepInEx;
using TMPro;
using UnityEngine;
using HarmonyLib;
using BepInEx.Logging;
using LLBML;
using Multiplayer;
using ModMenu;
using System.Collections.Generic;
using BepInEx.Configuration;
using System;
using System.IO;

namespace FancyNames
{
    [BepInPlugin("no.gentle.plugin.fancynames", "FancyNames", "0.0.2")]
    [BepInDependency(LLBML.PluginInfos.PLUGIN_ID)]
    [BepInDependency(ModMenu.PluginInfos.PLUGIN_ID, BepInDependency.DependencyFlags.SoftDependency)]
    public class FancyNames : BaseUnityPlugin
    {
        internal static FancyNames Instance { get; private set; } = null;
        internal static ManualLogSource Log { get; private set; } = null;
        internal static ConfigFile Conf { get; private set; }
        internal static DirectoryInfo ModFolder { get; private set; }
        public static Harmony harmony = new Harmony(PluginInfo.PLUGIN_NAME);

        private bool configInit = false;

        private void Awake()
        {
            Instance = this;
            Log = Logger;
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            ModFolder = LLBML.Utils.ModdingFolder.GetModSubFolder(Info);
            LLBML.Utils.ModDependenciesUtils.RegisterToModMenu(Info, new List<string>{
                "FancyNames lets your change your ingame name to whatever you want to without having to change your steam nickname",
            });
            harmony.PatchAll(typeof(Patches.PlayersSelection_Patches));
            harmony.PatchAll(typeof(Patches.ScreenMenu_Patches));
            harmony.PatchAll(typeof(Patches.PostSceenPlayerBar_Patches));
            harmony.PatchAll(typeof(Patches.PlatformBase_Patches));
        }

        private void FixedUpdate() {
            if (!configInit) {
                try {
                    if (LLBML.NetworkApi.GetCurrentPlatform()?.FGGNGJIOAKD() != null) {
                        ConfigHandler.InitConfig(Config);
                        configInit = true;
                    }
                } catch (Exception ex) {}
			}
		}

        private void OnGUI() {
            //No longer needed
            //if (!ConfigHandler.nameIsValid) GUI.Box(new Rect(10, Screen.height - 40, 200, 30), "Your fancyname is not valid");
		}
    }
}
