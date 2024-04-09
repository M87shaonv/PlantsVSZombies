using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CardListUI : MonoBehaviour
{
  public List<Card> cardList;//引用所有的卡片
  void Start()
  {
    DisableCardList();
  }
  /// <summary>
  /// 在外部调用来显示卡片列表
  /// </summary>
  public void ShowCardList()
  {
    GetComponent<RectTransform>().DOLocalMoveY(484.2f, 1);
    EnableCardList();
  }

  public void HideCardList()
  {
    GetComponent<RectTransform>().DOLocalMoveY(600, 1);
    DisableCardList();

  }
  /// <summary>
  /// 禁用所有卡片
  /// </summary>
  void DisableCardList()
  {
    foreach (Card card in cardList)//遍历所有卡片
    {
      card.DisableCard();
    }
  }
  /// <summary>
  /// 显示所有卡片
  /// </summary>
  void EnableCardList()
  {
    foreach (Card card in cardList)
      card.EnableCard();
  }
  
  
}
