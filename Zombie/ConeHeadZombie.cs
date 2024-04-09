using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ConeHeadZombie : Zombie
{
  private GameObject zombiePerfab;//僵尸预制体

  void Awake()
  {
    zombiePerfab = Resources.Load<GameObject>("Perfabs/Zombie/Zombie0");
  }

  override public void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    GetComponent<SpriteRenderer>().color = Color.white;
    StartCoroutine(ChangeColor());
    this.currentHP -= damage;
    if (currentHP <= 0)
    {
      currentHP = -1;
      Dead();
    }
    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    if (hppercent < 0.3f)//如果生命值低于30%,就更新为普通僵尸
    {
      //GameObject newZombie = Instantiate(zombiePerfab, transform.position, Quaternion.identity);
      GameObject newZombie = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[0]);
      newZombie.transform.position = transform.position;
      //Destroy(this.gameObject);
      ZombieEvent.Instance.OnZombieExited(Row, this);//@通知管理器僵尸离开该行
      BufferPoolManager.Instance.PushObj(ZombieManger.Instance.zombieTypeList[2], this.gameObject);

      ZombieManger.Instance.zombies.Add(newZombie.GetComponent<Zombie>());
    }
  }
}
