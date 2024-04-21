using Unity.VisualScripting;
using UnityEngine;

public class BalloonZombie : Zombie
{
  // 三种状态:
  // 0. 飞行状态,无法被普通植物攻击
  // 1. 爆炸状态,播放动画,停止移动
  // 2. 步行状态,可以被普通植物攻击
  //死亡和攻击状态的切换由Zombie类实现

  //子弹使用GameObject.FindWithTag查找它的SkyZombie标签
  //如果攻击到则转换ChangeObjectTag("Zombie");转换标签
  //:idle->pop->walk这三种状态的切换没有参数,直接代码切换
  //通过Play方法来直接切换到相应的动画状态：animator.Play("Jump");
  bool isPop;//气球是否爆炸,转换为地面状态

  protected override void OnEnable()
  {
    base.OnEnable();
    this.tag = "SkyZombie";
    ZombieEvent.Instance.AddZombie(this);
    isPop = false;
  }
  protected override void FixedUpdate()
  {
    if (!isPop)//如果气球没有爆炸,则飞行
    {
      SkyMove();
      return;
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

  void SkyMove()
  {
    if (currentHP != HP)//受到攻击
    {
      isPop = true;
      Pop();
      return;
    }
    rigid.MovePosition(rigid.position + Vector2.left * AlterMoveSpeed * Time.deltaTime);
  }

  void Pop()
  {
    PlayPop();
    StartCoroutine(GameManger.Instance.WaitForSeconds(PlayWalk, 1.1f));
  }

  void PlayPop()
  {
    anim.Play("BalloonZombie_Pop");
    this.rigid.simulated = false;
  }
  void PlayWalk()
  {
    anim.Play("BalloonZombie_Walk");
    this.rigid.simulated = true;
    ZombieEvent.Instance.RemoveZombie(this);
    this.tag = "Zombie";
    ZombieEvent.Instance.RemoveZombie(this);//从天空僵尸移除
    //ZombieManger.Instance.zombies.Add(this);//添加到地面僵尸
  }
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
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[zombieType], gameObject, 1.5f));//回收到对象池
  }
}

