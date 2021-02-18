using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using Modding;
using ModCommon;
using ModCommon.Util;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Security.Cryptography;
using DreamKing.Consts;

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
        RepairGlassHP,
        RepairGlassGeo,
        RepairGlassAttack,
        SalubrasBlessing,
        MapPin,
        MapMarker
    }
    public class DreamKing : Mod<DKSaveSettings, DKGlobalSettings>
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

            SFCore.AchievementHelper.Initialize();
            #region Shop Item Achievements
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.BoughtShovel1_Key, DreamKing.Instance.SpriteDict.Get(TextureStrings.AchievementBoughtShovel1Key), LanguageStrings.Achievement_BoughtShovel1_Title_Key, LanguageStrings.Achievement_BoughtShovel1_Text_Key, false);
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.BoughtShovel2_Key, DreamKing.Instance.SpriteDict.Get(TextureStrings.AchievementBoughtShovel2Key), LanguageStrings.Achievement_BoughtShovel2_Title_Key, LanguageStrings.Achievement_BoughtShovel2_Text_Key, false);
            #endregion
            #region Location Achievements
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.EnterLostDreams_Key, DreamKing.Instance.SpriteDict.Get(TextureStrings.AchievementLostDreamsKey), LanguageStrings.Achievement_EnterLostDreams_Title_Key, LanguageStrings.Achievement_EnterLostDreams_Text_Key, false);
            #endregion
            #region Boss Achievements
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.DefeatedPaleWyrm_Key, DreamKing.Instance.SpriteDict.Get(TextureStrings.AchievementBossKey), LanguageStrings.Achievement_DefeatedPaleWyrm_Title_Key, LanguageStrings.Achievement_DefeatedPaleWyrm_Text_Key, true);
            SFCore.AchievementHelper.AddAchievement(AchievementStrings.DefeatedPaleNosk_Key, DreamKing.Instance.SpriteDict.Get(TextureStrings.AchievementBossKey), LanguageStrings.Achievement_DefeatedPaleNosk_Title_Key, LanguageStrings.Achievement_DefeatedPaleNosk_Text_Key, true);
            #endregion
            initCallbacks();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");
            Instance = this;

            initGlobalSettings();
            SceneChanger = new SceneChanger(preloadedObjects);
            AudioDict = new AudioStrings(SceneChanger);
            //UIManager.instance.RefreshAchievementsList();

            //GameManager.instance.StartCoroutine(DEBUG_Shade_Style());

            Log("Initialized");
        }

        private void initGlobalSettings()
        {
            // Found in a project, might help saving, don't know, but who cares
            // Global Settings
        }

        private void initSaveSettings(SaveGameData data)
        {
            // Found in a project, might help saving, don't know, but who cares
            // Save Settings
            // Start Mod Quest
            Settings.SfGrenadeDreamKingStartQuest = Settings.SfGrenadeDreamKingStartQuest;
            if (!Settings.SfGrenadeDreamKingStartQuest)
                Settings.SfGrenadeDreamKingStartQuest = (PlayerData.instance.royalCharmState == 4);
            // Shop Items
            Settings.SfGrenadeDreamKingShopShovel1 = Settings.SfGrenadeDreamKingShopShovel1;
            Settings.SfGrenadeDreamKingShopShovel2 = Settings.SfGrenadeDreamKingShopShovel2;
            Settings.SfGrenadeDreamKingBoughtShovel1 = Settings.SfGrenadeDreamKingBoughtShovel1;
            Settings.SfGrenadeDreamKingBoughtShovel2 = Settings.SfGrenadeDreamKingBoughtShovel2;
            // Mechanics
            Settings.SfGrenadeDreamKingOwnOutskirtsBlizzard = Settings.SfGrenadeDreamKingOwnOutskirtsBlizzard;
            // Bosses
            Settings.SfGrenadeDreamKingDefeatedPaleWyrm = Settings.SfGrenadeDreamKingDefeatedPaleWyrm;
            Settings.SfGrenadeDreamKingDefeatedPaleNosk = Settings.SfGrenadeDreamKingDefeatedPaleNosk;
            // Areas
            Settings.SfGrenadeDreamKingVisitedWhiteWastes = Settings.SfGrenadeDreamKingVisitedWhiteWastes;

            // Inventory Stuff
            initInventory();
        }

        private void initCallbacks()
        {
            // Hooks
            ModHooks.Instance.GetPlayerBoolHook += OnGetPlayerBoolHook;
            ModHooks.Instance.SetPlayerBoolHook += OnSetPlayerBoolHook;
            ModHooks.Instance.GetPlayerIntHook += OnGetPlayerIntHook;
            ModHooks.Instance.SetPlayerIntHook += OnSetPlayerIntHook;
            ModHooks.Instance.AfterSavegameLoadHook += initSaveSettings;
            ModHooks.Instance.ApplicationQuitHook += SaveDKGlobalSettings;
            ModHooks.Instance.LanguageGetHook += OnLanguageGetHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void initInventory()
        {
            SFCore.ItemHelper.AddOneTwoItem("SF DK Shovel",
                SpriteDict.Get(TextureStrings.InvShovel1Key), SpriteDict.Get(TextureStrings.InvShovel2Key),
                nameof(Settings.SfGrenadeDreamKingBoughtShovel1), nameof(Settings.SfGrenadeDreamKingBoughtShovel2),
                LanguageStrings.Shovel1Name_Key, LanguageStrings.Shovel2Name_Key,
                LanguageStrings.Shovel1Desc_Key, LanguageStrings.Shovel2Desc_Key);
        }

        private void OnSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            // Don't change scene content if quest isn't started
            if (!Settings.SfGrenadeDreamKingStartQuest)
                return;

            string scene = to.name;
            Log("Scene Changed to: " + scene);

            if (scene == "Room_mapper")
            {
                // Iselda Shop
                SceneChanger.CR_Change_Room_Mapper(to, this);
            }
            else if (scene == "Deepnest_East_12")
            {
                // Room with dust entrance to Deepnest_East_Hornet
                SceneChanger.CR_Change_Deepnest_East_12(to);
            }
            else if (scene == "Deepnest_East_Hornet")
            {
                // Hornet 2 Room, entrance to Room_Wyrm
                SceneChanger.CR_Change_Deepnest_East_Hornet(to);
            }
            else if (scene == "Room_Wyrm")
            {
                // Cast-Off Shell
                SceneChanger.CR_Change_Room_Wyrm(to);
            }
            else if (scene == TransitionGateNames.ww01)
            {
                GameManager.instance.AwardAchievement(AchievementStrings.EnterLostDreams_Key);
                SceneChanger.CR_Change_WW01(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
            else if (scene == TransitionGateNames.ww02)
            {
                SceneChanger.CR_Change_WW02(to);
                GameManager.instance.RefreshTilemapInfo(scene);
            }
        }

        private void SaveDKGlobalSettings()
        {
            SaveGlobalSettings();
        }

        #region Get/Set Hooks

        private string OnLanguageGetHook(string key, string sheet)
        {
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return Language.Language.GetInternal(key, sheet);
        }

        private bool OnGetPlayerBoolHook(string target)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                if (!Settings.SfGrenadeDreamKingStartQuest)
                    Settings.SfGrenadeDreamKingStartQuest = (PlayerData.instance.royalCharmState == 4);
                return (bool)tmpField.GetValue(Settings);
            }
            return PlayerData.instance.GetBoolInternal(target);
        }

        private void OnSetPlayerBoolHook(string target, bool val)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                if (target == nameof(Settings.SfGrenadeDreamKingShopShovel1) && val)
                {
                    Settings.SfGrenadeDreamKingShopShovel1 = true;
                    Settings.SfGrenadeDreamKingShopShovel2 = true;
                    Settings.SfGrenadeDreamKingBoughtShovel1 = true;
                }
                else if (target == nameof(Settings.SfGrenadeDreamKingShopShovel2) && val)
                {
                    Settings.SfGrenadeDreamKingShopShovel1 = true;
                    Settings.SfGrenadeDreamKingShopShovel2 = true;
                    Settings.SfGrenadeDreamKingBoughtShovel2 = true;
                }
                else
                {
                    tmpField.SetValue(Settings, val);
                }
                return;
            }
            PlayerData.instance.SetBoolInternal(target, val);
        }

        private int OnGetPlayerIntHook(string target)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                return (int)tmpField.GetValue(Settings);
            }
            return PlayerData.instance.GetIntInternal(target);
        }

        private void OnSetPlayerIntHook(string target, int val)
        {
            var tmpField = Settings.GetType().GetField(target);
            if (tmpField != null)
            {
                tmpField.SetValue(Settings, val);
            }
            else
            {
                PlayerData.instance.SetIntInternal(target, val);
            }
        }

        #endregion Get/Set Hooks
    }
}
