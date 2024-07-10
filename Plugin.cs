using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace CGModifiers
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static AssetBundle assets;

        private void Awake()
        {
            assets = AssetBundle.LoadFromMemory(Properties.Resources.assets);

            new Harmony(PluginInfo.PLUGIN_GUID + ".harmony").PatchAll();
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }
}