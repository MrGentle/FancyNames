using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LLBML;
using UnityEngine;
using Multiplayer;
using System.IO;

namespace FancyNames {
	public static class ConfigHandler {

		public static ConfigEntry<string> FancyName;
		//public static ConfigEntry<string> NiceImage;
		public static ConfigEntry<bool> NiceImage;
		static DirectoryInfo niceImageFolder;

		internal static void InitConfig(ConfigFile config) {
			var steamName = NetworkApi.GetCurrentPlatform().FGGNGJIOAKD();

			FancyName = config.Bind<string>("FancyNames", "FancyName", steamName, new ConfigDescription("Your ingame displayname"));
			FancyName.SettingChanged += SettingChanged;

			/*
			 * For the future when we have a file picker
			 * NiceImage = config.Bind<string>("FancyNames", "Import Niceimage", null, new ConfigDescription("Converts an image and sets it as your fancy name", null, "modmenu_filepicker"));
			 * NiceImage.SettingChanged += SettingChanged;
			*/

			NiceImage = config.Bind<bool>("FancyNames", "Import Niceimage", false, new ConfigDescription("Converts an image and sets it as your fancy name"));
			NiceImage.SettingChanged += SettingChanged;

			try {
				niceImageFolder = Directory.CreateDirectory(FancyNames.ModFolder.FullName + "/NiceImage");
			} catch (Exception ex) {
				//Error handling
            }
		}

		public static void SettingChanged(object sender, EventArgs e) {
			if (sender.GetType() == typeof(ConfigEntry<string>)) {
				var str = (ConfigEntry<string>)sender;
				switch(str.Definition.Key) {
					case "FancyName": IOGKKINMEFB.CNDNNOJEMLJ(); break;
				}
			}

			/*if (sender.GetType() == typeof(ConfigEntry<string>)) {
				ConfigEntry<string> img = (ConfigEntry<string>)sender;
				switch(img.Definition.Key) {
					case "Import Niceimage":
						string[] fileFormats = {"png", "jpg", "jpeg"};
						if (img != null && fileFormats.Contains(Path.GetExtension(img.Value))) {
							try {
								string output = PNGToNiceImage(img.Value);
								Debug.Log(output);
								FancyName.BoxedValue = img;
							} catch (Exception ex) {
								Debug.Log(ex);
							}
                        }
						break;
                }
            }*/

			if (sender.GetType() == typeof(ConfigEntry<bool>)) {
				ConfigEntry<bool> flag = (ConfigEntry<bool>)sender;
				switch(flag.Definition.Key) {
					case "Import Niceimage":
						string[] fileFormats = {"png", "jpg", "jpeg"};

						if (niceImageFolder != null) {
							FileInfo file = niceImageFolder.GetFiles().First();
							if (file != null) {
								try {
									string img = PNGToNiceImage(file.FullName);
									FancyName.BoxedValue = img;
								} catch (Exception ex) {
									Debug.Log(ex);
								}
                            }
                        }
						break;
				}
				NiceImage.BoxedValue = false;
            }
		}

		public static string PNGToNiceImage(string file) {
			try {
				byte[] bytes = File.ReadAllBytes(file);
				//byte[] bytes = File.ReadAllBytes(file.FullName);

				Texture2D img = new Texture2D(0, 0);
				img.LoadImage(bytes);
				img.Compress(false);

				Texture2D flipped = new Texture2D(img.width, img.height);
         
				int xN = img.width;
				int yN = img.height;
         
				for(int i=0; i < xN; i++){
					for(int j=0; j < yN; j++){
						flipped.SetPixel(xN-i-1, j, img.GetPixel(i, j));
					}
				}
			
				int h = flipped.height;
				int w = flipped.width;
				int sq = h * w;
				Debug.Log("sq = " + sq.ToString());


				int fillpct = 300;
				int n = fillpct / h;
				int emsp = 2;

				Color32[] pixels = flipped.GetPixels32();
				var reverse = pixels.Reverse();
				string output = $"<size={n}%><mspace={emsp}e>";


				int count = 0;
				string prevRGB = "";

				int emCount = 0;

				foreach(Color32 pixel in reverse) {
					string nice = "";
					string RGB = $"{pixel.r:X2}{pixel.g:X2}{pixel.b:X2}";
					bool hasOpacity = pixel.a == 255 ? false : true;
					string finalRGB = hasOpacity ? $"{RGB}{pixel.a:X2}" : RGB;

					if (finalRGB != prevRGB) {
						if (emCount > 10) nice += $"<space={emCount}e>";
						else for(var i = 0; i < emCount+1; i++) nice += " ";
						
						if (RGB == "FFFFFF") nice += "<mark>";
						else nice += $"<mark=#{finalRGB}>";
						emCount = 0;
					} else {
						emCount++;
						if (count % w == 0 && count != 0) {
							if (emCount > 10) nice += $"<space={emCount}e>";
							else for(var i = 0; i < emCount+1; i++) nice += " ";
							emCount = 0;
                        }
					}

					if (count % w == 0 && count != 0) {
						nice += "<mark=#00000000>\\t";
					}
					if (count % w == 0 && count != 0 && count != sq - 1) nice += "\\n";
			
					prevRGB = finalRGB;
					count++;
					output += nice;
				}
				Debug.Log((output.Length / 1000.0f).ToString() + " kb");
				return output;
			} catch (Exception ex) {
				Debug.Log("FAIL");
				Debug.Log(ex);
				return null;
            }
			
        }

	}
}
