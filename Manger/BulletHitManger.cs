using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
/// <summary>
/// 子弹特效和其他特效管理器
/// </summary>
public class BulletHitManger : MonoBehaviour
{
  public static BulletHitManger Instance { get; private set; }
  void Awake()
  {
    Instance = this;
  }
  public GameObject PeaBulletHit;//豌豆子弹
  public GameObject FirePeaBulletHit;//火焰子弹
  public GameObject SnowPeaBulletHit;//寒冰子弹
  public GameObject CabbageBulletHit;//卷心菜子弹
  public GameObject BasketballBulletHit;//篮球车僵尸的子弹
  public GameObject BlastEffect;//joker僵尸爆炸特效
  public GameObject CherryBoom;//樱桃爆炸特效
  public GameObject CarambolaBulletHit;//杨桃子弹
  public GameObject CornCannonBulletHit;//玉米炮弹
  public GameObject ShellLandingTip;//炮弹击中点提示
  #region Blood effects for plants and zombies
  public GameObject BloodEffect;//植物加血特效
  public GameObject ZombieBloodEffect; //僵尸加血特效
  public Vector2 positionMin = new Vector2(-0.5f, -0.5f);
  public Vector2 positionMax = new Vector2(0.5f, 0.5f);

  public float sizeMin = 0.1f;//最小大小
  public float sizeMax = 0.6f;//最大大小
  public int objectsCount = 10;  // 要生成的对象数量
  /// <summary>
  /// 生成僵尸加血特效
  /// </summary>
  public IEnumerator SpawnZombieBloodEffect(Transform transform)
  {
    for (int i = 0; i < objectsCount; i++)
    {
      // 随机位置
      float x = Random.Range(transform.position.x + positionMin.x, transform.position.x + positionMax.x);
      float y = Random.Range(transform.position.y + positionMin.y, transform.position.y + positionMax.y);
      Vector2 randomPosition = new Vector2(x, y);
      // 随机大小
      float size = Random.Range(sizeMin, sizeMax);
      // 实例化对象并设置位置和大小
      GameObject obj = BufferPoolManager.Instance.GetObj(ZombieBloodEffect);
      obj.transform.position = randomPosition;
      obj.transform.localScale = new Vector2(size, size);
      StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieBloodEffect, obj, 0.5f));
      yield return new WaitForSeconds(0.1f);
    }
  }
  /// <summary>
  /// 生成植物加血特效
  /// </summary>
  public IEnumerator SpawnBloodEffect(Transform transform)
  {
    for (int i = 0; i < objectsCount; i++)
    {
      // 随机位置
      float x = Random.Range(transform.position.x + positionMin.x, transform.position.x + positionMax.x);
      float y = Random.Range(transform.position.y + positionMin.y, transform.position.y + positionMax.y);
      Vector2 randomPosition = new Vector2(x, y);
      // 随机大小
      float size = Random.Range(sizeMin, sizeMax);
      // 实例化对象并设置位置和大小
      GameObject obj = BufferPoolManager.Instance.GetObj(BloodEffect);
      obj.transform.position = randomPosition;
      obj.transform.localScale = new Vector2(size, size);
      StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BloodEffect, obj, 0.5f));
      yield return new WaitForSeconds(0.1f);
    }
  }
  public void PushEffect(GameObject gameobjectPerfab, GameObject gameobject, float time)
  {
    StartCoroutine(Effect(gameobjectPerfab, gameobject, time));
  }
  // 特效放入对象池
  IEnumerator Effect(GameObject gameobjectPerfab, GameObject gameobject, float time)
  {
    yield return new WaitForSeconds(time);
    BufferPoolManager.Instance.PushObj(gameobjectPerfab, gameobject);
  }
  #endregion


}