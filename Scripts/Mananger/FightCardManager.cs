using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightCardManager 
{
    public static FightCardManager Instance = new FightCardManager();
    public List<string> cardList;//卡牌集合
    public List<string> usedCardList;//弃牌集合
    public void Init()
    {
        cardList = new List<string>();
        usedCardList = new List<string>();

        //临时集合
        List<string> tempList = new List<string>();
        //存储至临时集合
        tempList.AddRange(RoleManager.Instance.cardList);
        while (tempList.Count > 0)
        {
            //随机下标
            int tempIndex = Random.Range(0, tempList.Count);
            //添加至卡堆
            cardList.Add(tempList[tempIndex]);
            //临时集合删除
            tempList.RemoveAt(tempIndex);
        }
        Debug.Log(cardList.Count);

    }
    //是否有卡
    public bool HasCard()
    {
        return cardList.Count > 0;
    }
    //抽卡
    public string DrawCard()
    {
        string id = cardList[cardList.Count - 1];
        cardList.RemoveAt(cardList.Count - 1);
        return id;
    }
}
