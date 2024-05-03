using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BuyCard : MonoBehaviour
{
  public String CardName;
  string ownedCardData;
  void Awake()
  {
    CardName = this.gameObject.name;
    ownedCardData = PlayerPrefs.GetString("OwnedCard");
    string[] ownedCards = ownedCardData.Split(',');//将从PlayerPrefs中获取的数据按逗号分隔，存储到ownedCards数组中
                                                   //使用 LINQ 的 Any 方法，它会遍历 ownedCards 数组中的值，检查是否存在不等于 CardName 的值
    if (ownedCards.Any(cn => cn == $"Card{CardName}"))
    {
      this.gameObject.SetActive(false);
    }
  }
  public void BuyThisCard()
  {
    Card card = Resources.Load($"Perfabs/Card{CardName}").GetComponent<Card>();
    int gold = PlayerPrefs.GetInt("Gold");
    if (card.needsumpoint <= gold)
    {
      ownedCardData = PlayerPrefs.GetString("OwnedCard");
      PlayerPrefs.SetString("OwnedCard", ownedCardData + $"Card{CardName}" + ',');//标记已购买
      gold -= card.needsumpoint;//扣除需要的金币
      PlayerPrefs.SetInt("Gold", gold);
      this.gameObject.SetActive(false);
    }
  }
}