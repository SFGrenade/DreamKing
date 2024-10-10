using JetBrains.Annotations;
using UnityEngine;

namespace DreamKing.MonoBehaviours;

[UsedImplicitly]
public class PatchDreamExit : MonoBehaviour
{
    public Vector2 ExitPosition = Vector2.zero;
    public string ToGate = "";
    public string ToScene = "";

    public void Start()
    {
    }
}