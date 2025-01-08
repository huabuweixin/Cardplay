using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗管理器
public enum FightType {
    None,
    Init,
    Player,
    Enemy,
    Win,
    Lose
}

public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    public FightUnit fightUnit;//战斗单元
    public int MaxHp;//最大血量
    public int CurHp;//当前血量
    public int MaxPowerCount;//最大能量
    public int CurPowerCount;//当前
    public int DefenseCount;//防御值
    public void Init()
    {
        MaxHp = 10;
        CurHp = 10;
        MaxPowerCount = 10;
        CurPowerCount = 3;
        DefenseCount = 3;
    }
    private void Awake()
    {
        Instance = this;
    }
    public void ChangeType(FightType type)
    {
        switch (type) {
            case FightType.None:
                break;
            case FightType.Init:
                fightUnit = new FightInit();
                break;
            case FightType.Player:
                fightUnit = new Fight_PlayerTurn();
                break;
            case FightType.Enemy:
                fightUnit = new Fight_EnemyTurn();
                break;
            case FightType.Win:
                fightUnit = new Fight_Win();
                break;
            case FightType.Lose:
                fightUnit = new Fight_Lose();
                break;
        }
        fightUnit.Init();//初始化

    }
    //玩家受伤
    public void GetPlayerHit(int hit)
    {
        //掉护盾
        if (DefenseCount >= hit)
        {
            DefenseCount -= hit;
        }
        else
        {
            hit = hit - DefenseCount;
            DefenseCount = 0;
            CurHp -= hit;
            if (CurHp <= 0)
            {
                CurHp = 0;
                //游戏失败
                ChangeType(FightType.Lose);
            }           
        }
        //更新
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateHp();
        UIManager.Instance.GetUI<FightUI>("FightUI").UpdateDefense();
    }
    private void Update()
    {
        if ((fightUnit!=null))
        {
            fightUnit.OnUpdate();
        }
    }

    internal void StartCoroutine(Func<IEnumerator> doAction)
    {
        // 使用 Unity 的 StartCoroutine 启动协程
        StartCoroutine(doAction());
    }



}
