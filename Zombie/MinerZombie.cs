using System.Collections;
using DG.Tweening;
using UnityEngine;
//:进入为Spin状态,会阻挡所有的子弹,在遇到植物时进入Spin_Walk,之后进入Walk状态
public class MinerZombie : Zombie
{
  bool isSpin = true;//是否处于旋转状态
  public float Range = 1;//射线检测范围
  protected override void OnEnable()
  {
    base.OnEnable();
    isSpin = true;
  }

  protected override void FixedUpdate()
  {
    if (isSpin)
    {
      Vector2 direction = new Vector2(transform.position.x, transform.position.y - 0.5f);
      RaycastHit2D[] Hits = Physics2D.RaycastAll(direction, -transform.right, Range);
      foreach (RaycastHit2D Hit in Hits)
      {
        if (Hit.collider.CompareTag("Plant"))
        {
          isSpin = false;
          AlterMoveSpeed = 0;
          StartCoroutine(ChangeMoveSpeed());
          return;
        }
      }
    }
    base.FixedUpdate();
  }
  IEnumerator ChangeMoveSpeed()
  {
    anim.Play("Spin_After");
    yield return new WaitForSeconds(1.5f);
    AlterMoveSpeed = moveSpeed / 2;
    anim.Play("Walk");
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (isSpin)//旋转状态下子弹无效
      if (other.CompareTag("bullet"))
      {
        if (other.GetComponent<CornCannonBullet>() == null)//玉米加农炮有效
          currentHP = HP;
        else
          isSpin = false;
      }

    if (other.CompareTag("Plant"))
    {
      anim.SetBool("isEating", true);
      zombieState = ZombieState.Eat;
      currentPlant = other.GetComponent<Plant>();//获取触碰到的植物对象的Palnt脚本
    }

    if (other.CompareTag("Door"))
    {
      transform.DOMoveY(-0.5f, 2);//移动到门口
      GameManger.Instance.GameOverFail();//摄像机移动
    }
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
    isSpin = false;
    anim.Play("underground");
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 6));
  }
  void DeadMove()
  {
    spriteRenderer.color = new Color32(220, 220, 220, 255);
    AlterMoveSpeed = -5;
    MoveUpdate();
  }
#if TEXTING
  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Vector3 center = transform.position;  // 长方形的中心点，可以根据需求修改
    Vector3 size = new Vector3(Range, 1, 0);  // 长方形的尺寸，可以根据需求修改
    Gizmos.DrawWireCube(center, size);
  }
#endif
}