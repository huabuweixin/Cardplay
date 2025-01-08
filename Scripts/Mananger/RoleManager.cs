using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用户信息管理器
public class RoleManager 
{
    public static RoleManager Instance = new RoleManager();
    public List<string> cardList;
    public void init()
    {
        cardList = new List<string>();
        cardList.Add("1000");
        cardList.Add("1000");
        cardList.Add("1000");
        cardList.Add("1000");

        cardList.Add("1001");
        cardList.Add("1001");
        cardList.Add("1001");
        cardList.Add("1001");

        cardList.Add("1002");
        cardList.Add("1002");
    }
}
