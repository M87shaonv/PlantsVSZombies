using UnityEngine;

public class TallWallNut : WallNut
{
  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantType.TallWallNut], this.gameObject);
  }
}