using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreUI : MonoBehaviour
{
  public GameObject NextPage;
  public GameObject PrevPage;


  public void NextPageButton()//下一页按钮
  {
    AudioManger.Instance.PlayClip(Config.ButtonOnClick);
    PrevPage.SetActive(false);
    NextPage.SetActive(true);
  }
  public void PrevPageButton()//上一页按钮
  {
    AudioManger.Instance.PlayClip(Config.ButtonOnClick);
    PrevPage.SetActive(true);
    NextPage.SetActive(false);
  }
  public void ReturnMainMenu()
  {
    AudioManger.Instance.PlayClip(Config.ButtonOnClickTap);
    SceneManager.LoadScene(0);//返回主菜单
  }
}