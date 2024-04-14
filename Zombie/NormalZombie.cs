using UnityEngine;

public class NormalZombie : Zombie
{
  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[zombieType], gameObject, 1.5f));//回收到对象池
  }
}
