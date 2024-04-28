using System.Collections;
using UnityEngine;

public class LuckyFlower : Plant
{
  float AddGoldTime = 10f;//:每10秒增加一次金币
  public GameObject GoldPrefab;//预制体
  public float jumpMinDistance = 1f;//跳跃的最小距离
  public float jumpMaxDistance = 1.3f;//跳跃的最大距离

  int gold = 0;
  protected override void OnEnable()
  {
    base.OnEnable();
    gold = 0;
  }
  protected override void EnableUpdate()
  {
    StartCoroutine(AddGold());
  }
  IEnumerator AddGold()
  {
    while (AlterHP > 0)
    {
      yield return new WaitForSeconds(AddGoldTime);
      ProduceGold();

    }
  }
  void ProduceGold()//生产金币
  {
    Gold Gold = BufferPoolManager.Instance.GetObj(GoldManger.Instance.GoldPrefab).GetComponent<Gold>();
    Gold.transform.position = this.transform.position;
    float ditance = Random.Range(jumpMinDistance, jumpMaxDistance);
    ditance = Random.Range(0, 2) < 1 ? -ditance : ditance;//0向左移动,1向右移动
    Vector3 position = transform.position;
    position.x += ditance;

    Gold.GetComponent<Gold>().JumpTo(position);

  }
  public override void Die()
  {
    StopAllCoroutines();
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.LuckyFlower], this.gameObject);
  }
}