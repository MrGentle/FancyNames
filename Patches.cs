using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using LLHandlers;
using LLBML;
using LLBML.States;
using LLBML.Players;
using Multiplayer;
using LLScreen;
using System.Text.RegularExpressions;

namespace FancyNames {
    public static class Patches {

        public static class PlayersSelection_Patches {
            [HarmonyPatch(typeof(PlayersSelection), "SetPlayerName")]
            [HarmonyPostfix]

            static void SetPlayerNamePatch(ref PlayersSelection __instance) {
                __instance.btPlayerName.GetTextMesh().richText = true;
            }
        }

        public static class ScreenMenu_Patches {
            [HarmonyPatch(typeof(ScreenMenu), "UpdatePlayerInfo")]
            [HarmonyPostfix]

            static void SetPlayerNamePatch(ref ScreenMenu __instance) {
                __instance.btAccount.GetTextMesh().richText = true;
            }
        }

        public static class PostSceenPlayerBar_Patches {
            [HarmonyPatch(typeof(PostSceenPlayerBar), "SetPlayer")]
            [HarmonyPostfix]

            static void SetPlayerPatch(ref PostSceenPlayerBar __instance) {
                __instance.btPlayerName.GetTextMesh().richText = true;
                //__instance.btPlayerName.GetTextMesh().parseCtrlCharacters = true;
            }
        }

        public static class PlatformBase_Patches {
            [HarmonyPatch(typeof(KKMGLMJABKH), "FGGNGJIOAKD")]
            [HarmonyPostfix]

            static string GetNamePatch(string __result) {
                return ConfigHandler.FancyName?.Value != null ? ConfigHandler.FancyName.Value : __result;
                
                //Bruh biggest chunk of code in the mod goes into the bin
                /*if (ConfigHandler.FancyName?.Value != null) {
                    string name = ConfigHandler.FancyName.Value;

                    string[] parts = Regex.Split(name, @"<[^>]*>");
                    int finalNameLength = 0;

                    foreach(string part in parts) finalNameLength += part.Length;

                    if (finalNameLength > 1 && finalNameLength <= 32) {
                        ConfigHandler.nameIsValid = true;
                        return name;
                    }
                    else ConfigHandler.nameIsValid = false; 
				} 
                
                return __result;*/
            }
        }
    }
}
