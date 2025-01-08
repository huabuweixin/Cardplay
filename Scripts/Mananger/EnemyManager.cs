using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager 
{
    public static EnemyManager Instance = new EnemyManager();
    private List<Enemy> enemyList;//存储战斗中的敌人

    public void LoadRes(string id)
    {
        enemyList = new List<Enemy>();
        //Id	Name	EnemyIds	Pos	
        //10003	3	10001=10002=10003	3,0,1=0,0,1=-3,0,1	
        Dictionary<string, string> levelData = GameConfigManager.Instance.GetLevelById(id);

        string[] enemyIds = levelData["EnemyIds"].Split('=');
        string[] enemyPos = levelData["Pos"].Split('=');//敌人位置信息
        for(int i = 0; i < enemyIds.Length; i++)
        {
            string enemyId = enemyIds[i];
            string[] posArr = enemyPos[i].Split(',');
            //敌人位置
            float x = float.Parse(posArr[0]);
            float y = float.Parse(posArr[1]);
            float z = float.Parse(posArr[2]);
            Dictionary<string, string> enemyData = GameConfigManager.Instance.GetEnemyById(enemyId);

            GameObject obj = Object.Instantiate(Resources.Load(enemyData["Model"])) as GameObject;//加载模型
            Enemy enemy = obj.AddComponent<Enemy>();
            enemy.Init(enemyData);//存储敌人信息
            enemyList.Add(enemy);//存储至集合
            obj.transform.position = new Vector3(x, y, z);
            obj.layer = LayerMask.NameToLayer("Enemy");
        }
        
    }
    //移除
    public void DeleteEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
        if (enemyList.Count == 0)
        {
            FightManager.Instance.ChangeType(FightType.Win);
        }
    }
    //执行怪物行为
    public IEnumerator DoAllEnemyAction()
    {
        for(int i = 0; i < enemyList.Count; i++)
        {
            yield return FightManager.Instance.StartCoroutine(enemyList[i].DoAction());
        }
        //更新敌人行为
        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetRandomAction();
        }
        //切换到玩家回合
        FightManager.Instance.ChangeType(FightType.Player);
    }
}
