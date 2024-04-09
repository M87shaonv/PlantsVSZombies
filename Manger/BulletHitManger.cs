using UnityEngine;

public class BulletHitManger : MonoBehaviour
{
  public static BulletHitManger Instance { get; private set; }
  void Awake()
  {
    Instance = this;
  }
  public GameObject PeaBulletHit;
  public GameObject FirePeaBulletHit;
  public GameObject SnowPeaBulletHit;
  public GameObject CabbageBulletHit;
  public GameObject BasketballBulletHit;
}