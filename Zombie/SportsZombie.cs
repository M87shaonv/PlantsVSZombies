using System.Collections;
using UnityEngine;
//:前方有植物便跳过,然后转换为Walk_AfterJump
//:任何状态都可以切换为Die
public class SportsZombie : Zombie
{
  bool isJumping = false;//是否跳跃过
  public float Range = 1;//射线检测范围
  protected override void OnEnable()
  {
    base.OnEnable();
    isJumping = false;
  }
  protected override void FixedUpdate()
  {
    if (!isJumping)
    {
      Vector2 direction = new Vector2(transform.position.x, transform.position.y - 0.5f);
      RaycastHit2D[] Hits = Physics2D.RaycastAll(direction, -transform.right, Range);
      foreach (RaycastHit2D Hit in Hits)
      {
        if (Hit.collider.CompareTag("Plant"))
        {
          Transform target = Hit.collider.transform;
          if (this.transform.position.x < target.position.x + 1)
          {
            Jump();
            return;
          }
        }
      }
      MoveUpdate();
    }
    base.FixedUpdate();
  }
  void Jump()
  {
    anim.Play("Jump");//播放跳跃动画
    isJumping = true;
    StartCoroutine(AfterJump());
  }
  IEnumerator AfterJump()
  {
    this.GetComponent<BoxCollider2D>().enabled = false;
    yield return new WaitForSeconds(1.1f);
    this.GetComponent<BoxCollider2D>().enabled = true;
    JumpMove();
    AlterMoveSpeed /= 2;//减速
    anim.Play("Walk_AfterJump");
  }
  public float move = 2.5f;
  public void JumpMove()//播放跳跃动画后瞬间向前移动
  {
    this.transform.Translate(Vector3.left * move, this.transform);
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
    anim.Play("Die");
    AlterMoveSpeed = 0;
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 1.5f));
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
