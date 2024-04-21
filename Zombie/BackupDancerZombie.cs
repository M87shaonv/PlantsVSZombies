using System.Collections;
using UnityEngine;

public class BackupDancerZombie : Zombie
{
  float DancingInterval = 5f;//跳舞间隔
  public float DancingTimer = 0f;//跳舞时间计时器
  bool isAppear = false;//是否出现
  protected override void OnEnable()
  {
    base.OnEnable();
    zombieState = ZombieState.Pause;
    DancingTimer = 0f;
    isAppear = false;
  }
  protected override void FixedUpdate()
  {
    if (isAppear)
    {
      DancingTimer += Time.deltaTime;
      if (DancingTimer >= DancingInterval)
      {

        StartCoroutine(PlayDancing());
        DancingTimer = 0;
      }
    }
    base.FixedUpdate();
  }

  /// <summary>
  /// $帧事件调用,仅出现时调用1次
  /// </summary>
  void PlayWalk()
  {
    anim.Play("Walk");
    zombieState = ZombieState.Move;
    isAppear = true;
  }

  IEnumerator PlayDancing()
  {
    anim.Play("Dancing");
    zombieState = ZombieState.Pause;

    yield return new WaitForSeconds(2f);
    anim.Play("Walk");
    zombieState = ZombieState.Move;
    DancingTimer = 0;
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