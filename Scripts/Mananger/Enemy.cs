using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
//敌人行动枚举
public enum ActionType
{
    None,
    Defend,//防御
    Attack,//攻击
}
//敌人脚本
public class Enemy : MonoBehaviour
{
    protected Dictionary<string, string> data;//敌人数据信息表
    public ActionType type;
    public GameObject hpItemObj;
    public GameObject actionObj;
    //ui相关
    public Transform attackTf;
    public Transform defendTf;
    public Text defendTxt;
    public Text hpTxt;
    public Image hpImg;
    //数值相关
    public int Defend;
    public int Attack;
    public int MaxHp;
    public int CurHp;
    //组件相关
    SkinnedMeshRenderer _meshRenderer;
    public Animator ani;
    public void Init(Dictionary<string,string>data)
    {
        this.data = data;
    }
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        ani = transform.GetComponent<Animator>();
        type = ActionType.None;
        hpItemObj = UIManager.Instance.CreateHpItem();
        actionObj = UIManager.Instance.CreateActionIcon();
        attackTf = actionObj.transform.Find("attack");
        defendTf = actionObj.transform.Find("defend");
        defendTxt = hpItemObj.transform.Find("fangyu/Text").GetComponent<Text>();
        hpTxt = hpItemObj.transform.Find("hpTxt").GetComponent<Text>();
        hpImg = hpItemObj.transform.Find("fill").GetComponent<Image>();
        //设置血条 行动力位置
        hpItemObj.transform.position = Camera.main.WorldToScreenPoint(transform.position+Vector3.down*0.2f);
        actionObj.transform.position = Camera.main.WorldToScreenPoint(transform.Find("head").position);

        SetRandomAction();

        //初始化数值
        Attack = int.Parse(data["Attack"]);
        CurHp = int.Parse(data["Hp"]);
        MaxHp = CurHp;
        Defend = int.Parse(data["Defend"]);
        UpdateHp();
        UpdateDefend();
        //test
        //OnSelect();
    }
    //随机一个行动
    public void SetRandomAction()
    {
        int ran = Random.Range(0, 3);
        type = (ActionType)ran;
        switch (type)
        {
            case ActionType.None:
                break;
            case ActionType.Defend:
                attackTf.gameObject.SetActive(false);
                defendTf.gameObject.SetActive(true);
                break;
            case ActionType.Attack:
                attackTf.gameObject.SetActive(true);
                defendTf.gameObject.SetActive(false);
                break;
        }
    }
    //更新血量信息
    public void UpdateHp()
    {
        hpTxt.text = CurHp + "/" + MaxHp;
        hpImg.fillAmount = (float)CurHp / (float)MaxHp;
    }
    //更新防御信息
    public void UpdateDefend()
    {
        defendTxt.text = Defend.ToString();
    }
    //被攻击卡选中，显示红边
    public void OnSelect()
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.red);
    }
    //未选中
    public void OnUnSelect()
    {
        _meshRenderer.material.SetColor("_OtlColor", Color.black);
    }
    //受伤
    public void Hit(int val)
    {
        //先减护盾
        if (Defend >= val)
        {
            Defend -= val;

            ani.Play("hit", 0, 0);
        }
        else
        {
            val = val - Defend;
            Defend = 0;
            CurHp -= val;
            if (CurHp <= 0)
            {
                CurHp = 0;
                //播放死亡
                ani.Play("die");
                //移除
                EnemyManager.Instance.DeleteEnemy(this);
                Destroy(gameObject, 1);
                Destroy(actionObj);
                Destroy(hpItemObj);
            }
            else
            {
                //受伤
                ani.Play("hit", 0, 0);
            }
        }
        //刷新
        UpdateDefend();
        UpdateHp();
    }
    //隐藏行动标志
    public void HideAction()
    {
        attackTf.gameObject.SetActive(false);
        defendTf.gameObject.SetActive(false);
    }
    //执行敌人行动
    public IEnumerator DoAction()
    {
        HideAction();

        // 等待 0.5 秒
        yield return new WaitForSeconds(0.5f);

        // 根据类型执行不同的操作
        switch (type)
        {
            case ActionType.None:
                break;
            case ActionType.Defend:
                // 加防御
                Defend += 1;
                UpdateDefend();
                break;
            case ActionType.Attack:
                // 播放攻击动画
                ani.Play("attack");
                // 玩家掉血
                FightManager.Instance.GetPlayerHit(Attack);

                // 相机震动
                Camera.main.DOShakePosition(0.1f, 0.2f, 5, 45);
                break;
            default:
                break;
        }

        // 等待 1 秒后播放 idle 动画
        yield return new WaitForSeconds(1);
        ani.Play("idle");
    }


}
