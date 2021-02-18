using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DreamKing.Consts
{
    public class AudioStrings
    {
        public const string RawstyleKey = "Rawstyle";
        private const string RawstyleFile = "Rawstyle_Test_3";

        private Dictionary<string, AudioClip> dict;

        public AudioStrings(SceneChanger sc)
        {
            Assembly _asm = Assembly.GetExecutingAssembly();
            dict = new Dictionary<string, AudioClip>();
            var tmpAudio = new Dictionary<string, string>();
            tmpAudio.Add(RawstyleKey, RawstyleFile);
            foreach (var pair in tmpAudio)
            {
                dict.Add(pair.Key, sc.AbOverallMat.LoadAsset<AudioClip>(pair.Value));
                GameObject.DontDestroyOnLoad(dict[pair.Key]);
            }
        }

        public AudioClip Get(string key)
        {
            return dict[key];
        }
    }
}
