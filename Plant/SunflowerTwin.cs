using UnityEngine;

public class SunflowerTwin : SunFlower
{
  public override void ProduceSun()
  {
    for (int i = 0; i < 2; ++i)
    {
      Sun sun = BufferPoolManager.Instance.GetObj(sunPerfab).GetComponent<Sun>();//从对象池中获取对象
      sun.transform.position = this.transform.position;//太阳花自身位置
                                                       //阳光的随机跳跃
      float ditance = Random.Range(jumpMinDistance, jumpMaxDistance);
      ditance = Random.Range(0, 2) < 1 ? -ditance : ditance;//0向左移动,1向右移动
      Vector3 position = transform.position;
      position.x += ditance;
      sun.GetComponent<Sun>().JumpTo(position);
    }
  }
  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.SunflowerTwin], this.gameObject);
  }
}