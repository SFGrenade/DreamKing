using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DreamKing.MonoBehaviours
{
    public class PatchDreamBeam : MonoBehaviour
    {
        public void Start()
        {
            GameObject dreamBeamAnim = GameObject.Instantiate(PrefabHolder.wp03DreamBeamAnim, transform);
            dreamBeamAnim.transform.localPosition = Vector3.zero;
            dreamBeamAnim.transform.localEulerAngles = Vector3.zero;
            dreamBeamAnim.transform.localScale = Vector3.one;
            dreamBeamAnim.SetActive(true);
            dreamBeamAnim.name = "dream_beam_animation";
        }
    }
}
