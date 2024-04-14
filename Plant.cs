using System.Collections;
using UnityEngine;

/// <summary>
/// 植物状态
/// </summary>
enum PlantState
{
  Disable,
  Enable,
}
public class Plant : MonoBehaviour
{
  public static Plant instance { get; private set; }
  PlantState plantState = PlantState.Disable;//默认禁用
  public PlantType plantType;
  public float HP = 100;//植物原血量
  public float AlterHP;//植物扣血的血量
  public int row;//植物所在的行
  protected Color origincolor;
  protected Animator anim;
  public float offsetX = 0; // X轴偏移量
  public float offsetY = 0; // Y轴偏移量
  public Cell thisCell;//当前植物所在的格子
  protected virtual void OnEnable()
  {
    AlterHP = HP;
    TransToDisable();//默认禁用状态
  }
  void Start()
  {
    anim = GetComponent<Animator>();
    origincolor = GetComponent<SpriteRenderer>().color;
    AlterHP = HP;
    instance = this;
    TransToDisable();//默认禁用状态
    transform.position += new Vector3(offsetX, offsetY, 0);
  }

  private void Update()
  {
    switch (plantState)
    {
      case PlantState.Disable:
        DisableUpdate();
        break;
      case PlantState.Enable:
        EnableUpdate();
        break;
      default:
        break;
    }
  }
  void DisableUpdate()
  {

  }
  /// <summary>
  /// protected表示受保护的，子类可以访问,外部类不可以访问
  /// </summary>
  protected virtual void EnableUpdate()
  {

  }

  /// <summary>
  /// 转换为禁用状态
  /// </summary>
  public void TransToDisable()
  {
    plantState = PlantState.Disable;
    GetComponent<Animator>().enabled = false;//禁用动画
    //! 因为cell和plant都有BoxCollider2D,植物的Collier把cell的Collier阻挡了,就会导致种植异常
    //所以需要在生成植物时将Collier关闭
    GetComponent<BoxCollider2D>().enabled = false;
    //TODO 利用这个特性这里可以制作一个植物或者物品,在玩家点击拖到僵尸前面,僵尸会停止移动开始eat,所以可以制作一个物品,比如多余的阳光,可以拖到僵尸前面,僵尸会吃掉阳光
  }
  /// <summary>
  /// 转换为启用状态
  /// </summary>
  public virtual void TransToEnable()
  {
    //@ 启用时将通知事件管理器生成了植物在哪一行
    ZombieEvent.Instance.OnPlantEntered(row, this);
    plantState = PlantState.Enable;
    GetComponent<Animator>().enabled = true;
    GetComponent<BoxCollider2D>().enabled = true;
  }
  /// <summary>
  /// 受到攻击
  /// </summary>
  public virtual void TakeDamage(int damage)
  {
    this.AlterHP -= damage;
    GetComponent<SpriteRenderer>().color = Color.white;

    StartCoroutine(ColorChange());
    if (this.AlterHP <= 0)
    {
      Die();
    }
  }
  /// <summary>
  /// 植物加血
  /// </summary>
  public void AddBlood(int blood)
  {
    this.AlterHP += blood;
    StartCoroutine(AddBloodEffect.instance.SpawnBloodEffect(transform, AddBloodEffect.instance.BloodEffect));
    if (this.AlterHP >= HP)
    {
      this.AlterHP = HP;
    }
  }

  public virtual void Die()
  {
    //@ 在死亡时将该植物从该行移除
    ZombieEvent.Instance.OnPlantExited(row, this);
    thisCell.RemovePlant();
  }
  bool isColorChanging = false; // 添加一个标志
  IEnumerator ColorChange()//受击高亮后的颜色恢复
  {
    if (!isColorChanging)
    {
      isColorChanging = true;
      yield return new WaitForSeconds(0.1f);
      GetComponent<SpriteRenderer>().color = origincolor;
      isColorChanging = false;
    }
  }
  void OnMouseEnter()
  {
    GetComponent<SpriteRenderer>().color = Color.white;
  }
  void OnMouseExit()
  {
    GetComponent<SpriteRenderer>().color = origincolor;
  }
  public void TransToPause()
  {
    plantState = PlantState.Disable;
    anim.enabled = false;
  }
}
