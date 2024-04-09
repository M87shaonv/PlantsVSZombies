using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SettingUI : MonoBehaviour
{
  public GameObject setting;
  public List<GameObject> UI;
  public Text zombieText;

  public void PauseGame()
  {
    if (GameManger.Instance.isGameOverend) return;//游戏胜利或失败后无法暂停
    if (setting.activeSelf)
    {
      ResumeGame();
      RamdomText();
      return;
    }
    AudioManger.Instance.PlayClip(Config.pause);
    Time.timeScale = 0;
    setting.SetActive(true); // 显示设置界面
  }
  public void ResumeGame()
  {
    Time.timeScale = 1;
    setting.SetActive(false); // 隐藏设置界面
  }

  public void RestartGame()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 重新加载当前场景
  }
  public void QuitGame()
  {
    SceneManager.LoadScene(1);// 加载主菜单场景
  }
  String[] texts = new string[]{
    "暂停?\n你打的什么坏主意",
    "感受到僵尸大军的压迫了是吗\n那就铲掉所有植物吧!",
    "Brian!!!",
    "如果不是因为被困在这里,\n我早就去见我的宝贝了",
    "你以为我什么都不知道吗？\n我可是个僵尸！",
    "相信我,僵尸不会说谎\n你的大脑就像一团浆糊一样难吃",
    "活着真难.....\n所以快让我们来吃掉你吧",
    "日复一日机械的劳作\n和僵尸真的有什么区别吗？",
    "喂喂喂\n别一直暂停",
    "我知道这游戏做的很烂\n但你先别急着暂停",
    };
  public void RamdomText()
  {
    int randomText = UnityEngine.Random.Range(0, 10);
    zombieText.text = texts[randomText];
  }

  public void reconfirmRestart()// 再次确认重新开始
  {
    UI[0].SetActive(true); // 显示重新开始确认界面
    UI[1].SetActive(true);//显示restart提示
    UI[3].SetActive(true);
    UI[4].SetActive(true);//重新开始按钮
    UI[6].SetActive(true);
    UI[7].SetActive(true);//重新开始返回文本
  }
  public void reconfirmExit()// 再次确认退出
  {
    UI[0].SetActive(true); // 显示重新开始确认界面
    UI[2].SetActive(true);//显示exit提示
    UI[3].SetActive(true);
    UI[5].SetActive(true);//退出按钮
    UI[6].SetActive(true);
    UI[8].SetActive(true);//退出返回文本
  }
  public void backfirmRestart()
  {
    UI[0].SetActive(false);
    UI[1].SetActive(false);
    UI[3].SetActive(false);
    UI[4].SetActive(false);
    UI[6].SetActive(false);
    UI[7].SetActive(false);

  }
  public void backfirmExit()
  {
    UI[0].SetActive(false);
    UI[2].SetActive(false);
    UI[3].SetActive(false);
    UI[5].SetActive(false);
    UI[6].SetActive(false);
    UI[8].SetActive(false);
  }
}
