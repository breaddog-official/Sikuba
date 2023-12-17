using Scripts.InputManagement;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected float speed;

    [SerializeField] protected Player player;
    protected Camera mainCamera;

    private void Start()
    {
        player = GetComponent<Player>();
        mainCamera = Camera.main;
    }
    public void Move()
    {
        transform.position = Vector2.Lerp(transform.position, (Vector2)transform.position + InputManager.Controls.Game.Move.ReadValue<Vector2>(), Time.fixedDeltaTime * speed);
    }
    
}
