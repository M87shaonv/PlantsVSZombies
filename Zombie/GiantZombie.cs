using DG.Tweening;
using UnityEngine;
//:进入为Walk状态,一定概率直接丢出小鬼僵尸
//:10秒内生命值小于600丢出小鬼僵尸
public class GiantZombie : Zombie
{
  public Transform ThrowPoint;
  public float ThrowTime = 10;
  public float ThrowTimer;
  bool isThrow = false;//是否丢出小鬼僵尸
  int random;

  protected override void OnEnable()
  {
    base.OnEnable();
    isThrow = false;
    ThrowTimer = 0;
    random = Random.Range(0, 11);
  }
  protected override void MoveUpdate()
  {
    if (!isThrow)
    {
      if (random == 0)
      {
        anim.Play("Throw");
        isThrow = true;
      }
    }

    if (isPush)
    {
      ZombieEvent.Instance.OnZombieEntered(Row, this);//@通知事件管理器生成了僵尸在哪一行
      isPush = false;
    }

    rigid.MovePosition(rigid.position + Vector2.left * AlterMoveSpeed * Time.deltaTime);
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Plant"))
    {
      anim.SetBool("isEating", true);
      zombieState = ZombieState.Eat;
      currentPlant = other.GetComponent<Plant>();//获取触碰到的植物对象的Palnt脚本
    }
    else if (other.CompareTag("FloorPlant"))//可以攻击地刺
    {
      anim.SetBool("isEating", true);
      zombieState = ZombieState.Eat;
      currentPlant = other.GetComponent<Plant>();//获取触碰到的植物对象的Palnt脚本
    }
    else if (other.CompareTag("Door"))
    {
      transform.DOMoveY(-0.5f, 2);//移动到门口
      GameManger.Instance.GameOverFail();//摄像机移动
    }
  }
  protected override void FixedUpdate()
  {
    if (ThrowTime <= 10 && !isThrow)
    {
      ThrowTimer += Time.deltaTime;
      if (currentHP < 500 && currentHP > 0)
      {
        anim.Play("Throw");
        isThrow = true;
      }
    }
    switch (zombieState)
    {
      case ZombieState.Move:
        MoveUpdate();
        break;
      case ZombieState.Eat:
        EatUpdate();
        break;
      case ZombieState.Die:
        break;
      default:
        break;
    }
  }
  protected override void EatUpdate()
  {

    attackTimer += Time.deltaTime;
    if (attackTimer >= attackInterval && currentPlant != null)
    {
      currentPlant.TakeDamage(attack);
      attackTimer = 0;
    }
  }
  #region 在帧事件中调用，用于处理动画事件
  void ThrowZombie()
  {
    GameObject LitterGhostZombie = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[(int)ZombieTypes.LitterGhostZombie]);
    LitterGhostZombie.transform.position = ThrowPoint.position;
    LitterGhostZombie.GetComponent<Zombie>().Row = ++this.Row;
    isThrow = true;
  }
  void PlayNoLitterGhostWalk()
  {
    anim.Play("NoLitterGhost_Walk");
  }
  #endregion

  public override void TakeDamage(int damage)
  {

    if (currentHP <= 0) return;
    this.currentHP -= damage;
    if (damage != 15)
    {
      GetComponent<SpriteRenderer>().color = Color.white;
      StartCoroutine(ChangeColor());
    }
    if (currentHP <= 0)
    {
      currentHP = -1;
      Dead();
    }
    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    anim.SetFloat("HPpercent", hppercent);
  }

  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 4));
  }
}