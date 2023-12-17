using Mirror;
using NaughtyAttributes;
using Scripts.InputManagement;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerGun : MonoBehaviour
{
    [SerializeField, BoxGroup("Gun")] protected Transform gun;
    [SerializeField, BoxGroup("Gun")] protected Transform shootPoint;

    [SerializeField, BoxGroup("Bullet")] protected GameObject bullet;
    [SerializeField, BoxGroup("Bullet")] protected float cd;
    [SerializeField, BoxGroup("Bullet")] protected AudioSource shootSfx;
    protected bool canShoot = true;

    [SerializeField] protected Player player;
    protected Camera mainCamera;

    private void Start()
    {
        if (player == null)
            player = GetComponent<Player>();
        mainCamera = Camera.main;

        if (player.isLocalPlayer)
        {
            InputManager.Controls.Game.Shoot.started += ctx => Shoot();
            StartCoroutine(nameof(GunCd));
        }
    }
    private IEnumerator GunCd()
    {
        while (true)
        {
            if (!canShoot)
            {
                yield return new WaitForSeconds(cd);
                canShoot = true;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;

            NetworkServer.Spawn(Instantiate(bullet, shootPoint.position, gun.rotation), player.gameObject);
            shootSfx.Play();
        }
    }
    public void RotateGun()
    {
        Vector3 difference = mainCamera.ScreenToWorldPoint(InputManager.Controls.Game.Point.ReadValue<Vector2>()) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
