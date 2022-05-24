using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LLBML;
using UnityEngine;
using Multiplayer;

namespace FancyNames {
	public static class ConfigHandler {

		public static ConfigEntry<string> FancyName;
		public static bool nameIsValid = true;

		internal static void InitConfig(ConfigFile config) {
			var steamName = NetworkApi.GetCurrentPlatform().FGGNGJIOAKD();

			FancyName = config.Bind<string>("FancyNames", "FancyName", steamName, new ConfigDescription("Your ingame displayname"));
			FancyName.SettingChanged += SettingChanged;
		}

		public static void SettingChanged(object sender, EventArgs e) {
			if (sender.GetType() == typeof(ConfigEntry<string>)) {
				var str = (ConfigEntry<string>)sender;
				switch(str.Definition.Key) {
					case "FancyName": IOGKKINMEFB.CNDNNOJEMLJ(); break;
				}
			}
		}

	}
}
