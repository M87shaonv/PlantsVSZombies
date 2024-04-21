using System.Collections;
using UnityEngine;
//:增加报纸血量,报纸消失,僵直n秒
//:LostPaper的动画转换由代码直接控制
public class NewsPaperZombie : Zombie
{
  public int AlterAttack = 20;//丢失报纸后翻倍攻击力
  protected override void OnEnable()
  {
    base.OnEnable();
    haveNewsPaper = true;
  }

  bool haveNewsPaper = true;//是否有报纸
  float NewsPaperHP = 80;//报纸血量
  public override void TakeDamage(int damage)
  {
    if (NewsPaperHP > 0)
    {
      this.NewsPaperHP -= damage;
      return;
    }
    if (haveNewsPaper && NewsPaperHP <= 0)
    {
      StartCoroutine(LostPaper());
      haveNewsPaper = false;
    }
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

  IEnumerator LostPaper()
  {
    anim.Play("LostPaper");
    float move = AlterMoveSpeed;
    AlterMoveSpeed = 0;

    yield return new WaitForSeconds(2f);
    AlterMoveSpeed = move;
    anim.Play("LostPaper_Walk");
    attack += AlterAttack;//攻击力翻倍
    AlterMoveSpeed += AlterMoveSpeed * 2;//移动速度翻倍
  }
  public override void Dead()
  {
    base.Dead();
    if (havehead)//是否掉头
    {
      havehead = false;
      GameObject head = BufferPoolManager.Instance.GetObj(zombieHead);
      head.transform.position = transform.position;
      BulletHitManger.Instance.PushEffect(zombieHead, head, 1.5f);
    }
    attack -= AlterAttack;//恢复原攻击力
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 1.5f));
  }
}
