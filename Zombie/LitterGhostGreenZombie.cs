using UnityEngine;

public class LitterGhostGreenZombie : LitterGhostZombie
{
  protected override void OnEnable()
  {
    base.OnEnable();
    float random = Random.Range(2f, 4f);
    MoveOnParabola(transform.position, new Vector3(transform.position.x - random, transform.position.y, transform.position.z), 3, 2);
    StartCoroutine(AnimTOWalk());
  }
}