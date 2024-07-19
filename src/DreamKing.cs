using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Modding;
using UnityEngine;
using System.Security.Cryptography;
using DreamKing.Consts;
using SFCore.Generics;

namespace DreamKing
{
    enum ShopItemStatsSpecialType
    {
        None,
        HeartPiece,
        Charm,
        SoulPiece,
        Relic1,
        Relic2,
        Relic3,
        Relic4,
        Notch,
        Map,
        SimpleKey,
        RancidEgg,
        REPAIR_GLASS_HP,
        RepairGlassGeo,
        RepairGlassAttack,
        SalubrasBlessing,
        MapPin,
        MapMarker
    }
    public class DreamKing : FullSettingsMod<DkSaveSettings, DkGlobalSettings>
    {
        internal static DreamKing Instance;

        public LanguageStrings LangStrings { get; private set; }
        public TextureStrings SpriteDict { get; private set; }
        public AudioStrings AudioDict { get; private set; }
        public SceneChanger SceneChanger { get; private set; }

        public static AudioClip GetAudio(string name) => Instance.AudioDict.Get(name);

        public static Sprite GetSprite(string name) => Instance.SpriteDict.Get(name);

        public override string GetVersion()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string ver = asm.GetName().Version.ToString();
            SHA1 sha1 = SHA1.Create();
            FileStream stream = File.OpenRead(asm.Location);
            byte[] hashBytes = sha1.ComputeHash(stream);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            stream.Close();
            sha1.Clear();
            return $"{ver}-{hash.Substring(0, 6)}";
        }

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                new ValueTuple<string, string>("White_Palace_18", "White Palace Fly"),
                new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw"),
                new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw (2)"),
                new ValueTuple<string, string>("White_Palace_18", "Soul Totem white_Infinte"),
                new ValueTuple<string, string>("White_Palace_18", "Area Title Controller"),
                new ValueTuple<string, string>("White_Palace_18", "glow response lore 1/Glow Response Object (11)"),
                new ValueTuple<string, string>("White_Palace_18", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_18", "Inspect Region"),
                new ValueTuple<string, string>("White_Palace_18", "_Managers/PlayMaker Unity 2D"),
                new ValueTuple<string, string>("White_Palace_18", "Music Region (1)"),
                new ValueTuple<string, string>("White_Palace_17", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
                new ValueTuple<string, string>("White_Palace_19", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_20", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_20", "Battle Scene"),
                new ValueTuple<string, string>("Abyss_05", "Dusk Knight/Idle Pt"),
                new ValueTuple<string, string>("Abyss_05", "Dusk Knight/Dream Enter 2"),
                new ValueTuple<string, string>("Abyss_05", "door_dreamReturn"),
                new ValueTuple<string, string>("Abyss_05", "door_dreamReturn_reality"),
                new ValueTuple<string, string>("White_Palace_03_hub", "door1"),
                new ValueTuple<string, string>("White_Palace_03_hub", "Dream Entry"),
                new ValueTuple<string, string>("White_Palace_03_hub", "doorWarp"),
                new ValueTuple<string, string>("White_Palace_03_hub", "dream_beam_animation"),
                new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
                new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette")
            };
        }

