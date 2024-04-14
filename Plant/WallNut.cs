using UnityEngine;

public class WallNut : Plant
{
  private float percent;//血量百分比
  protected override void OnEnable()
  {

    base.OnEnable();
  }
  void Awake()
  {
    anim = GetComponent<Animator>();
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
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantType.WallNut], this.gameObject);
  }
}
