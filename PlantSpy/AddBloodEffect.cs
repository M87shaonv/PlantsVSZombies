using System.Collections;
using UnityEngine;

public class AddBloodEffect : MonoBehaviour
{
  public GameObject BloodEffect;//植物加血特效
  public GameObject ZombieBloodEffect; //僵尸加血特效
  // 位置和大小的范围
  public Vector2 positionMin = new Vector2(-0.5f, -0.5f);
  public Vector2 positionMax = new Vector2(0.5f, 0.5f);
  public float sizeMin = 0.1f;
  public float sizeMax = 0.6f;

  // 要生成的对象数量
  public int objectsCount = 10;
  public static AddBloodEffect instance;
  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(this.gameObject);
    }
  }

  public IEnumerator SpawnBloodEffect(Transform transform, GameObject bloodEffect)
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
      GameObject obj = BufferPoolManager.Instance.GetObj(bloodEffect);
      obj.transform.position = randomPosition;
      obj.transform.localScale = new Vector2(size, size);
      StartCoroutine(BufferPoolManager.Instance.WaitAndPush(bloodEffect, obj, 0.5f));
      yield return new WaitForSeconds(0.05f);
    }
  }
}
