using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Imitater : Plant
{
  protected override void EnableUpdate()
  {
    StartCoroutine(PlayExplore());
  }
  IEnumerator PlayExplore()
  {
    yield return new WaitForSeconds(2);
    anim.Play("Explore");
    yield return new WaitForSeconds(2);
    Initial();
  }
  void Initial()
  {
    this.Die();
    int zombier = Random.Range(0, ZombieManger.Instance.zombieTypeList.Count + 1);
    GameObject newZombie = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[zombier]);

    Transform create = GameObject.Find("ZombiePointList").transform.GetChild(row);
    newZombie.GetComponent<SpriteRenderer>().sortingOrder = create.GetComponent<SpriteRenderer>().sortingOrder + ZombieManger.Instance.order;
    newZombie.transform.position = create.position;

    ZombieManger.Instance.zombies.Add(newZombie.GetComponent<Zombie>());
    ZombieEvent.Instance.OnZombieEntered(row, newZombie.GetComponent<Zombie>());
    for (int i = 0; i < 5; ++i)
    {
      Sun sun = BufferPoolManager.Instance.GetObj(SunManger.Insance.sunPerfab).GetComponent<Sun>();//从对象池中获取对象
      sun.transform.position = this.transform.position;//S自身位置
                                                       //阳光的随机跳跃
      float ditance = Random.Range(0.2f, 1.5f);
      ditance = Random.Range(0, 2) < 1 ? -ditance : ditance;//0向左移动,1向右移动
      Vector3 position = transform.position;
      position.x += ditance;
      sun.GetComponent<Sun>().JumpTo(position);
    }
  }

  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.Imitater], this.gameObject);
  }
}