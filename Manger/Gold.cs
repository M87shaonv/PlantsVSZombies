using DG.Tweening;
using UnityEngine;

public class Gold : MonoBehaviour
{
  public float moveduration = 1f;//金币移动的持续时间
  void OnEnable()//每次启用时调用
  {
    StartCoroutine(GameManger.Instance.WaitForSeconds(autoCollect, 4));
  }

  public void autoCollect()//自动收集金币
  {
    this.GetComponent<Gold>().OnMouseDown();
  }
  public void JumpTo(Vector3 targetPos)
  {
    Vector3 centerPos = (transform.position + targetPos) / 2;
    float distance = Vector3.Distance(transform.position, targetPos);

    centerPos.y += distance / 2;
    //PathType.CatmullRom表示曲线
    //Ease.OutQuad表示表示四阶缓动曲线,曲线在起始点和结束点处平滑度较高,中间部分平缓
    transform.DOPath(new Vector3[] { transform.position, centerPos, targetPos },
    moveduration, PathType.CatmullRom).SetEase(Ease.OutQuad);

  }
  public void OnMouseDown()
  {
    transform.DOMove(GoldManger.Instance.CalculateGoldBarTextPosition(), moveduration)
      .SetEase(Ease.OutQuad)
      .OnComplete(() =>
      {
        //AudioManger.Instance.PlayClip(Config.getSun);
        //放入对象池中
        BufferPoolManager.Instance.PushObj(GoldManger.Instance.GoldPrefab, this.gameObject);
        //取消自身的全部协程或延迟调用
        StopAllCoroutines();
        int gold = PlayerPrefs.GetInt("Gold");
        gold += 1;
        PlayerPrefs.SetInt("Gold", gold);
      }
      );
  }
}