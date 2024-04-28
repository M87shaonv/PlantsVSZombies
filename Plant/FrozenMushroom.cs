using System.Collections;
using UnityEngine;

public class FrozenMushroom : Plant
{
  bool isFrozen = false;
  protected override void OnEnable()
  {
    GetComponent<SpriteRenderer>().enabled = true;
    GetComponent<BoxCollider2D>().enabled = true;
    base.OnEnable();
    isFrozen = false;
  }
  protected override void EnableUpdate()
  {
    if (!isFrozen)
      StartCoroutine(Frozen());
  }
  IEnumerator Frozen()
  {
    isFrozen = true;
    yield return new WaitForSeconds(2);
    foreach (Zombie zombie in ZombieManger.Instance.zombies)
    {
      zombie.FrozenZombie();
    }
    GetComponent<SpriteRenderer>().enabled = false;
    GetComponent<BoxCollider2D>().enabled = false;
    StartCoroutine(FrozenEnd());
  }
  IEnumerator FrozenEnd()
  {
    yield return new WaitForSeconds(5);
    foreach (Zombie zombie in ZombieManger.Instance.zombies)
    {
      zombie.FrozenedZombie();
    }
  }
  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.FrozenMushroom], this.gameObject);
  }
}