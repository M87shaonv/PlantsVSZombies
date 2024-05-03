using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;
using Unity.VisualScripting;

public class CardListUI : MonoBehaviour
{
  public List<Card> cardList;//引用所有的卡片
  public GameObject ChooseBar;//选择栏
  public GameObject CardPrefab;//卡片预制体
  public List<Card> ChooseCardsList;//选择的卡片列表
  public int currentIndex = 0;//当前卡片将放到哪个索引的位置
  public int Count = 0;//卡片数量
  void Start()
  {
    //初始化卡片列表
    foreach (string cardName in PlayerPrefs.GetString("OwnedCard").Split(','))
    {
      if (!string.IsNullOrEmpty(cardName))//判断是否为空
      {
        Card card = Resources.Load($"Perfabs/{cardName}").GetComponent<Card>();
        cardList.Add(card);
        card.CardId = cardList.IndexOf(card);//给卡片编号
      }
    }
    // string ownedCardData = PlayerPrefs.GetString("OwnedCard");
    // string[] ownedCards = ownedCardData.Split(',');
    // foreach (string cardName in ownedCards)
    // {
    //   GameObject cardPrefab = Resources.Load($"Perfabs/{cardName}") as GameObject;
    //   if (cardPrefab != null)
    //   {
    //     Card cardComponent = cardPrefab.GetComponent<Card>();
    //     if (cardComponent != null)
    //     {
    //       cardList.Add(cardComponent);
    //       cardComponent.CardId = cardList.IndexOf(cardComponent); // 给卡片编号
    //     }
    //   }
    // }
    Count = UIManger.Instance.cardListUI.cardList.Count;
    ChooseCardsList = new List<Card>();
    for (int i = 0; i < cardList.Count; i++)
    {
      GameObject Card = Instantiate(CardPrefab);
      Card.transform.SetParent(ChooseBar.transform, false);
      Card.name = "Card" + i.ToString();
      Card.transform.Find("Bg").gameObject.SetActive(false);
    }
    UIManger.Instance.selectCardBarUI.InitiCardBarUI();
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

  public void UpdateCardPosition()//更新卡牌位置
  {
    for (int i = 0; i < ChooseCardsList.Count; i++)
    {
      Card useCard = ChooseCardsList[i];
      Transform CardTransform = ChooseBar.transform.Find("Card" + i.ToString());
      useCard.GetComponent<Card>().isMoving = true;
      useCard.transform.DOMove(CardTransform.position, 0.3f).OnComplete(
        () =>
      {
        useCard.transform.SetParent(CardTransform, false);
        useCard.transform.localPosition = Vector3.zero;//卡牌位置设置为 (0, 0, 0)
        useCard.GetComponent<Card>().isMoving = false;
      }
      );
    }
  }


}
