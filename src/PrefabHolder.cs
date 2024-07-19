using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using SFCore.Utils;

namespace DreamKing
{
    class PrefabHolder
    {
        public static GameObject PopAreaTitleCtrlPrefab { get; private set; }
        public static GameObject PopSceneManagerPrefab { get; private set; }
        public static GameObject PopPmU2dPrefab { get; private set; }
        public static GameObject WhiteBenchPrefab { get; private set; }
        public static GameObject A05IdlePt { get; private set; }
        public static GameObject A05De2 { get; private set; }
        public static GameObject A05Ddr { get; private set; }
        public static GameObject A05Ddrr { get; private set; }
        public static GameObject Wp03Door { get; private set; }
        public static GameObject Wp03Dream { get; private set; }
        public static GameObject Wp03Warp { get; private set; }
        public static GameObject Wp03DreamBeamAnim { get; private set; }
        public static GameObject BreakableWallPrefab { get; private set; }

        public static void Preloaded(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            PopAreaTitleCtrlPrefab = Object.Instantiate(preloadedObjects["White_Palace_18"]["Area Title Controller"]);
            SetInactive(PopAreaTitleCtrlPrefab);
            PopSceneManagerPrefab = UObject.Instantiate(preloadedObjects["White_Palace_18"]["_SceneManager"]);
            {
                var sm = PopSceneManagerPrefab.GetComponent<SceneManager>();
                sm.SetAttr("musicTransitionTime", 3.0f);
            }
            SetInactive(PopSceneManagerPrefab);
            PopPmU2dPrefab = Object.Instantiate(preloadedObjects["White_Palace_18"]["_Managers/PlayMaker Unity 2D"]);
            SetInactive(PopPmU2dPrefab);
            WhiteBenchPrefab = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["WhiteBench"]);
            SetInactive(WhiteBenchPrefab);
            A05IdlePt = Object.Instantiate(preloadedObjects["Abyss_05"]["Dusk Knight/Idle Pt"]);
            SetInactive(A05IdlePt);
            A05De2 = Object.Instantiate(preloadedObjects["Abyss_05"]["Dusk Knight/Dream Enter 2"]);
            SetInactive(A05De2);
            A05Ddr = Object.Instantiate(preloadedObjects["Abyss_05"]["door_dreamReturn"]);
            SetInactive(A05Ddr);
            A05Ddrr = Object.Instantiate(preloadedObjects["Abyss_05"]["door_dreamReturn_reality"]);
            SetInactive(A05Ddrr);
            Wp03Door = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["door1"]);
            SetInactive(Wp03Door);
            Wp03Dream = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["Dream Entry"]);
            SetInactive(Wp03Dream);
            Wp03Warp = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["doorWarp"]);
            SetInactive(Wp03Warp);
            Wp03DreamBeamAnim = Object.Instantiate(preloadedObjects["White_Palace_03_hub"]["dream_beam_animation"]);
            SetInactive(Wp03DreamBeamAnim);
            BreakableWallPrefab = UObject.Instantiate(preloadedObjects["Crossroads_07"]["Breakable Wall_Silhouette"]);
            {
                UObject.Destroy(BreakableWallPrefab.GetComponent<PersistentBoolItem>());
            }
            SetInactive(BreakableWallPrefab);
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
