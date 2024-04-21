using UnityEngine;
//:进入倒着走，碰到植物进入walk
//:抬手，然后抬眼镜召唤僵尸
public class DiscoZombie : Zombie
{
  public Transform[] summonPoints;
  public float height;
  public float weight;
  Vector2 Size;
  bool isWalkBackward = true;//是否倒着走
  float SummonInterval = 15;//召唤僵尸间隔
  public float SummonTimer = 0;//召唤僵尸计时器
  int order = 1;//控制僵尸排序
  bool isEating = false;//是否正在吃东西
  protected override void OnEnable()
  {
    base.OnEnable();
    Size = new Vector2(height, weight);
    isWalkBackward = true;
    SummonTimer = 0;
    order = 1;
    isEating = false;
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
  protected override void FixedUpdate()
  {
    if (isWalkBackward)
    {
      RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Size, 0, Vector2.left);
      foreach (var hit in hits)
      {
        if (hit.collider.CompareTag("Plant"))
        {
          PlayRaiseGlasses();//:播放动画的同时,帧事件调用SummonZombie
          isWalkBackward = false;
          return;
        }
      }
    }
    SummonTimer += Time.deltaTime;
    if (SummonTimer >= SummonInterval)
    {
      if (!isEating)
        PlayRaiseGlasses();

      SummonTimer = 0;
    }
    base.FixedUpdate();
  }
  protected override void EatUpdate()
  {
    isEating = true;
    base.EatUpdate();
  }
  protected override void MoveUpdate()
  {
    isEating = false;
    base.MoveUpdate();
  }
  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 3f));
  }

  #region 帧事件调用
  void PlayWalk()
  {
    anim.Play("Walk");
    SummonInterval = 0;
  }
  void PlayRaiseGlasses()
  {
    anim.Play("RaiseGlasses");
  }
  void SummonZombie()
  {
    for (int i = 0; i < summonPoints.Length; ++i)
    {
      GameObject follower = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[(int)ZombieTypes.FollowerZombie]);
      follower.transform.position = summonPoints[i].position;
      follower.GetComponent<SpriteRenderer>().sortingOrder += (-i + order);
    }
    ++order;
  }
  #endregion
}