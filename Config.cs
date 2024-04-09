
using UnityEngine;

//只用于保存一些常量
public class Config
{
  #region 背景音乐和游戏UI音效
  /// <summary>
  /// 游戏中背景音乐,轻快
  /// </summary>
  public const string BgminGameQucik = "Audio/Music/bgm5";
  /// <summary>
  /// 主菜单背景音乐
  /// </summary>
  public const string BgminMainMenu = "Audio/Music/ThemeSong";
  /// <summary>
  /// 失败音乐
  /// </summary>
  public const string loseSound = "Audio/Sound/losemusic";
  /// <summary>
  /// 胜利音乐
  /// </summary>
  public const string winSound = "Audio/Sound/winmusic";
  /// <summary>
  /// 点击音效重
  /// </summary>
  public const string ButtonOnClick = "Audio/Sound/buttonclick";
  /// <summary>
  /// 点击音效轻
  /// </summary>
  public const string ButtonOnClickTap = "Audio/Sound/tap";
  #endregion

  #region 僵尸音效
  /// <summary>
  /// 僵尸吃植物音效
  /// </summary>
  public const string eat = "Audio/Sound/chompsoft";
  /// <summary>
  /// 最后一波僵尸音效
  /// </summary>
  public const string lastwave = "Audio/Sound/lastwave";
  /// <summary>
  /// 一大波僵尸音效
  /// </summary>
  public const string bigwave = "Audio/Sound/bigwave";
  /// <summary>
  /// 旗帜僵尸音效
  /// </summary>
  public const string FlagZombie = "Audio/Sound/groan";
  #endregion

  #region 植物音效
  /// <summary>
  /// 植物种植音效
  /// </summary>
  public const string PlantGrowers = "Audio/Sound/plant";
  /// <summary>
  /// 豌豆音效
  /// </summary>
  public const string peaShoot = "Audio/Sound/shoot";

  public const string Squash = "Audio/Sound/squash";
  #endregion

  /// <summary>
  /// 阳光获取音效
  /// </summary>
  public const string getSun = "Audio/Sound/getSun";
  /// <summary>
  /// 铲子音效
  /// </summary>
  public const string soil = "Audio/Sound/soil";
  /// <summary>
  /// 暂停音效
  /// </summary>
  public const string pause = "Audio/Sound/pause";
  /// <summary>
  /// 准备音效
  /// </summary>
  public const string prepare = "Audio/Sound/prepare";
  /// <summary>
  /// 小推车音效
  /// </summary>
  public const string Stroller = "Audio/Sound/lawnmower";
}