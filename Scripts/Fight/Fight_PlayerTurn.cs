using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_PlayerTurn : FightUnit
{
    public override void Init()
    {
        //Debug.Log("player time");
        UIManager.Instance.ShowTip("玩家回合", Color.green, delegate ()
        {
            //恢复能量
            FightManager.Instance.CurPowerCount = 3;
            UIManager.Instance.GetUI<FightUI>("FightUI").UpdatePower();
            //卡堆内无卡重新初始化
            if (FightCardManager.Instance.HasCard() == false)
            {
                FightCardManager.Instance.Init();
                UIManager.Instance.GetUI<FightUI>("FightUI").UpdateUsedCardCount();
            }
        UIManager.Instance.GetUI<FightUI>("FightUI").CreateCardItem(4);//抽4张
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardItemPos();
        //更新卡牌数
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateCardCount();
        });
    }
    public override void OnUpdate()
    {
        
    }
}
