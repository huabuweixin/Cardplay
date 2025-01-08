using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private Transform canvasTf;//画布变换组件
    private List<UIBase> uiList;//存储加载过的界面
    private void Awake()
    {
        Instance = this;
        canvasTf = GameObject.Find("Canvas").transform;
        uiList = new List<UIBase>();
    }
    //显示
    public UIBase ShowUI<T>(string uiName) where T:UIBase
    {
        UIBase ui = Find(uiName);
        if (ui == null)
        {
            GameObject obj = Instantiate(Resources.Load("UI/" + uiName), canvasTf) as GameObject;//从资源文件夹中寻找UI
            obj.name = uiName;
            ui = obj.AddComponent<T>();
            uiList.Add(ui);
        }
        else
        {
            ui.Show();
        }
        return ui;

    }
    //隐藏
    public void HideUI(string uiName)
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            ui.Hide();
        }
    }
    //关闭所有界面
    public void CloseAllUI()
    {
        for(int i = uiList.Count - 1; i >= 0; i--)
        {
            Destroy(uiList[i].gameObject);
        }
        uiList.Clear();
    }
    //关闭
    public void CloseUI(string uiName)
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            uiList.Remove(ui);
            Destroy(ui.gameObject);
        }
    }
    //找到名字对应的界面
    public UIBase Find(string uiName)
    {
        for(int i = 0; i < uiList.Count; i++)
        {
            if (uiList[i].name == uiName)
            {
                return uiList[i];
            }
        }
        return null;
    }
    //获得界面
    public T GetUI<T>(string uiName) where T : UIBase
    {
        UIBase ui = Find(uiName);
        if (ui != null)
        {
            return ui.GetComponent<T>();
        }
        return null;
    }
    //创建敌人头部的行动图标
    public GameObject CreateActionIcon()
    {
        GameObject obj = Instantiate(Resources.Load("UI/actionIcon"), canvasTf) as GameObject;
        obj.transform.SetAsFirstSibling();//设置在父级的第一位
        return obj;
    }
    //创建敌人底部的血量物体
    public GameObject CreateHpItem()
    {
        GameObject obj = Instantiate(Resources.Load("UI/HpItem"), canvasTf) as GameObject;
        obj.transform.SetAsFirstSibling();//设置在父级的第一位
        return obj;
    }
    //提示界面
    public void ShowTip(string msg,Color color,System.Action callback = null)
    {
        GameObject obj = Instantiate(Resources.Load("UI/Tips"), canvasTf) as GameObject;
        Text text= obj.transform.Find("bg/Text").GetComponent<Text>();
        text.color = color;
        text.text = msg;
        Tween scale1 = obj.transform.Find("bg").DOScale(1, 0.4f);
        Tween scale2 = obj.transform.Find("bg").DOScale(0, 0.4f);
        Sequence seq = DOTween.Sequence();
        seq.Append(scale1);
        seq.AppendInterval(0.5f);
        seq.Append(scale2);
        seq.AppendCallback(delegate ()
        {
            if (callback != null)
            {
                callback();
            }
        });
        MonoBehaviour.Destroy(obj, 2);
    }
}
