using DreamKing.Consts;
using UnityEngine;

namespace DreamKing.MonoBehaviours;

public class PatchDreamExit : MonoBehaviour
{
    public Vector2 ExitPosition = GameObject.Find("e-xit1").transform.position;
    public string ToGate = TransitionGateNames.RwWw012;
    public string ToScene = TransitionGateNames.RWyrm;

    public void Start()
    {
        GameObject doorWarp = Instantiate(PrefabHolder.Wp03Warp);
        doorWarp.SetActive(true);
        doorWarp.name = "doorWarp";
        doorWarp.transform.position = ExitPosition;
        doorWarp.transform.localScale = Vector3.one;
        doorWarp.transform.eulerAngles = Vector3.zero;
        var pfsm = doorWarp.LocateMyFSM("Door Control");
        pfsm.FsmVariables.GetFsmString("EntryGate").Value = ToGate;
        pfsm.FsmVariables.GetFsmString("New Scene").Value = ToScene;
        pfsm.FsmVariables.GetFsmString("Set PD Bool").Value = "";
        var bc2d = doorWarp.GetComponent<BoxCollider2D>();
        bc2d.size = new Vector2(2.13f, 0.2f);
        bc2d.offset = new Vector2(0.0f, 0.6f);
        GameObject promptMarker = new GameObject("Prompt Marker");
        promptMarker.transform.SetParent(doorWarp.transform);
        promptMarker.transform.localPosition = new Vector3(0.0f, 3.94f, 0.0f);
    }
}