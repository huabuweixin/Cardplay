using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//攻击卡
public class AttackCardItem : CardItem, IPointerDownHandler
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }
    public override void OnDrag(PointerEventData eventData)
    {
        
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        
    }
    //按下
    public void OnPointerDown(PointerEventData eventData)
    {
        //播放声音
        AudioManager.Instance.PlayEffect("Cards/draw");
        //显示曲线界面
        UIManager.Instance.ShowUI<LineUI>("LineUI");
        //设置开始点位置
        UIManager.Instance.GetUI<LineUI>("LineUI").SetStartPos(transform.GetComponent<RectTransform>().anchoredPosition - new Vector2(50f, 0f));
        StopAllCoroutines();
        StartCoroutine(OnMouseDownRight(eventData));
    }
    IEnumerator OnMouseDownRight(PointerEventData pData)
    {
        while (true)
        {
            //再次按下
            if (Input.GetMouseButton(1))
            {
                break;
            }
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent.GetComponent<RectTransform>(),
                pData.position,
                pData.pressEventCamera,
                out pos))
            {
                //设置箭头位置
                UIManager.Instance.GetUI<LineUI>("LineUI").SetEndPos(pos);
                //射线检测
                CheckRayToEnemy();
            }
            yield return null;
        }
        UIManager.Instance.CloseUI("LineUI");
    }
    Enemy hitEnemy;
    private void CheckRayToEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 10000, LayerMask.GetMask("Enemy")))
        {
            hitEnemy = hit.transform.GetComponent<Enemy>();
            hitEnemy.OnSelect();
            if (Input.GetMouseButton(0))
            {
                StopAllCoroutines();

                UIManager.Instance.CloseUI("LineUI");
                if (TryUse() == true)
                {
                    PlayEffect(hitEnemy.transform.position);
                    //打击音效
                    AudioManager.Instance.PlayEffect("Effect/Sword");
                    //敌人受伤
                    int val = int.Parse(data["Arg0"]);
                    hitEnemy.Hit(val);
                }
                //未选中
                hitEnemy.OnUnSelect();
            }
        }
        else
        {
            //未射到
            if (hitEnemy != null)
            {
                hitEnemy.OnUnSelect();
                hitEnemy = null;
            }
        }
    }
}
