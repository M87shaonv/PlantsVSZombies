using UnityEngine;
using System.Collections.Generic;

public class LevelData : ScriptableObject
{
  public List<LevelItem> levelDataList = new List<LevelItem>();
}

[System.Serializable]
public class LevelItem
{
  public int ID;
  /// <summary>
  /// 关卡ID
  /// </summary>
  public int LevelID;
  /// <summary>
  /// 波数
  /// </summary>
  public int progress;
  /// <summary>
  /// 生成时间
  /// </summary>
  public int createTime;
  /// <summary>
  /// 生成类型
  /// </summary>
  public int zombieType;
  /// <summary>
  /// 当前关卡最大波数
  /// </summary>
  public int Maxprogress;
  /// <summary>
  /// 是否显示警告
  /// </summary>
  public int displayWarning;
}