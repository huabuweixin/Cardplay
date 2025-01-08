using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//开始界面
public class LoginUI : UIBase
{
    private void Awake()
    {
        //开始游戏
        Register("bg/startBtn").onClick = onStartGameBtn;
        //退出游戏
        Register("bg/quitBtn").onClick = onQuitGameBtn;
    }

    private void onQuitGameBtn(GameObject @object, PointerEventData data)
    {
        Application.Quit();
    }

    private void onStartGameBtn(GameObject obj,PointerEventData pData)
    {
        Close();
        //战斗初始化
        FightManager.Instance.ChangeType(FightType.Init);
    }
   
}
