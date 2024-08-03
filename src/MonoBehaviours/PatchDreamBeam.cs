using UnityEngine;

namespace DreamKing.MonoBehaviours;

public class PatchDreamBeam : MonoBehaviour
{
    public void Start()
    {
        GameObject dreamBeamAnim = Instantiate(PrefabHolder.Wp03DreamBeamAnim, transform);
        dreamBeamAnim.transform.localPosition = Vector3.zero;
        dreamBeamAnim.transform.localEulerAngles = Vector3.zero;
        dreamBeamAnim.transform.localScale = Vector3.one;
        dreamBeamAnim.SetActive(true);
        dreamBeamAnim.name = "dream_beam_animation";
    }
}