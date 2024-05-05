using Mirror;
using NaughtyAttributes;
using Scripts.InputManagement;
using Unity.Burst;
using UnityEngine;

namespace Scripts.Gameplay
{
    [BurstCompile]
    public class PlayerMovementUnsafe : NetworkBehaviour
    {
        [Header("General")]
        [SerializeField, BoxGroup("General")] private float MovementSpeed = 5.0f;
        [SerializeField, BoxGroup("General")] private float MovementAcceleration = 0.2f;
        [SerializeField, BoxGroup("General")] private float RotationSpeed = 2.0f;
        [Header("MouseRotation")]
        [SerializeField, BoxGroup("MouseRotation")] private LayerMask ignoreLayers;

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

        private void FixedUpdate()
        {
            Move(InputManager.Controls.Game.Move.ReadValue<Vector2>(),
                 InputManager.Controls.Game.Point.ReadValue<Vector2>());
        }
        private void LateUpdate() => Animate();
        #region Moving
        private void Move(Vector2 vector, Vector2 rotationPoint) 
        {
            _rb.Move(CalculateMove(vector),
                     CalculateRotateToMouse(rotationPoint));
        } 

        private Vector3 CalculateMove(Vector2 movementVector)
        {
            Vector3 targetPosition = GetTargetPosition(movementVector, out targetMovementVector);
            targetPosition.y = transform.position.y;
            return targetPosition;
        }
        private Vector3 GetTargetPosition(Vector2 moveInputVector, out Vector3 targetVector, bool withSmooth = true)
        {
            if (withSmooth) currentInputVector = Vector2.SmoothDamp(currentInputVector, moveInputVector, ref smoothInputVector, MovementAcceleration);
            targetVector = Quaternion.Euler(0, _camera.gameObject.transform.rotation.eulerAngles.y, 0) *
                new Vector3(withSmooth ? currentInputVector.x : moveInputVector.x, 0, withSmooth ? currentInputVector.y : moveInputVector.y);
            return transform.position + targetVector * (MovementSpeed * Time.deltaTime);
        }
        private Quaternion CalculateRotateToMouse(Vector2 mousePoint)
        {
            Ray ray = _camera.ScreenPointToRay(mousePoint);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f, ~ignoreLayers))
            {
                Vector3 currentLookVector = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);

                var targetRotation = Quaternion.LookRotation(currentLookVector - transform.position);
                return Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
            return _rb.rotation;
        }
        #endregion
        private void Animate()
        {
            Vector3 rotation = Quaternion.Inverse(transform.rotation) * targetMovementVector;
            _anim.SetFloat("InclineX", rotation.x);
            _anim.SetFloat("InclineY", rotation.z);
        }
    }
}