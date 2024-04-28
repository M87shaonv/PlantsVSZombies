using DG.Tweening;
using UnityEngine;
//:20秒内未死亡,将丢出10个小鬼僵尸
public class GiantGreenZombie : Zombie
{
  public Transform ThrowPoint;
  float ThrowTime = 20;//开始受到伤害丢出小鬼僵尸的抛掷时间
  public float ThrowTimer;
  int order = 1;//控制小鬼僵尸的order
  bool isThrow = false;//是否丢出小鬼僵尸
  protected override void OnEnable()
  {
    base.OnEnable();
    ThrowTimer = 0;
    order = 1;
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
  protected override void MoveUpdate()
  {
    if (isPush)
    {
      //:0\1\2\3\4
      if (Row < 2)
      {
        ZombieEvent.Instance.OnZombieEntered(Row, this);
        ZombieEvent.Instance.OnZombieEntered(Row + 1, this);
        ZombieEvent.Instance.OnZombieEntered(Row + 2, this);
      }
      if (Row == 3)
      {
        ZombieEvent.Instance.OnZombieEntered(Row, this);
        ZombieEvent.Instance.OnZombieEntered(Row + 1, this);
      }
      isPush = false;
    }

    rigid.MovePosition(rigid.position + Vector2.left * AlterMoveSpeed * Time.deltaTime);
  }
  protected override void FixedUpdate()
  {
    ThrowTimer += Time.deltaTime;
    if (currentHP > 0 && !isThrow)
    {
      anim.Play("Throw");
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

  #region 在帧事件中调用，用于处理动画事件
  void ThrowZombie()
  {
    GameObject LitterGhostZombie = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[(int)ZombieTypes.LitterGhostGreenZombie]);
    LitterGhostZombie.transform.position = ThrowPoint.position;
    LitterGhostZombie.GetComponent<Zombie>().Row = this.Row;
    LitterGhostZombie.GetComponent<LitterGhostGreenZombie>().spriteRenderer.sortingOrder += order;
    ++order;
  }
  void PlayNoLitterGhostWalk()
  {
    anim.Play("NoLitterGhost_Walk");
    if (order > 4)//:第一次抛出3只
    {
      isThrow = true;
    }
  }
  #endregion
  protected override void EatUpdate()
  {

    attackTimer += Time.deltaTime;
    if (attackTimer >= attackInterval && currentPlant != null)
    {
      currentPlant.TakeDamage(attack);
      attackTimer = 0;
    }
  }
  public override void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    if (ThrowTimer >= ThrowTime)
      anim.Play("Throw");//:在20秒后受到一次伤害就丢一个小鬼

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
    if (Row < 2)
    {
      //:base.Dead()里面调用过
      //ZombieEvent.Instance.OnZombieExited(Row, this);
      ZombieEvent.Instance.OnZombieExited(Row + 1, this);
      ZombieEvent.Instance.OnZombieExited(Row + 2, this);
    }
    if (Row == 3)
    {
      ZombieEvent.Instance.OnZombieExited(Row + 1, this);
    }
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 4));
  }

}