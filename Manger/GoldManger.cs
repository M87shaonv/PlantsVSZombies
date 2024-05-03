using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoldManger : MonoBehaviour
{
  public static GoldManger Instance;
  void Awake()
  {
    Instance = this;
  }

  public GameObject GoldPrefab;//金币预制体
  public Text text;//显示金币的UI
  public Transform GoldBar;//金币条的位置

  void Update()
  {
    text.text = PlayerPrefs.GetInt("Gold").ToString();
  }
  public Vector3 CalculateGoldBarTextPosition()
  {
    Vector3 position = Camera.main.ScreenToWorldPoint(GoldBar.transform.position);
    position.z = 0;
    position.x -= 1.4f;
    return position;
  }
}