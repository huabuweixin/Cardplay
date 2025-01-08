using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    //注册监听事件
    public UIEventTrigger Register(string name)
    {
        Transform tf = transform.Find(name);
        return UIEventTrigger.Get(tf.gameObject);
    }
    //显示
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    //隐藏
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    //关闭界面
    public virtual void Close()
    {
        UIManager.Instance.CloseUI(gameObject.name);
    }
}
