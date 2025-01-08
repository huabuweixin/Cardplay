using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//战斗界面
public class FightUI : UIBase
{
    private Text cardCountTxt;//卡牌数量
    private Text nocardCountTxt;//弃牌数量
    private Text powerTxt;
    private Text hpTxt;
    private Image hpImg;
    private Text fyTxt;
    private List<CardItem> cardItemList;//存储卡牌物体的集合
    private void Awake()
    {
        cardItemList = new List<CardItem>();
        cardCountTxt = transform.Find("hasCard/icon/Text").GetComponent<Text>();
        nocardCountTxt = transform.Find("noCard/icon/Text").GetComponent<Text>();
        powerTxt = transform.Find("mana/Text").GetComponent<Text>();
        hpTxt= transform.Find("hp/moneyTxt").GetComponent<Text>();
        hpImg = transform.Find("hp/fill").GetComponent<Image>();
        fyTxt = transform.Find("hp/fangyu/Text").GetComponent<Text>();
        transform.Find("turnBtn").GetComponent<Button>().onClick.AddListener(onChangeTurnBtn);
    }
    //玩家回合结束切换敌人回合
    private void onChangeTurnBtn()
    {
        if(FightManager.Instance.fightUnit is Fight_PlayerTurn)
        {
            FightManager.Instance.ChangeType(FightType.Enemy);
            UpdateCardCount();
            UpdateUsedCardCount();
        }
    }
    private void Start()
    {
        UpdateHp();
        UpdateUsedCardCount();
        UpdateDefense();
        UpdatePower();
        UpdateCardCount();
        UpdateUsedCardCount();
    }
    //更新血量显示
    public void UpdateHp()
    {
        hpTxt.text = FightManager.Instance.CurHp + "/" + FightManager.Instance.MaxHp;
        hpImg.fillAmount = (float)FightManager.Instance.CurHp / (float)FightManager.Instance.MaxHp;
    }
    //更新能量
    public void UpdatePower()
    {
        powerTxt.text = FightManager.Instance.CurPowerCount + "/" + FightManager.Instance.MaxPowerCount;
    }
    //防御更新
    public void UpdateDefense()
    {
        fyTxt.text = FightManager.Instance.DefenseCount.ToString();
    }
    //更新卡牌数量
    public void UpdateCardCount()
    {
        cardCountTxt.text = FightCardManager.Instance.cardList.Count.ToString();
    }
    public void UpdateUsedCardCount()
    {
        nocardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
    }
    //创建卡牌物体
    public void CreateCardItem(int count)
    {
        if (count > FightCardManager.Instance.cardList.Count)
        {
            count = FightCardManager.Instance.cardList.Count;
        }
        for(int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(Resources.Load("UI/CardItem"), transform) as GameObject;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -30);
            //var item = obj.AddComponent<CardItem>();
            string cardId = FightCardManager.Instance.DrawCard();
            Dictionary<string, string> data = GameConfigManager.Instance.GetCardById(cardId);
            CardItem item = obj.AddComponent(System.Type.GetType(data["Script"])) as CardItem;
            item.Init(data);
            cardItemList.Add(item);
        }
    }
    //更新卡牌位置
    public void UpdateCardItemPos()
    {
        float offset = 400.0f / cardItemList.Count;
        Vector2 startPos = new Vector2(180-cardItemList.Count / 2.0f * offset + offset * 0.5f, -30);
        for(int i = 0; i < cardItemList.Count; i++)
        {
            cardItemList[i].GetComponent<RectTransform>().DOAnchorPos(startPos,0.5f);
            startPos.x = startPos.x + offset;
        }
    }
    //删除卡牌
    public void RemoveCard(CardItem item)
    {
        AudioManager.Instance.PlayEffect("Cards/cardShove");//移除音效
        item.enabled = false;//禁用卡牌
        //添加至弃牌堆
        FightCardManager.Instance.usedCardList.Add(item.data["Id"]);
        //更新使用后的卡牌数量
        nocardCountTxt.text = FightCardManager.Instance.usedCardList.Count.ToString();
        //从集合中删除
        cardItemList.Remove(item);
        //刷新卡牌位置
        UpdateCardItemPos();
        //卡牌移到弃牌堆
        item.GetComponent<RectTransform>().DOAnchorPos(new Vector2(1000, -30), 0.25f);
        item.transform.DOScale(0, 0.25f);
        Destroy(item.gameObject, 1);

    }
    //删除所有卡牌
    public void RemoveAllCard()
    {
        for(int i = cardItemList.Count - 1; i >= 0; i--)
        {
            RemoveCard(cardItemList[i]);
        }
    }
}
