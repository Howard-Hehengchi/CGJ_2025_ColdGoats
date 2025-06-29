using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Rigidbody2D body2D;

    private Vector2 input;

    [SerializeField] float speed = 5f;
    [SerializeField] Transform bulletSpawnTF;

    [SerializeField] float shootInterval = 0.5f;
    private float shootTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        shootTimer = shootInterval;
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if(Input.GetMouseButton(0))
        {
            shootTimer += Time.deltaTime;
            if(shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                Shoot();
            }
        }
        else
        {
            shootTimer = shootInterval;
        }
    }

    private void Shoot()
    {
        SFXManager.Instance.PlayGunFireSFX();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        BulletManager.Instance.InstantiateBullet(bulletSpawnTF.position, direction);
    }

    private void FixedUpdate()
    {
        body2D.velocity = input * speed;
    }
}
