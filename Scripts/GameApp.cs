using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏入口
public class GameApp : MonoBehaviour
{

    void Start()
    {
        //初始化配置表
        GameConfigManager.Instance.Init();
        //初始化音频管理器
        AudioManager.Instance.Init();
        //用户信息
        RoleManager.Instance.init();
        //显示LoginUI
        UIManager.Instance.ShowUI<LoginUI>("LoginUI");
        AudioManager.Instance.PlayBGM("bgm1");
      
        string name = GameConfigManager.Instance.GetCardById("1001")["Des"];
        print(name);
    }

}
