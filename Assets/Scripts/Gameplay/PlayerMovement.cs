using NaughtyAttributes;
using Scripts.InputManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("General")] private float MovementSpeed;
    [SerializeField, BoxGroup("General")] private float RotationSpeed;

    [SerializeField, BoxGroup("MouseRotation")] private LayerMask ignoreLayers;
    [SerializeField, BoxGroup("MouseRotation")] private bool RotateTowardMouse = true;

    // Links
    private Camera _camera;
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
        _camera = MainCamera.Instance.Camera;

        if (_player.isLocalPlayer)
        {
            Destroy(this);
        }
    }
    private void LateUpdate()
    {
        var moveInputVector = InputManager.Controls.Game.Move.ReadValue<Vector2>();

        var targetVector = new Vector3(moveInputVector.x, 0, moveInputVector.y);
        var movementVector = MoveTowardTarget(targetVector);

        if (RotateTowardMouse) RotateFromMouseVector();
        else RotateTowardMovementVector(movementVector);
    }

    private void RotateFromMouseVector()
    {
        Ray ray = _camera.ScreenPointToRay(InputManager.Controls.Game.Point.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f, ~ignoreLayers))
        {
            Quaternion rotation = Quaternion.LookRotation(hitInfo.point);
            rotation.x = 0.0f;
            rotation.z = 0.0f;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
        }
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = MovementSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, _camera.gameObject.transform.rotation.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;
        return targetVector;
    }

    private void RotateTowardMovementVector(Vector3 movementDirection)
    {
        if (movementDirection.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed);
    }
}