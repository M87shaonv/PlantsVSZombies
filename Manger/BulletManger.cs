using UnityEngine;

public class BulletManger : MonoBehaviour
{
  public static BulletManger Instance { get; private set; }
  void Awake()
  {
    Instance = this;
  }

  public GameObject PeaBullet;
  public GameObject BigPeaBullet;
  public GameObject FirePeaBullet;
  public GameObject SnowPeaBullet;
  public GameObject CabbageBullet;
  public GameObject BasketballBullet;
  public GameObject CarambolaBullet;
  public GameObject MagicCatBullet;
  public GameObject CornCannonBullet;//玉米炮弹
}