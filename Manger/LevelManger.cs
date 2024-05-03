using System.Collections.Generic;
using UnityEngine;

public class LevelManger : MonoBehaviour
{
  public static LevelManger Instance;
  LevelItem levelItem;
  /// <summary>
  /// 0脚本花,1土豆地雷,2坚果墙,3寒冰豌豆射手,4双发豌豆射手,5火炬树桩
  /// </summary>
  public List<GameObject> cards;
  public GameObject[] zombieShows;
  private void Awake()
  {
    Instance = this;
    currentLevel = PlayerPrefs.GetInt("Level");
    levelItem = ZombieManger.Instance.levelData.levelDataList[currentLevel];
  }
  public int currentLevel;
  void Start()
  {
    LevelEVent();
  }

  public void LevelEVent()
  {
    //显示当前关卡的僵尸
    for (int current = 0; current < ZombieManger.Instance.levelData.levelDataList.Count; current++)//当前关卡当前波次的所有levelItem数据
    {
      LevelItem levelItem = ZombieManger.Instance.levelData.levelDataList[current];
      if (levelItem.LevelID == currentLevel)
      {
        zombieShows[levelItem.zombieType].SetActive(true);
      }
      if (levelItem.LevelID < currentLevel)
      {
        break;
      }
    }
    if (levelItem.LevelID == 2)
      PlayerPrefs.SetString("OwnedCard", cards[0].name);
    else if (levelItem.LevelID == 3)
      PlayerPrefs.SetString("OwnedCard", cards[1].name);
    else if (levelItem.LevelID == 4)
      PlayerPrefs.SetString("OwnedCard", cards[2].name);
    else if (levelItem.LevelID == 5)
      PlayerPrefs.SetString("OwnedCard", cards[3].name);
    else if (levelItem.LevelID == 6)
      PlayerPrefs.SetString("OwnedCard", cards[4].name);
    else if (levelItem.LevelID == 7)
      PlayerPrefs.SetString("OwnedCard", cards[5].name);
  }
}