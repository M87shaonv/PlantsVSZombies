using UnityEngine;

public class SunFlower : Plant
{
  public float produceTime = 5f;//生产阳光的时间
  private float produceTimer = 0;//生产阳光的计时器
  public GameObject sunPerfab;//阳光预制体
  public float jumpMinDistance = 1f;//跳跃的最小距离
  public float jumpMaxDistance = 1.3f;//跳跃的最大距离
  void OnEnable()
  {
    AlterHP = HP;
    TransToDisable();//默认禁用状态
    produceTimer = 0;
  }
  void Awake()
  {
    anim = GetComponent<Animator>();
  }
  protected override void EnableUpdate()
  {
    produceTimer += Time.deltaTime;
    if (produceTimer >= produceTime)
    {
      anim.SetTrigger("isglowing");
      produceTimer = 0;
    }
  }
  /// <summary>
  /// 生产阳光
  /// </summary>
  public void ProduceSun()
  {
    Sun sun = BufferPoolManager.Instance.GetObj(sunPerfab).GetComponent<Sun>();//从对象池中获取对象
    //GameObject sun = GameObject.Instantiate(sunPerfab, transform.position, Quaternion.identity);
    sun.transform.position = this.transform.position;//太阳花自身位置
    //阳光的随机跳跃
    float ditance = Random.Range(jumpMinDistance, jumpMaxDistance);
    ditance = Random.Range(0, 2) < 1 ? -ditance : ditance;//0向左移动,1向右移动
    Vector3 position = transform.position;
    position.x += ditance;

    sun.GetComponent<Sun>().JumpTo(position);
  }
  public override void Die()
  {
    base.Die();
    produceTimer = 0;
    AlterHP = HP;//死亡时恢复生命值
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[0], this.gameObject);
  }
}
