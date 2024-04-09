using UnityEngine;

public class WallNut : Plant
{
  private float percent;//血量百分比
  void OnEnable()
  {
    AlterHP = HP;
    TransToDisable();//默认禁用状态
  }
  void Awake()
  {
    anim = GetComponent<Animator>();
    AlterHP = HP;
  }
  protected override void EnableUpdate()
  {
    SwitchState();
  }
  /// <summary>
  /// 更新血量百分比,判断是否切换动画
  /// </summary>
  void SwitchState()
  {
    percent = AlterHP / HP;
    anim.SetFloat("BloodPercent", percent);
  }
  public override void Die()
  {
    base.Die();
    AlterHP = HP;//死亡时恢复生命值
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[3], this.gameObject);
  }
}
