using Mirror;
using NaughtyAttributes;
using Scripts.InputManagement;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, BoxGroup("General")] private float MovementSpeed = 5.0f;
    [SerializeField, BoxGroup("General")] private float MovementAcceleration = 0.2f;
    [SerializeField, BoxGroup("General")] private float RotationSpeed = 2.0f;

    [SerializeField, BoxGroup("MouseRotation")] private LayerMask ignoreLayers;
    [SerializeField, BoxGroup("MouseRotation")] private bool RotateTowardMouse = true;

    private Camera _camera;
    private Player _player;
    private Rigidbody _rb;
    private Animator _anim;

    Vector3 targetMovementVector;

    Vector2 currentInputVector;
    Vector2 smoothInputVector;

    private void Start()
    {
        _player = GetComponent<Player>();
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponentInChildren<Rigidbody>();
        _camera = MainCamera.Instance.Camera;

        if (!_player.isLocalPlayer && (NetworkManager.singleton ? NetworkManager.singleton.isNetworkActive : false)) Destroy(this);
    }
    private void FixedUpdate() => _rb.Move(CalculateMove(), RotateTowardMouse ? CalculateRotateToMouse() : CalculateRotateToMovement());
    private void LateUpdate() => Animate();
    #region Moving
    private Vector3 CalculateMove()
    {
        Vector3 targetPosition = GetTargetPosition(MovementAcceleration, out targetMovementVector, true);
        targetPosition.y = transform.position.y;
        return targetPosition;
    }
    private Vector3 GetTargetPosition(float smoothTime, out Vector3 targetVector, bool withSmooth = true)
    {
        Vector2 moveInputVector = InputManager.Controls.Game.Move.ReadValue<Vector2>();
        
        if (withSmooth) currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInputVector, ref smoothInputVector, smoothTime);
        targetVector = Quaternion.Euler(0, _camera.gameObject.transform.rotation.eulerAngles.y, 0) * 
            new Vector3(withSmooth ? currentInputVector.x : moveInputVector.x, 0, withSmooth ? currentInputVector.y : moveInputVector.y);
        return transform.position + targetVector * (MovementSpeed * Time.deltaTime);
    }
    #endregion
    #region Rotation
    private Quaternion CalculateRotateToMouse()
    {
        Ray ray = _camera.ScreenPointToRay(InputManager.Controls.Game.Point.ReadValue<Vector2>());

        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f, ~ignoreLayers))
        {
            Vector3 currentLookVector = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);

            var targetRotation = Quaternion.LookRotation(currentLookVector - transform.position);
            return Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        return default;
    }
    private Quaternion CalculateRotateToMovement()
    {
        if (targetMovementVector.magnitude == 0) { return default; }
        var rotation = Quaternion.LookRotation(targetMovementVector);
        return Quaternion.RotateTowards(transform.rotation, rotation, RotationSpeed / 10.0f);
    }
    #endregion
    private void Animate()
    {
        Vector3 fixedRotation = Quaternion.Inverse(transform.rotation) * targetMovementVector;
        _anim.SetFloat("InclineX", fixedRotation.x);
        _anim.SetFloat("InclineY", fixedRotation.z);
    }
}