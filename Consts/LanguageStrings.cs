using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace DreamKing.Consts
{
    public class LanguageStrings
    {
        #region Language Strings
        // Shop Items
        public const string Shovel1Name_Key = "SFGrenadeDreamKing_Shovel1NameConvo";
        public const string Shovel1Desc_Key = "SFGrenadeDreamKing_Shovel1DescConvo";
        public const string Shovel2Name_Key = "SFGrenadeDreamKing_Shovel2NameConvo";
        public const string Shovel2Desc_Key = "SFGrenadeDreamKing_Shovel2DescConvo";
        // Inventory Items
        public const string Shovel1InvDesc_Key = "SFGrenadeDreamKing_Shovel1InvDesc";
        public const string Shovel2InvDesc_Key = "SFGrenadeDreamKing_Shovel2InvDesc";
        // Bosses
        public const string PaleWyrmName_Key = "SFGrenadeDreamKing_PaleWyrmName";
        public const string PaleNoskName_Key = "SFGrenadeDreamKing_PaleNoskName";
        // Places
        public const string WwAreaTitle_Event = "SFGrenadeDreamKing_WwAreaTitle";
        #endregion

        #region Achievement Strings
        // Shop Items
        public const string Achievement_BoughtShovel1_Title_Key = "SFGrenadeDreamKing_Achievement_Title_BoughtShovel1";
        public const string Achievement_BoughtShovel1_Text_Key = "SFGrenadeDreamKing_Achievement_Text_BoughtShovel1";
        public const string Achievement_BoughtShovel2_Title_Key = "SFGrenadeDreamKing_Achievement_Title_BoughtShovel2";
        public const string Achievement_BoughtShovel2_Text_Key = "SFGrenadeDreamKing_Achievement_Text_BoughtShovel2";
        // Locations
        public const string Achievement_EnterLostDreams_Title_Key = "SFGrenadeDreamKing_Achievement_Title_EnterLostDreams";
        public const string Achievement_EnterLostDreams_Text_Key = "SFGrenadeDreamKing_Achievement_Text_EnterLostDreams";
        // Bosses
        public const string Achievement_DefeatedPaleWyrm_Title_Key = "SFGrenadeDreamKing_Achievement_Title_DefeatedPaleWyrm";
        public const string Achievement_DefeatedPaleWyrm_Text_Key = "SFGrenadeDreamKing_Achievement_Text_DefeatedPaleWyrm";
        public const string Achievement_DefeatedPaleNosk_Title_Key = "SFGrenadeDreamKing_Achievement_Title_DefeatedPaleNosk";
        public const string Achievement_DefeatedPaleNosk_Text_Key = "SFGrenadeDreamKing_Achievement_Text_DefeatedPaleNosk";
        #endregion

        private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> jsonDict;

        public LanguageStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream s = asm.GetManifestResourceStream("TestOfTeamwork.Resources.Language.json"))
            {
                if (s == null) return;

                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                s.Dispose();

                string json = System.Text.Encoding.Default.GetString(buffer);

                jsonDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
            }
        }

        public string Get(string key, string sheet)
        {
            GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
            try
            {
                return jsonDict[lang.ToString()][sheet][key].Replace("<br>", "\n");
            }
            catch
            {
                return jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet][key].Replace("<br>", "\n");
            }
        }

        public bool ContainsKey(string key, string sheet)
        {
            try
            {
                GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
                try
                {
                    return jsonDict[lang.ToString()][sheet].ContainsKey(key);
                }
                catch
                {
                    try
                    {
                        return jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet].ContainsKey(key);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
