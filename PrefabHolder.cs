using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using GlobalEnums;
using HutongGames.PlayMaker;
using On;
using Logger = Modding.Logger;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Profiling;
using ModCommon.Util;
using HutongGames.PlayMaker.Actions;
using DreamKing.Consts;
using DreamKing.Utils;
using UnityEngine.UI;
using UObject = UnityEngine.Object;

namespace DreamKing
{
    class PrefabHolder
    {
        public static GameObject popAreaTitleCtrlPrefab { get; private set; }
        public static GameObject popSceneManagerPrefab { get; private set; }
        public static GameObject popPmU2dPrefab { get; private set; }
        public static GameObject whiteBenchPrefab { get; private set; }
        public static GameObject a05IdlePT { get; private set; }
        public static GameObject a05DE2 { get; private set; }
        public static GameObject a05DDR { get; private set; }
        public static GameObject a05DDRR { get; private set; }
        public static GameObject wp03Door { get; private set; }
        public static GameObject wp03Dream { get; private set; }
        public static GameObject wp03Warp { get; private set; }
        public static GameObject wp03DreamBeamAnim { get; private set; }
        public static GameObject breakableWallPrefab { get; private set; }

        public static void preloaded(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            popAreaTitleCtrlPrefab = GameObject.Instantiate(preloadedObjects["White_Palace_18"]["Area Title Controller"]);
            SetInactive(popAreaTitleCtrlPrefab);
            popSceneManagerPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["_SceneManager"]);
            {
                var sm = popSceneManagerPrefab.GetComponent<SceneManager>();
                sm.SetAttr("musicTransitionTime", 3.0f);
            }
            SetInactive(popSceneManagerPrefab);
            popPmU2dPrefab = GameObject.Instantiate(preloadedObjects["White_Palace_18"]["_Managers/PlayMaker Unity 2D"]);
            SetInactive(popPmU2dPrefab);
            whiteBenchPrefab = GameObject.Instantiate(preloadedObjects["White_Palace_03_hub"]["WhiteBench"]);
            SetInactive(whiteBenchPrefab);
            a05IdlePT = GameObject.Instantiate(preloadedObjects["Abyss_05"]["Dusk Knight/Idle Pt"]);
            SetInactive(a05IdlePT);
            a05DE2 = GameObject.Instantiate(preloadedObjects["Abyss_05"]["Dusk Knight/Dream Enter 2"]);
            SetInactive(a05DE2);
            a05DDR = GameObject.Instantiate(preloadedObjects["Abyss_05"]["door_dreamReturn"]);
            SetInactive(a05DDR);
            a05DDRR = GameObject.Instantiate(preloadedObjects["Abyss_05"]["door_dreamReturn_reality"]);
            SetInactive(a05DDRR);
            wp03Door = GameObject.Instantiate(preloadedObjects["White_Palace_03_hub"]["door1"]);
            SetInactive(wp03Door);
            wp03Dream = GameObject.Instantiate(preloadedObjects["White_Palace_03_hub"]["Dream Entry"]);
            SetInactive(wp03Dream);
            wp03Warp = GameObject.Instantiate(preloadedObjects["White_Palace_03_hub"]["doorWarp"]);
            SetInactive(wp03Warp);
            wp03DreamBeamAnim = GameObject.Instantiate(preloadedObjects["White_Palace_03_hub"]["dream_beam_animation"]);
            SetInactive(wp03DreamBeamAnim);
            breakableWallPrefab = UObject.Instantiate(preloadedObjects["Crossroads_07"]["Breakable Wall_Silhouette"]);
            {
                UObject.Destroy(breakableWallPrefab.GetComponent<PersistentBoolItem>());
            }
            SetInactive(breakableWallPrefab);
        }
        private static void SetInactive(GameObject go)
        {
            if (go != null)
            {
                UObject.DontDestroyOnLoad(go);
                go.SetActive(false);
            }
        }
        private static void SetInactive(UObject go)
        {
            if (go != null)
            {
                UObject.DontDestroyOnLoad(go);
            }
        }
    }
}
