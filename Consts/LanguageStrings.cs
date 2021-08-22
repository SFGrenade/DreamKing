using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace DreamKing.Consts
{
    public class LanguageStrings
    {
        #region Language Strings
        // Shop Items
        public const string Shovel1NameKey = "SFGrenadeDreamKing_Shovel1NameConvo";
        public const string Shovel1DescKey = "SFGrenadeDreamKing_Shovel1DescConvo";
        public const string Shovel2NameKey = "SFGrenadeDreamKing_Shovel2NameConvo";
        public const string Shovel2DescKey = "SFGrenadeDreamKing_Shovel2DescConvo";
        // Inventory Items
        public const string Shovel1InvDescKey = "SFGrenadeDreamKing_Shovel1InvDesc";
        public const string Shovel2InvDescKey = "SFGrenadeDreamKing_Shovel2InvDesc";
        // Bosses
        public const string PaleWyrmNameKey = "SFGrenadeDreamKing_PaleWyrmName";
        public const string PaleNoskNameKey = "SFGrenadeDreamKing_PaleNoskName";
        // Places
        public const string WwAreaTitleEvent = "SFGrenadeDreamKing_WwAreaTitle";
        #endregion

        #region Achievement Strings
        // Shop Items
        public const string AchievementBoughtShovel1TitleKey = "SFGrenadeDreamKing_Achievement_Title_BoughtShovel1";
        public const string AchievementBoughtShovel1TextKey = "SFGrenadeDreamKing_Achievement_Text_BoughtShovel1";
        public const string AchievementBoughtShovel2TitleKey = "SFGrenadeDreamKing_Achievement_Title_BoughtShovel2";
        public const string AchievementBoughtShovel2TextKey = "SFGrenadeDreamKing_Achievement_Text_BoughtShovel2";
        // Locations
        public const string AchievementEnterLostDreamsTitleKey = "SFGrenadeDreamKing_Achievement_Title_EnterLostDreams";
        public const string AchievementEnterLostDreamsTextKey = "SFGrenadeDreamKing_Achievement_Text_EnterLostDreams";
        // Bosses
        public const string AchievementDefeatedPaleWyrmTitleKey = "SFGrenadeDreamKing_Achievement_Title_DefeatedPaleWyrm";
        public const string AchievementDefeatedPaleWyrmTextKey = "SFGrenadeDreamKing_Achievement_Text_DefeatedPaleWyrm";
        public const string AchievementDefeatedPaleNoskTitleKey = "SFGrenadeDreamKing_Achievement_Title_DefeatedPaleNosk";
        public const string AchievementDefeatedPaleNoskTextKey = "SFGrenadeDreamKing_Achievement_Text_DefeatedPaleNosk";
        #endregion

        private readonly Dictionary<string, Dictionary<string, Dictionary<string, string>>> _jsonDict;

        public LanguageStrings()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream s = asm.GetManifestResourceStream("DreamKing.Resources.Language.json"))
            {
                if (s == null) return;

                byte[] buffer = new byte[s.Length];
                s.Read(buffer, 0, buffer.Length);
                s.Dispose();

                string json = System.Text.Encoding.Default.GetString(buffer);

                _jsonDict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
            }
        }

        public string Get(string key, string sheet)
        {
            GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
            try
            {
                return _jsonDict[lang.ToString()][sheet][key].Replace("<br>", "\n");
            }
            catch
            {
                return _jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet][key].Replace("<br>", "\n");
            }
        }

        public bool ContainsKey(string key, string sheet)
        {
            try
            {
                GlobalEnums.SupportedLanguages lang = GameManager.instance.gameSettings.gameLanguage;
                try
                {
                    return _jsonDict[lang.ToString()][sheet].ContainsKey(key);
                }
                catch
                {
                    try
                    {
                        return _jsonDict[GlobalEnums.SupportedLanguages.EN.ToString()][sheet].ContainsKey(key);
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
