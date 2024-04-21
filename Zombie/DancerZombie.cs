using System.Collections;
using UnityEngine;

public class DancerZombie : Zombie
{
  float SummonInterval = 5f; // 召唤僵尸的时间间隔
  float SummonTime = 0f; // 召唤僵尸的时间，会根据召唤次数改变
  public float SummonTimer = 0f; // 计时器
  public Transform[] SummonPos = new Transform[2]; // 召唤僵尸的位置
  public GameObject Ray;
  protected override void OnEnable()
  {
    base.OnEnable();
    SummonTimer = 0;
    SummonTime = SummonInterval;
    Ray.SetActive(false);
  }
  protected override void FixedUpdate()
  {
    //计时器，召唤僵尸
    SummonTimer += Time.deltaTime;
    if (SummonTimer >= SummonTime)
    {
      StartCoroutine(PlayDancing());
      SummonTimer = 0;
      SummonTime += 2;//召唤僵尸的时间间隔增加
    }
    base.FixedUpdate();
  }

  IEnumerator PlayDancing()
  {
    Ray.SetActive(true);
    anim.Play("Dancing");
    zombieState = ZombieState.Pause;

    yield return new WaitForSeconds(3f);
    anim.Play("SummonZombie");
    SummonZombie();

    yield return new WaitForSeconds(2f);
    Ray.SetActive(false);
    anim.Play("Walk");
    SummonTimer = 0;
    zombieState = ZombieState.Move;
  }

  /// <summary>
  /// 召唤僵尸
  /// </summary>
  void SummonZombie()
  {
    for (int i = 0; i < SummonPos.Length; i++)
    {
      GameObject Backup = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[(int)ZombieTypes.BackupDancerZombie]);
      Backup.transform.position = SummonPos[i].position;
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
    base.Dead();
    if (havehead)//是否掉头
    {
      havehead = false;
      GameObject head = BufferPoolManager.Instance.GetObj(zombieHead);
      head.transform.position = transform.position;
      BulletHitManger.Instance.PushEffect(zombieHead, head, 2f);
    }
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 2f));
  }
}