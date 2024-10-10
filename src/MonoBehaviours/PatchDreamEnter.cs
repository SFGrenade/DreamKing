using JetBrains.Annotations;
using UnityEngine;

namespace DreamKing.MonoBehaviours;

[UsedImplicitly]
public class PatchDreamEnter : MonoBehaviour
{
    public string EnterDoorName = "";
    public Vector2 EnterPosition = Vector2.zero;

    public void Start()
    {
    }

    private void Log(string message)
    {
    }

    private void Log(object message)
    {
    }
}