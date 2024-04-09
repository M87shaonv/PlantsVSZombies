using System.Collections.Generic;
using UnityEngine;

public class LevelManger : MonoBehaviour
{
  public static LevelManger Instance;
  public List<GameObject> cards;
  private void Awake()
  {
    Instance = this;
  }
  public int currentLevel;
  void Update()
  {
    LevelEVent();
  }

  public void LevelEVent()
  {
    if (currentLevel == 2)
    {
      cards[2].SetActive(true);//双发射手卡牌
    }
    if (currentLevel == 3)
    {
      cards[3].SetActive(true);//坚果卡牌
    }
    if (currentLevel == 4)
    {
      cards[4].SetActive(true);//火焰木桩卡牌
    }
    if (currentLevel == 5)
    {

      cards[5].SetActive(true);//窝瓜卡牌
    }

    if (currentLevel == 6)
    {
      cards[6].SetActive(true);//寒冰射手卡牌
    }
    if (currentLevel == 7)
    {
      cards[7].SetActive(true);//机枪豌豆卡牌
    }
  }
}