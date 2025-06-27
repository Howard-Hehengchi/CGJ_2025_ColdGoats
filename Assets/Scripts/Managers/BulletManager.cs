using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    [SerializeField] Bullet bulletPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantiateBullet(Vector2 position, Vector2 direction)
    {
        var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.Init(direction);
    }
}
