using Mirror;
using NTC.Pool;
using Scripts.Gameplay;
using Scripts.InputManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : NetworkBehaviour
{
    [SerializeField] private Bullet bulletPrafab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootDelay = 0.15f;

    private bool CanShoot = true;
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        InputManager.Controls.Game.Shoot.started += PreShoot;
    }
    private void OnDisable()
    {
        InputManager.Controls.Game.Shoot.started -= PreShoot;
    }
    [Client]
    private void PreShoot(InputAction.CallbackContext ctx)
    {
        if (!_player.isLocalPlayer && (NetworkManager.singleton ? NetworkManager.singleton.isNetworkActive : false)) return;
        Shoot();
    }
    [Command]
    private void Shoot()
    {
        if (!CanShoot) return;

        CanShoot = false;
        Invoke(nameof(SetCanShoot), shootDelay);
        Bullet bullet =  Instantiate(bulletPrafab, shootPoint.position, shootPoint.rotation);
        NetworkServer.Spawn(bullet.gameObject, connectionToClient);
    }
    private void SetCanShoot() => CanShoot = true;
}
