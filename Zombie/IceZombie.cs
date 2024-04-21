using DG.Tweening;
using UnityEngine;

public class IceZombie : Zombie
{
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Cell"))
    {
      other.GetComponent<Cell>().DisableCell();
      //Spriterender切换成ice
      Sprite Icesprite = other.GetComponent<Cell>().Ice.GetComponent<SpriteRenderer>().sprite;
      other.GetComponent<SpriteRenderer>().sprite = Icesprite;
    }
    if (other.CompareTag("Plant"))
    {
      currentPlant = other.GetComponent<Plant>();//获取触碰到的植物对象的Palnt脚本
      currentPlant.Die();
    }

    if (other.CompareTag("Door"))
    {
      transform.DOMoveY(-0.5f, 2);//移动到门口
      GameManger.Instance.GameOverFail();//摄像机移动
    }
  }
  void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Plant"))
    {
      anim.SetBool("isEating", false);
      zombieState = ZombieState.Move;
      currentPlant = null;//离开植物对象时，将当前吃植物的对象设置为空
      attackTimer = 0;//攻击计时器归零
    }
  }

  #region 在动画帧事件中调用的函数
  public void PlayBlast()
  {
    anim.Play("Blast");
  }
  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 3f));
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
}