        public DreamKing() : base("Dream King")
        {
            Instance = this;

            LangStrings = new LanguageStrings();
            SpriteDict = new TextureStrings();

            #region Shop Item Achievements
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.BoughtShovel1Key, Instance.SpriteDict.Get(TextureStrings.AchievementBoughtShovel1Key), LanguageStrings.AchievementBoughtShovel1TitleKey, LanguageStrings.AchievementBoughtShovel1TextKey, false);
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.BoughtShovel2Key, Instance.SpriteDict.Get(TextureStrings.AchievementBoughtShovel2Key), LanguageStrings.AchievementBoughtShovel2TitleKey, LanguageStrings.AchievementBoughtShovel2TextKey, false);
            #endregion
            #region Location Achievements
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.EnterLostDreamsKey, Instance.SpriteDict.Get(TextureStrings.AchievementLostDreamsKey), LanguageStrings.AchievementEnterLostDreamsTitleKey, LanguageStrings.AchievementEnterLostDreamsTextKey, false);
            #endregion
            #region Boss Achievements
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.DefeatedPaleWyrmKey, Instance.SpriteDict.Get(TextureStrings.AchievementBossKey), LanguageStrings.AchievementDefeatedPaleWyrmTitleKey, LanguageStrings.AchievementDefeatedPaleWyrmTextKey, true);
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.DefeatedPaleNoskKey, Instance.SpriteDict.Get(TextureStrings.AchievementBossKey), LanguageStrings.AchievementDefeatedPaleNoskTitleKey, LanguageStrings.AchievementDefeatedPaleNoskTextKey, true);
            #endregion
            InitCallbacks();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");
            Instance = this;

            InitGlobalSettings();
            SceneChanger = new SceneChanger(preloadedObjects);
            AudioDict = new AudioStrings(SceneChanger);
            //UIManager.instance.RefreshAchievementsList();

            //GameManager.instance.StartCoroutine(DEBUG_Shade_Style());

            Log("Initialized");
        }

        private void InitGlobalSettings()
        {
            // Found in a project, might help saving, don't know, but who cares
            // Global Settings
        }

        private void InitSaveSettings(SaveGameData data)
        {
            // Found in a project, might help saving, don't know, but who cares
            // Save Settings

            // Inventory Stuff
            InitInventory();
        }

        private void InitCallbacks()
        {
            // Hooks
            ModHooks.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.SetPlayerBoolHook += OnSetPlayerBoolHook;
            ModHooks.GetPlayerIntHook += OnGetPlayerIntHook;
            ModHooks.SetPlayerIntHook += OnSetPlayerIntHook;
            ModHooks.AfterSavegameLoadHook += InitSaveSettings;
            ModHooks.ApplicationQuitHook += SaveDkGlobalSettings;
            ModHooks.LanguageGetHook += OnLanguageGetHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void InitInventory()
        {
            SFCore.ItemHelper.AddOneTwoItem(SpriteDict.Get(TextureStrings.InvShovel1Key), SpriteDict.Get(TextureStrings.InvShovel2Key),
                nameof(SaveSettings.SfGrenadeDreamKingBoughtShovel1), nameof(SaveSettings.SfGrenadeDreamKingBoughtShovel2),
                LanguageStrings.Shovel1NameKey, LanguageStrings.Shovel2NameKey,
                LanguageStrings.Shovel1DescKey, LanguageStrings.Shovel2DescKey);
        }

        private void OnSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            string scene = to.name;
            Log("Scene Changed to: " + scene);

            if (scene == "Room_mapper" && SaveSettings.SfGrenadeDreamKingStartQuest)
            {
                // Iselda Shop
                SceneChanger.CR_Change_Room_Mapper(to, this);
            }
            else if (scene == "Deepnest_East_12" && SaveSettings.SfGrenadeDreamKingShopShovel1)
            {
                // Room with dust entrance to Deepnest_East_Hornet
                SceneChanger.CR_Change_Deepnest_East_12(to);
            }
            else if (scene == "Deepnest_East_Hornet" && SaveSettings.SfGrenadeDreamKingShopShovel1)
            {
                // Hornet 2 Room, entrance to Room_Wyrm
                SceneChanger.CR_Change_Deepnest_East_Hornet(to);
            }
            else if (scene == "Room_Wyrm" && SaveSettings.SfGrenadeDreamKingShopShovel1)
            {
                // Cast-Off Shell
                SceneChanger.CR_Change_Room_Wyrm(to);
            }
            else if (scene == TransitionGateNames.Ww01)
            {
                GameManager.instance.AwardAchievement(AchievementStrings.EnterLostDreamsKey);
                SceneChanger.CR_Change_WW01(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.Ww02)
            {
                SceneChanger.CR_Change_WW02(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
        }

        private void SaveDkGlobalSettings()
        {
            SaveGlobalSettings();
        }

        #region Get/Set Hooks

        private bool HasSettingsValue<T>(string target)
        {
            var tmpField = ReflectionHelper.GetFieldInfo(typeof(DkSaveSettings), target);
            return tmpField != null && tmpField.FieldType == typeof(T);
        }
        private T GetSettingsValue<T>(string target)
        {
            return ReflectionHelper.GetField<DkSaveSettings, T>(target);
        }
        private void SetSettingsValue<T>(string target, T val)
        {
            ReflectionHelper.SetField<DkSaveSettings, T>(target, val);
        }

        private string OnLanguageGetHook(string key, string sheet, string orig)
        {
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return orig;
        }

        private bool OnGetPlayerBoolHook(string target, bool orig)
        {
            if (HasSettingsValue<bool>(target))
            {
                if (!SaveSettings.SfGrenadeDreamKingStartQuest)
                    SaveSettings.SfGrenadeDreamKingStartQuest = (PlayerData.instance.royalCharmState == 4);
                return GetSettingsValue<bool>(target);
            }
            return orig;
        }

        private bool OnSetPlayerBoolHook(string target, bool orig)
        {
            if (HasSettingsValue<bool>(target))
            {
                if (target == nameof(SaveSettings.SfGrenadeDreamKingShopShovel1) && orig)
                {
                    SaveSettings.SfGrenadeDreamKingShopShovel1 = true;
                    SaveSettings.SfGrenadeDreamKingShopShovel2 = true;
                    SaveSettings.SfGrenadeDreamKingBoughtShovel1 = true;
                    GameManager.instance.AwardAchievement(AchievementStrings.DefeatedPaleNoskKey);
                }
                else if (target == nameof(SaveSettings.SfGrenadeDreamKingShopShovel2) && orig)
                {
                    SaveSettings.SfGrenadeDreamKingShopShovel1 = true;
                    SaveSettings.SfGrenadeDreamKingShopShovel2 = true;
                    SaveSettings.SfGrenadeDreamKingBoughtShovel2 = true;
                    GameManager.instance.AwardAchievement(AchievementStrings.DefeatedPaleWyrmKey);
                }
                else
                {
                    SetSettingsValue(target, orig);
                }
            }
            return orig;
        }

        private int OnGetPlayerIntHook(string target, int orig)
        {
            if (HasSettingsValue<int>(target))
            {
                return GetSettingsValue<int>(target);
            }
            return orig;
        }

        private int OnSetPlayerIntHook(string target, int orig)
        {
            if (HasSettingsValue<int>(target))
            {
                SetSettingsValue(target, orig);
            }
            return orig;
        }

        #endregion Get/Set Hooks
    }
}
