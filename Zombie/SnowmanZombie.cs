using UnityEngine;

public class SnowmanZombie : Zombie
{
  protected override void OnEnable()
  {
    isPush = true;
    this.GetComponent<Collider2D>().enabled = true;
    zombieState = ZombieState.Move;
    currentHP = HP;
    currentPlant = null;
    attackTimer = 0;
    AlterMoveSpeed = moveSpeed;
  }

  public override void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    this.currentHP -= damage;
    GetComponent<SpriteRenderer>().color = Color.white;
    StartCoroutine(ChangeColor());
    if (currentHP <= 0)
    {
      Dead();
    }

    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    anim.SetFloat("HPpercent", hppercent);
  }
  protected override void FixedUpdate()
  {
    switch (zombieState)
    {
      case ZombieState.Move:
        MoveUpdate();
        break;
      case ZombieState.Eat:
        EatUpdate();
        break;
      case ZombieState.Die:
        DeadMove();
        break;
      default:
        break;
    }
  }

  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[zombieType], this.gameObject, 6));
  }
  void DeadMove()
  {
    rigid.MovePosition(rigid.position + Vector2.right * AlterMoveSpeed * Time.deltaTime);
    spriteRenderer.color = new Color32(220, 220, 220, 255);
    AlterMoveSpeed = 3;
  }
}

