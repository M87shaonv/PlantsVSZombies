using UnityEngine;

public class FollowerZombie : Zombie
{
  public float ArmraiseTime = 5f;
  public float ArmraiseTimer;
  protected override void OnEnable()
  {
    base.OnEnable();
    ArmraiseTimer = 0;
  }

  protected override void FixedUpdate()
  {
    ArmraiseTimer += Time.deltaTime;
    if (ArmraiseTimer >= ArmraiseTime)
    {
      PlayAraise();
      ArmraiseTimer = 0;
    }
    base.FixedUpdate();

  }

  void PlayWalk()//帧事件调用
  {
    anim.Play("Walk");
    zombieState = ZombieState.Move;
    ArmraiseTimer = 0;
  }
  void PlayAraise()
  {
    anim.Play("Armraise");
    zombieState = ZombieState.Pause;
  }

  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 3));
  }
}