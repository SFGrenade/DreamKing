using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DreamKing.Consts;
using UnityEngine;
using GlobalEnums;
using Logger = Modding.Logger;
using UnityEngine.SceneManagement;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;

namespace DreamKing
{

    enum EnviromentType
    {
        DUST = 0,
        GRASS,
        BONE,
        SPA,
        METAL,
        NOEFFECT,
        WET
    }

    public class SceneChanger : MonoBehaviour
    {
        private static bool _debug = true;
        private static string _abPath = "E:\\Documents\\Projects\\Unity Projects\\DreamKing Assets\\Assets\\AssetBundles\\";

        public AssetBundle AbOverallMat { get; private set; } = null;
        public AssetBundle AbWwScene { get; private set; } = null;
        public AssetBundle AbWwMat { get; private set; } = null;

        public SceneChanger(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.GameManager.RefreshTilemapInfo += OnGameManagerRefreshTilemapInfo;

            PrefabHolder.Preloaded(preloadedObjects);

            #region Load AssetBundles
            Log("Loading AssetBundles");
            Assembly asm = Assembly.GetExecutingAssembly();
            if (AbOverallMat == null)
            {
                if (!_debug)
                {
                    using (Stream s = asm.GetManifestResourceStream("DreamKing.Resources.overall_materials_ww"))
                    {
                        if (s != null)
                        {
                            AbOverallMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbOverallMat = AssetBundle.LoadFromFile(_abPath + "overall_materials_ww");
                }
            }
            if (AbWwScene == null)
            {
                if (!_debug)
                {
                    using (Stream s = asm.GetManifestResourceStream("DreamKing.Resources.white_wastes_scenes"))
                    {
                        if (s != null)
                        {
                            AbWwScene = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbWwScene = AssetBundle.LoadFromFile(_abPath + "white_wastes_scenes");
                }
            }
            if (AbWwMat == null)
            {
                if (!_debug)
                {
                    using (Stream s = asm.GetManifestResourceStream("DreamKing.Resources.white_wates_materials"))
                    {
                        if (s != null)
                        {
                            AbWwMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbWwMat = AssetBundle.LoadFromFile(_abPath + "white_wastes_materials");
                }
            }
            Log("Finished loading AssetBundles");
            #endregion
        }

        private void OnGameManagerRefreshTilemapInfo(On.GameManager.orig_RefreshTilemapInfo orig, GameManager self, string targetScene)
        {
            orig(self, targetScene);
            if (targetScene == TransitionGateNames.Ww01)
            {
                self.tilemap.width = 68;
                self.tilemap.height = 100;
                self.sceneWidth = 68;
                self.sceneHeight = 100;
            }
            else if (targetScene == TransitionGateNames.Ww02)
            {
                self.tilemap.width = 128;
                self.tilemap.height = 64;
                self.sceneWidth = 128;
                self.sceneHeight = 64;
            }
        }

        public void CR_Change_Room_Mapper(Scene scene, DreamKing dreamKing)
        {
            if (scene.name != "Room_mapper")
                return;

            // Find the shop and save an item for use later
            GameObject shopObj = GameObject.Find("Shop Menu");

            ShopMenuStock shop = shopObj.GetComponent<ShopMenuStock>();
            GameObject itemPrefab = Instantiate(shop.stock[0]);
            itemPrefab.SetActive(false);

            // List of items from the store
            List<GameObject> newStock = new List<GameObject>();

            GameObject newItemObj;
            ShopItemStats stats;
            #region Shop Item Shovel1
            // Create a new shop item for this item def
            newItemObj = Instantiate(itemPrefab);
            newItemObj.SetActive(false);

            // Apply all the stored values
            stats = newItemObj.GetComponent<ShopItemStats>();
            stats.playerDataBoolName = "SfGrenadeDreamKingShopShovel1";
            stats.nameConvo = Consts.LanguageStrings.Shovel1NameKey;
            stats.descConvo = Consts.LanguageStrings.Shovel1DescKey;
            stats.requiredPlayerDataBool = "";
            stats.removalPlayerDataBool = "";
            stats.dungDiscount = false;
            stats.notchCostBool = "";
            stats.cost = 10000;

            // Need to set all these to make sure the item doesn't break in one of various ways
            stats.priceConvo = string.Empty;
            stats.specialType = (int)ShopItemStatsSpecialType.Charm;
            stats.charmsRequired = 0;
            stats.relic = false;
            stats.relicNumber = 0;
            stats.relicPDInt = string.Empty;

            // Apply the sprite for the UI
            newItemObj.transform.Find("Item Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = dreamKing.SpriteDict.Get(TextureStrings.Shovel1Key);

            newStock.Add(newItemObj);
            #endregion
            #region Shop Item Shovel2
            // Create a new shop item for this item def
            newItemObj = Instantiate(itemPrefab);
            newItemObj.SetActive(false);

            // Apply all the stored values
            stats = newItemObj.GetComponent<ShopItemStats>();
            stats.playerDataBoolName = "SfGrenadeDreamKingShopShovel2";
            stats.nameConvo = Consts.LanguageStrings.Shovel2NameKey;
            stats.descConvo = Consts.LanguageStrings.Shovel2DescKey;
            stats.requiredPlayerDataBool = "";
            stats.removalPlayerDataBool = "";
            stats.dungDiscount = false;
            stats.notchCostBool = "";
            stats.cost = 10000;

            // Need to set all these to make sure the item doesn't break in one of various ways
            stats.priceConvo = string.Empty;
            stats.specialType = (int)ShopItemStatsSpecialType.Charm;
            stats.charmsRequired = 0;
            stats.relic = false;
            stats.relicNumber = 0;
            stats.relicPDInt = string.Empty;

            // Apply the sprite for the UI
            newItemObj.transform.Find("Item Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = dreamKing.SpriteDict.Get(TextureStrings.Shovel2Key);

            newStock.Add(newItemObj);
            #endregion

            foreach (GameObject stockItem in shop.stock)
            {
                if (!stockItem.GetComponent<ShopItemStats>().GetPlayerDataBoolName().StartsWith("SfGrenadeDreamKing"))
                {
                    newStock.Add(stockItem);
                }
            }

            // Save unchanged list for potential alt stock
            List<GameObject> altStock = new List<GameObject>();
            altStock.AddRange(newStock);
            foreach (GameObject item in shop.stockAlt)
            {
                if (!newStock.Contains(item))
                {
                    altStock.Add(item);
                }
            }

            shop.stock = newStock.ToArray();
            if (shop.stockAlt != null)
                shop.stockAlt = altStock.ToArray();
        }

        public void CR_Change_Deepnest_East_12(Scene scene)
        {
            if (scene.name != "Deepnest_East_12")
                return;

            Log("CR_Change_Deepnest_East_12()");

            if (DreamKing.Instance.SaveSettings.SfGrenadeDreamKingBoughtShovel1 || DreamKing.Instance.SaveSettings.SfGrenadeDreamKingBoughtShovel2)
            {
                GameObject blizzardWall = scene.Find("blizzard_wall");
                Destroy(blizzardWall.transform.Find("Block").gameObject);

                CreateBreakableWall(scene.name, "SF_DK_Breakable_Wall_DK", new Vector3(112.8521f, 12.94972f, 0.44f), Vector3.zero, Vector3.one, new Vector2(5.394047f, 6.216965f), nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOpenedAshGrave));
            }
            Log("CR_Change_Deepnest_East_12 Done");
        }

        public void CR_Change_Deepnest_East_Hornet(Scene scene)
        {
            if (scene.name != "Deepnest_East_Hornet")
                return;

            Log("CR_Change_Deepnest_East_Hornet()");
            //yield return null;

            MakeSceneManagerDeepnest03(scene.FindRoot("_SceneManager").GetComponent<SceneManager>());

            GameObject[] tmp = FindObjectsOfType<GameObject>();
            foreach (var go in tmp)
            {
                if (!go.activeInHierarchy)
                    continue;
                // Color Grader
                if (go.name == "GradeMarker")
                    go.SetActive(false);
            }

            PatchOutskirtsBlizzard(scene);

            Log("CR_Change_Deepnest_East_Hornet Done");
        }

        public void CR_Change_Room_Wyrm(Scene scene)
        {
            Log("CR_Change_Room_Wyrm()");
            //yield return null;

            CreateDreamGateway("Dream Enter", new Vector2(13.0f, 8.0f), new Vector2(5.25f, 5.25f), TransitionGateNames.Ww01, TransitionGateNames.RWyrm);
            // x=18.5
            GameObject ddr = Instantiate(PrefabHolder.A05Ddr);
            ddr.name = TransitionGateNames.RwWw01;
            ddr.SetActive(true);
            ddr.transform.position = new Vector3(18.5f, 6.87f, 0.2f);
            GameObject ddrr = Instantiate(PrefabHolder.A05Ddrr);
            ddrr.name = TransitionGateNames.RwWw012;
            ddrr.SetActive(true);
            ddrr.transform.position = new Vector3(18.5f, 6.87f, 0.2f);

            MakeSceneManagerDeepnest03(scene.FindRoot("_SceneManager").GetComponent<SceneManager>());

            GameObject[] tmp = scene.GetRootGameObjects();
            scene.FindRoot("Glow Response Object").SetActive(false);

            PatchOutskirtsBlizzard(scene);

            Log("CR_Change_Room_Wyrm Done");
        }

        public void CR_Change_WW01(Scene scene)
        {
            Log("CR_Change_WW01()");

            PatchMisc(scene);

            GameManager.instance.StartCoroutine(PatchCameraLockAreas(scene));
        }

        public void CR_Change_WW02(Scene scene)
        {
            Log("CR_Change_WW02()");

            PatchMisc(scene);

            GameManager.instance.StartCoroutine(PatchCameraLockAreas(scene));
        }

        private void PatchMisc(Scene scene)
        {
            Log("!Misc");
            #region Area Title Controller
            GameObject tmpPmu2D = Instantiate(PrefabHolder.PopPmU2dPrefab, scene.GetRootGameObjects()[6].transform);
            tmpPmu2D.SetActive(true);
            tmpPmu2D.name = "PlayMaker Unity 2D";
            if (scene.name == TransitionGateNames.Ww01)
            {
                GameObject atc = Instantiate(PrefabHolder.PopAreaTitleCtrlPrefab);
                atc.SetActive(true);
                atc.transform.localPosition = Vector3.zero;
                atc.transform.localEulerAngles = Vector3.zero;
                atc.transform.localScale = Vector3.one;

                PlayMakerFSM atcFsm = atc.GetComponent<PlayMakerFSM>();
                atcFsm.FsmVariables.GetFsmFloat("Unvisited Pause").Value = 3f;
                atcFsm.FsmVariables.GetFsmFloat("Visited Pause").Value = 3f;

                atcFsm.FsmVariables.GetFsmBool("Always Visited").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Display Right").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Only On Revisit").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Sub Area").Value = false;
                atcFsm.FsmVariables.GetFsmBool("Visited Area").Value = DreamKing.Instance.SaveSettings.SfGrenadeDreamKingVisitedWhiteWastes;
                atcFsm.FsmVariables.GetFsmBool("Wait for Trigger").Value = false;

                atcFsm.FsmVariables.GetFsmString("Area Event").Value = Consts.LanguageStrings.WwAreaTitleEvent;
                atcFsm.FsmVariables.GetFsmString("Visited Bool").Value = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingVisitedWhiteWastes);

                atcFsm.FsmVariables.GetFsmGameObject("Area Title").Value = GameObject.Find("Area Title");

                atcFsm.SendEvent("DISPLAY");

                atc.AddComponent<NonBouncer>();

                DreamKing.Instance.SaveSettings.SfGrenadeDreamKingVisitedWhiteWastes = true;
            }
            #endregion

            #region Scene Manager
            if (scene.name == TransitionGateNames.Ww01)
            {
                GameObject tmp = Instantiate(PrefabHolder.PopSceneManagerPrefab);
                tmp.name = "_SceneManager";
                tmp.SetActive(true);
                var sm = tmp.GetComponent<SceneManager>();
                sm.overrideParticlesWith = MapZone.DREAM_WORLD;
                sm.mapZone = MapZone.DREAM_WORLD;
                // Steal other data from level422 aka Godhome
                sm.saturation = 0.7f;
                sm.defaultColor = new Color(0.904412f, 0.904412f, 0.904412f, 1.0f);
                sm.redChannel = new AnimationCurve(new Keyframe[4] { new Keyframe(0.0f, 0.0f), new Keyframe(0.198206f, 0.169463f), new Keyframe(0.810443f, 0.863599f), new Keyframe(1.0f, 1.0f) });
                sm.greenChannel = new AnimationCurve(new Keyframe[4] { new Keyframe(0.0f, 0.0f), new Keyframe(0.173057f, 0.125936f), new Keyframe(0.511333f, 0.455806f), new Keyframe(1.0f, 1.0f) });
                sm.blueChannel = new AnimationCurve(new Keyframe[5] { new Keyframe(0.0f, 0.0f), new Keyframe(0.146939f, 0.142570f), new Keyframe(0.472280f, 0.441470f), new Keyframe(0.862940f, 0.794455f), new Keyframe(1.0f, 1.0f) });
                sm.defaultIntensity = 0.968f;
                sm.heroLightColor = new Color(0.904412f, 0.785627f, 0.658359f, 0.678f);
            }
            else if (scene.name == TransitionGateNames.Ww02)
            {
                GameObject tmp = Instantiate(PrefabHolder.PopSceneManagerPrefab);
                tmp.name = "_SceneManager";
                tmp.SetActive(true);
                var sm = tmp.GetComponent<SceneManager>();
                sm.overrideParticlesWith = MapZone.DREAM_WORLD;
                sm.mapZone = MapZone.DREAM_WORLD;
                // Steal other data from level422 aka Godhome
                sm.saturation = 0.7f;
                sm.defaultColor = new Color(0.904412f, 0.904412f, 0.904412f, 1.0f);
                sm.redChannel = new AnimationCurve(new Keyframe[4] { new Keyframe(0.0f, 0.0f), new Keyframe(0.198206f, 0.169463f), new Keyframe(0.810443f, 0.863599f), new Keyframe(1.0f, 1.0f) });
                sm.greenChannel = new AnimationCurve(new Keyframe[4] { new Keyframe(0.0f, 0.0f), new Keyframe(0.173057f, 0.125936f), new Keyframe(0.511333f, 0.455806f), new Keyframe(1.0f, 1.0f) });
                sm.blueChannel = new AnimationCurve(new Keyframe[5] { new Keyframe(0.0f, 0.0f), new Keyframe(0.146939f, 0.142570f), new Keyframe(0.472280f, 0.441470f), new Keyframe(0.862940f, 0.794455f), new Keyframe(1.0f, 1.0f) });
                sm.defaultIntensity = 0.968f;
                sm.heroLightColor = new Color(0.904412f, 0.785627f, 0.658359f, 0.678f);
            }
            #endregion
            Log("~Misc");
        }

        private IEnumerator PatchDamageResetColliders(Scene scene)
        {
            yield return null;
            Log("!Damage Colliders");

            GameObject parent = GameObject.Find("Damage Colliders");
            Transform ch;
            for (int a = 0; a < parent.transform.childCount; a++)
            {
                ch = parent.transform.GetChild(a);

                if ((ch != null) && ch.gameObject.activeInHierarchy)
                {
                    try
                    {
                        var dh = ch.gameObject.AddComponent<DamageHero>();
                        dh.damageDealt = 1;
                        dh.shadowDashHazard = false;
                        dh.resetOnEnable = false;
                        dh.hazardType = (int)HazardType.ACID;

                        string name = ch.gameObject.name.ToLower();
                        if (name.Contains("thorn"))
                        {
                            ch.gameObject.AddComponent<NonBouncer>();
                        }
                        else if (name.Contains("pit"))
                        {
                            //dh.hazardType = (int)HazardType.PIT;
                            ch.gameObject.AddComponent<NonBouncer>();
                        }
                        else if (name.Contains("spike"))
                        {
                        }
                        else if (name.Contains("saw"))
                        {
                        }
                    }
                    catch (Exception ex)
                    {
                        Log("PatchDamageResetColliders - " + ex);
                    }
                }
            }
            Log("~Damage Colliders");
        }

        private IEnumerator PatchCameraLockAreas(Scene scene)
        {
            yield return new WaitWhile(() => !GameObject.Find("_Camera Lock Zones"));
            Log("!Camera Lock Areas");

            GameObject areas = GameObject.Find("_Camera Lock Zones");
            if (areas == null)
            {
                yield break;
            }
            Transform tf;
            GameObject go;
            BoxCollider2D bc2d;
            CameraLockArea cla;
            for (int i = 0; i < areas.transform.childCount; i++)
            {
                tf = areas.transform.GetChild(i);
                go = tf.gameObject;
                cla = go.AddComponent<CameraLockArea>();
                cla = go.GetComponent<CameraLockArea>();
                bc2d = go.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>();
                cla.cameraXMin = bc2d.bounds.min.x + 14.6f;
                cla.cameraYMin = bc2d.bounds.min.y + 8.3f;
                cla.cameraXMax = bc2d.bounds.max.x - 14.6f;
                cla.cameraYMax = bc2d.bounds.max.y - 8.3f;
                cla.preventLookUp = go.name.Contains("nlu") || go.name.Contains("nldu");
                cla.preventLookDown = go.name.Contains("nld") || go.name.Contains("nlud");
                cla.maxPriority = false;
                //cla.respawnFacingRight = true;
            }

            Log("~Camera Lock Areas");
        }

        private void MakeSceneManagerDeepnest03(SceneManager sm)
        {
            sm.sceneType = SceneType.GAMEPLAY;
            sm.mapZone = MapZone.OUTSKIRTS; // MapZone.OUTSKIRTS
            sm.isWindy = false;
            sm.isTremorZone = false;
            sm.environmentType = (int)EnviromentType.DUST;
            sm.darknessLevel = 2; // -1
            sm.noLantern = false;
            sm.saturation = 0.62f; // 0.5f
            // Color Curves
            sm.redChannel.keys = new Keyframe[4]
            {
                new Keyframe(0.0f, 0.0f), // 0.0f, 0.0f
                new Keyframe(0.283927f, 0.207444f), // 0.255242f, 0.313168f
                new Keyframe(0.696142f, 0.748298f), // 0.712533f, 0.727153f
                new Keyframe(0.989648f, 0.987768f) // 0.989648f, 0.987768f
            };
            sm.greenChannel.keys = new Keyframe[5]
            {
                new Keyframe(0.0f, 0.0f), // 0.0f, 0.0f
                new Keyframe(0.231539f, 0.182780f), // 0.461354f, 0.578872f
                new Keyframe(0.508553f, 0.525523f), // 1.0f, 1.0f
                new Keyframe(0.825743f, 0.765417f), // -
                new Keyframe(1.0f, 1.0f) // -
            };
            sm.blueChannel.keys = new Keyframe[4]
            {
                new Keyframe(0.0f, 0.0f), // 0.0f, 0.0f
                new Keyframe(0.181679f, 0.278094f), // 0.255242f, 0.313168f
                new Keyframe(0.661387f, 0.574693f), // 0.712533f, 0.727153f
                new Keyframe(1.0f, 1.0f) // 0.989648f, 0.987768f
            };
            sm.defaultColor.r = 0.807843f; // 0.692582f
            sm.defaultColor.g = 0.921569f; // 0.770142f
            sm.defaultColor.b = 0.952941f; // 0.897059f
            sm.defaultColor.a = 1.0f; // 1.0f
            sm.defaultIntensity = 0.6f; // 0.8f
            sm.heroLightColor.r = 0.941176f; // 1.0f
            sm.heroLightColor.g = 0.996078f; // 1.0f
            sm.heroLightColor.b = 1.0f; // 1.0f
            sm.heroLightColor.a = 0.458824f; // 0.672f
            sm.noParticles = false; // false
        }

        private void PatchOutskirtsBlizzard(Scene scene)
        {
            if ((scene.name != "Deepnest_East_Hornet") && (scene.name != "Room_Wyrm"))
                return;
            Log("!Outskirts Blizzard");

            if (scene.name == "Deepnest_East_Hornet")
            {
                var apb = scene.FindRoot("Audio Player Blizzard");
                var apbFsm = apb.LocateMyFSM("Play");
                apbFsm.GetAction<PlayerDataBoolTest>("State 1", 0).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                apbFsm.SetState("Pause");

                var apbw = scene.FindRoot("Audio Player Blizzard Wyrm");
                var apbwFsm = apbw.LocateMyFSM("Play");
                apbwFsm.GetAction<PlayerDataBoolTest>("State 1", 0).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                apbwFsm.SetState("Pause");

                var wap = scene.FindRoot("white_ash_particles");
                wap.LocateMyFSM("Control").SendEvent("DRIFT END");
                var wapFsm = wap.LocateMyFSM("Control2");
                wapFsm.GetAction<PlayerDataBoolTest>("Init", 3).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                wapFsm.SetState("Pause");

                var bs = scene.FindRoot("Battle Scene");
                var bsFsm = bs.LocateMyFSM("FSM");
                bsFsm.GetAction<SendMessageByTag>("Activate", 1).message = "SetStill";
                bsFsm.SetState("Pause");

                var bp = scene.Find("blizzard_particles");
                var bpFsm = bp.LocateMyFSM("Control");
                bpFsm.GetAction<PlayerDataBoolTest>("Init", 3).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                bpFsm.SetState("Pause");

                var bp1 = scene.Find("blizzard_particles (1)");
                var bp1Fsm = bp1.LocateMyFSM("Control");
                bp1Fsm.GetAction<PlayerDataBoolTest>("Init", 3).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                bp1Fsm.SetState("Pause");

                var bp2 = scene.Find("blizzard_particles (2)");
                var bp2Fsm = bp2.LocateMyFSM("Control");
                bp2Fsm.GetAction<PlayerDataBoolTest>("Init", 3).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                bp2Fsm.SetState("Pause");

                var bp3 = scene.Find("blizzard_particles (3)");
                var bp3Fsm = bp3.LocateMyFSM("Control");
                bp3Fsm.GetAction<PlayerDataBoolTest>("Init", 3).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                bp3Fsm.SetState("Pause");
            }
            else if (scene.name == "Room_Wyrm")
            {
                scene.FindRoot("Audio Blizzard").SetActive(false);
                scene.FindRoot("Audio Rumble Cave In").SetActive(false);
                scene.FindRoot("Avalanche End").SetActive(false);
                var activateFsm = scene.FindRoot("Avalanche").LocateMyFSM("Activate");
                activateFsm.GetAction<PlayerDataBoolTest>("Idle", 1).boolName = nameof(DreamKing.Instance.SaveSettings.SfGrenadeDreamKingOwnOutskirtsBlizzard);
                activateFsm.SetState("Pause");
                scene.FindRoot("blizzard_particles").LocateMyFSM("Stop").SetState("Stop");
                scene.FindRoot("blizzard_particles").SetActive(false);
                scene.FindRoot("Door Blocker").SetActive(false);

                var sceneryGo = scene.FindRoot("_Scenery");
                for (int i = 0; i < sceneryGo.transform.childCount; i++)
                {
                    var gogo = sceneryGo.transform.GetChild(i).gameObject;
                    if (!gogo.activeInHierarchy)
                        continue;
                    if (gogo.name.Contains("collapse_chunk2"))
                    {
                        gogo.SetActive(false);
                    }
                    else if (gogo.name.Contains("wyrm_tendril"))
                    {
                        gogo.SetActive(false);
                    }
                    else if (gogo.name.Contains("wispy smoke BG"))
                    {
                        gogo.SetActive(false);
                    }
                }
            }

            Log("~Outskirts Blizzard");
        }

        private void CreateBreakableWall(string sceneName, string name, Vector3 pos, Vector3 angles, Vector3 scale, Vector2 size, string playerDataBool)
        {
            Log("!CreateBreakableWall");
            //SFGrenadeDreamKing_TotOpened

            GameObject breakableWall = Instantiate(PrefabHolder.BreakableWallPrefab);
            breakableWall.SetActive(true);
            breakableWall.name = name;
            breakableWall.transform.position = pos;
            breakableWall.transform.eulerAngles = angles;
            breakableWall.transform.localScale = scale;

            var wallBc2d = breakableWall.GetComponent<BoxCollider2D>();
            wallBc2d.size = size;
            wallBc2d.offset = Vector2.zero;

            var wallFsm = breakableWall.LocateMyFSM("breakable_wall_v2");
            wallFsm.FsmVariables.GetFsmBool("Activated").Value = false;
            wallFsm.FsmVariables.GetFsmBool("Ruin Lift").Value = false;
            wallFsm.FsmVariables.GetFsmString("PlayerData Bool").Value = playerDataBool;

            Log("~CreateBreakableWall");
        }

        private void CreateGateway(string gateName, Vector2 pos, Vector2 size, string toScene, string entryGate, Vector2 respawnPoint,
            bool right, bool left, bool onlyOut, GameManager.SceneLoadVisualizations vis)
        {
            Log("!Gateway");

            GameObject gate = new GameObject(gateName);
            gate.transform.SetPosition2D(pos);
            var tp = gate.AddComponent<TransitionPoint>();
            if (!onlyOut)
            {
                var bc = gate.AddComponent<BoxCollider2D>();
                bc.size = size;
                bc.isTrigger = true;
                tp.SetTargetScene(toScene);
                tp.entryPoint = entryGate;
            }
            tp.alwaysEnterLeft = left;
            tp.alwaysEnterRight = right;

            GameObject rm = new GameObject("Hazard Respawn Marker");
            rm.transform.parent = gate.transform;
            rm.transform.SetPosition2D(pos.x + respawnPoint.x, pos.y + respawnPoint.y);
            var tmp = rm.AddComponent<HazardRespawnMarker>();
            tmp.respawnFacingRight = right;
            tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
            tp.sceneLoadVisualization = vis;

            Log("~Gateway");
        }

        private void CreateDreamGateway(string gateName, Vector2 pos, Vector2 size, string toScene, string returnScene)
        {
            Log("!DreamGateway");

            // a05DE2 = Abyss_05 -> Dusk Knight/Dream Enter 2
            // a05IdlePT = Abyss_05 -> Dusk Knight/Idle Pt
            GameObject dreamEnter = Instantiate(PrefabHolder.A05De2);
            dreamEnter.name = gateName;
            dreamEnter.SetActive(true);
            dreamEnter.transform.position = pos;
            dreamEnter.transform.localScale = Vector3.one;
            dreamEnter.transform.eulerAngles = Vector3.zero;

            dreamEnter.GetComponent<BoxCollider2D>().size = size;
            dreamEnter.GetComponent<BoxCollider2D>().offset = Vector2.zero;

            foreach (var pfsm in dreamEnter.GetComponents<PlayMakerFSM>())
            {
                if (pfsm.FsmName == "Control")
                {
                    pfsm.FsmVariables.GetFsmString("Return Scene").Value = returnScene;
                    pfsm.FsmVariables.GetFsmString("To Scene").Value = toScene;
                }
            }

            GameObject dreamPt = Instantiate(PrefabHolder.A05IdlePt);
            dreamPt.SetActive(true);
            dreamPt.transform.position = new Vector3(pos.x, pos.y, -0.002f);
            dreamPt.transform.localScale = Vector3.one;
            dreamPt.transform.eulerAngles = Vector3.zero;

            var shape = dreamPt.GetComponent<ParticleSystem>().shape;
            shape.scale = new Vector3(size.x, size.y, 0.001f);

            Log("~DreamGateway");
        }

        private void CreateBench(string benchName, Vector3 pos, string sceneName)
        {
            GameObject bench = Instantiate(PrefabHolder.WhiteBenchPrefab);
            bench.SetActive(true);
            bench.transform.position = pos;
            bench.name = benchName;
            var benchFsm = bench.LocateMyFSM("Bench Control");
            benchFsm.FsmVariables.FindFsmString("Scene Name").Value = sceneName;
            benchFsm.FsmVariables.FindFsmString("Spawn Name").Value = benchName;
        }

        private void PrintDebug(GameObject go, string tabindex = "")
        {
            Log(tabindex + "Name: " + go.name);
            foreach (var comp in go.GetComponents<Component>())
            {
                Log(tabindex + "Component: " + comp.GetType());
            }
            for (int i = 0; i < go.transform.childCount; i++)
            {
                PrintDebug(go.transform.GetChild(i).gameObject, tabindex + "\t");
            }
        }

        private void Log(String message)
        {
            Logger.Log($"[{GetType().FullName.Replace(".", "]:[")}] - {message}");
        }
        private void Log(System.Object message)
        {
            Logger.Log($"[{GetType().FullName.Replace(".", "]:[")}] - {message}");
        }

        private static void SetInactive(GameObject go)
        {
            if (go != null)
            {
                DontDestroyOnLoad(go);
                go.SetActive(false);
            }
        }
        private static void SetInactive(UnityEngine.Object go)
        {
            if (go != null)
            {
                DontDestroyOnLoad(go);
            }
        }
    }
}
