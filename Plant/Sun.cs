using UnityEngine;
using DG.Tweening;

public class Sun : MonoBehaviour
{
  public float moveduration = 1f;//阳光移动的持续时间
  public byte[] randomSun = new byte[10] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };//随机阳光值
  public GameObject thisSun;

  void OnEnable()//每次启用时调用
  {
    StartCoroutine(GameManger.Instance.WaitForSeconds(autoCollect, 4));

  }

  public void autoCollect()
  {
    thisSun.GetComponent<Sun>().OnMouseDown();
  }
  /// <summary>
  /// 控制阳光的移动
  /// </summary>
  public void LinearTo(Vector3 targetPos)
  {
    transform.DOMove(targetPos, 6).SetEase(Ease.OutSine);
  }
  /// <summary>
  /// 控制阳光的跳跃
  /// </summary>
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

  int t = 0;
  int random = 0;
  int[] rate = new int[] { 15, 5, 5, 10, 20, 20, 10, 5, 5, 5 };//随机阳光的权重
  /// <summary>
  /// 随机阳光
  /// </summary>
  public int randomIndex(int[] rate)
  {
    random = Random.Range(0, 101);
    for (int i = 0; i < rate.Length; i++)
    {
      t += rate[i];
      if (random <= t)
        return i;
    }
    return 0;
  }


  //! 这里注意设置阳光的z轴为-1,因为sun和cell都处在同一个z坐标,都通过OnMouseDown来检测,会导致互相干扰，导致无法正确地检测到鼠标点击事件
  /// <summary>
  /// 鼠标点击阳光时执行的事件
  /// </summary>
  public void OnMouseDown()
  {
    int index = randomIndex(rate);

    transform.DOMove(SunManger.Insance.GetSunPountTextPosition(), moveduration)
      .SetEase(Ease.OutQuad)
      .OnComplete(() =>
      {
        AudioManger.Instance.PlayClip(Config.getSun);
        //Destroy(this.gameObject);
        //放入对象池中
        BufferPoolManager.Instance.PushObj(SunManger.Insance.sunPerfab, this.gameObject);
        //取消自身的全部协程或延迟调用
        StopAllCoroutines();
        SunManger.Insance.AddSunPoint(randomSun[index]);
      }
      );
  }

}