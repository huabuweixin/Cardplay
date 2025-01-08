using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// win
public class Fight_Win : FightUnit
{
    public override void Init()
    {
        //Debug.Log("游戏胜利");
        FightManager.Instance.StopAllCoroutines();
        UIManager.Instance.ShowTip("游戏胜利,3秒后跳转界面", Color.red);
        FightManager.Instance.StartCoroutine(ShowLoginUIWithDelay());
    }

    private IEnumerator ShowLoginUIWithDelay()
    {
        // 等待3秒
        yield return new WaitForSeconds(3f);

        // 3秒后切换到名为"Game"的场景
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene("Game");
    }
}
