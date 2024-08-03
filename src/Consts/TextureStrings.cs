using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace DreamKing.Consts;

public class TextureStrings
{
    #region Texture Keys and Files
    // Shopitems
    public const string Shovel1Key = "Shovel_1";
    private const string Shovel1File = "DreamKing.Resources.Shovel_1.png";
    public const string Shovel2Key = "Shovel_2";
    private const string Shovel2File = "DreamKing.Resources.Shovel_2.png";
    // Inventory Items
    public const string InvShovel1Key = "Inv_Shovel_1";
    private const string InvShovel1File = "DreamKing.Resources.Inv_Shovel_1.png";
    public const string InvShovel2Key = "Inv_Shovel_2";
    private const string InvShovel2File = "DreamKing.Resources.Inv_Shovel_2.png";
    // Bosses
    public const string PaleWyrmKey = "PaleWyrm";
    private const string PaleWyrmFile = "DreamKing.Resources.PaleWyrm.png";
    public const string PaleNoskKey = "PaleNosk";
    private const string PaleNoskFile = "DreamKing.Resources.PaleNosk.png";
    // Achievements
    public const string AchievementItemKey = "Achievement_Item";
    private const string AchievementItemFile = "DreamKing.Resources.Achievement_Item.png";
    public const string AchievementBossKey = "Achievement_Boss";
    private const string AchievementBossFile = "DreamKing.Resources.Achievement_Boss.png";
    public const string AchievementBoughtShovel1Key = "Achievement_Shovel_1";
    private const string AchievementBoughtShovel1File = "DreamKing.Resources.Achievement_Shovel_1.png";
    public const string AchievementBoughtShovel2Key = "Achievement_Shovel_2";
    private const string AchievementBoughtShovel2File = "DreamKing.Resources.Achievement_Shovel_2.png";
    public const string AchievementLostDreamsKey = "Achievement_EnterLostDreams";
    private const string AchievementLostDreamsFile = "DreamKing.Resources.Achievement_Lost_Dreams.png";
    #endregion

    private Dictionary<string, Sprite> _dict;

    public TextureStrings()
    {
        Assembly asm = Assembly.GetExecutingAssembly();
        _dict = new Dictionary<string, Sprite>();
        var tmpTextures = new Dictionary<string, string>();
        tmpTextures.Add(Shovel1Key, Shovel1File);
        tmpTextures.Add(Shovel2Key, Shovel2File);
        tmpTextures.Add(InvShovel1Key, InvShovel1File);
        tmpTextures.Add(InvShovel2Key, InvShovel2File);
        tmpTextures.Add(AchievementItemKey, AchievementItemFile);
        tmpTextures.Add(AchievementBossKey, AchievementBossFile);
        tmpTextures.Add(AchievementBoughtShovel1Key, AchievementBoughtShovel1File);
        tmpTextures.Add(AchievementBoughtShovel2Key, AchievementBoughtShovel2File);
        tmpTextures.Add(AchievementLostDreamsKey, AchievementLostDreamsFile);

        foreach (var pair in tmpTextures)
        {
            using (Stream s = asm.GetManifestResourceStream(pair.Value))
            {
                if (s != null)
                {
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    s.Dispose();

                    //Create texture from bytes
                    var tex = new Texture2D(2, 2);

                    tex.LoadImage(buffer, true);

                    // Create sprite from texture
                    // Split is to cut off the DreamKing.Resources. and the .png
                    _dict.Add(pair.Key, Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                }
            }
        }
    }

    public Sprite Get(string key)
    {
        return _dict[key];
    }
}