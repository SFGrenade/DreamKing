using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DreamKing.Consts
{
    public class AudioStrings
    {
        public const string RawstyleKey = "Rawstyle";
        private const string RawstyleFile = "Rawstyle_Test_3";

        private Dictionary<string, AudioClip> _dict;

        public AudioStrings(SceneChanger sc)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            _dict = new Dictionary<string, AudioClip>();
            var tmpAudio = new Dictionary<string, string>();
            tmpAudio.Add(RawstyleKey, RawstyleFile);
            foreach (var pair in tmpAudio)
            {
                _dict.Add(pair.Key, sc.AbOverallMat.LoadAsset<AudioClip>(pair.Value));
                Object.DontDestroyOnLoad(_dict[pair.Key]);
            }
        }

        public AudioClip Get(string key)
        {
            return _dict[key];
        }
    }
}
