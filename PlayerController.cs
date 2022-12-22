using FishNet.Object;
using FishNet.Object.Prediction;
using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// PlayerController is used to provide movement control to the player.
    /// Note that the Rotate method sets the rotation.
    /// ApplyInput is a method that applies both rotation and movement changes, it is executed both on the server and the client to perform server authoratative movement with client side prediction.
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PlayerController : NetworkBehaviour
    {
        #region Private Fields
        
        private Rigidbody _rigidbody;
        
        private Collider _collider;
        
        private InputDriver _inputDriver;
        
        // Default variables
        [SerializeField]
        private float gravity, jumpAcceleration, movementForceGround, movementForceAir, airMaxSpeed, groundDrag, jumpCooldown, climbSpeed, climbResetTime, climbTime; 
        
        // Runtime variables
        private float _gravity, _jumpAcceleration, _movementForceGround, _movementForceAir, _airMaxSpeed, _groundDrag, _jumpCooldown, _climbSpeed, _climbResetTime, _climbTime;

        [SerializeField]
        private LayerMask groundMask, climbMask;
        
        [SerializeField]
        private bool canJump;
        
        private bool _canClimb;
        
        //Has gravity is for climb mechanic only.
        private bool _hasGravity;
        
        private float _climbCooldownTimer, _climbTimer, _jumpCooldownTimer;
        
        [SerializeField]
        private float slerpSpeed;
        
        private RaycastHit _slopeHit;
        
        private bool _subscribed;
        
        #endregion
        
        #region Public Properties
        public bool IsGrounded { get; private set; }

        public LayerMask GroundMask => groundMask;
        public LayerMask ClimbMask => climbMask;

        #endregion

        public GameObject cameraRig;

        private void ApplyInput(InputData input)
        {
            ApplyTimelessModifiers();
            Move(input.MoveDirection);
            Rotate(input.RawRotationInput);
            Jump(input.Jump);
            Climb(input.RawMoveInput);
        }

        private void Move(Vector3 moveDirection)
        {
            if (!IsGrounded)
            {
                var expectedHorizontalVelocityMagnitude =
                    Vector3.Magnitude(ZeroY(_rigidbody.velocity) + DirectionByForce(moveDirection, _movementForceAir));
                if (expectedHorizontalVelocityMagnitude > _airMaxSpeed ||
                    expectedHorizontalVelocityMagnitude > HorizontalVelocityMagnitude(_rigidbody)) return;
                AddForceInDirection(_rigidbody, moveDirection, _movementForceAir);
                return;
            }
            AddForceInDirection(_rigidbody, moveDirection, _movementForceGround);
        }

        private static float HorizontalVelocityMagnitude(Rigidbody rigidbody)
        {
            return Vector3.Magnitude(ZeroY(rigidbody.velocity));
        }
        
        private void AddForceInDirection(Rigidbody localRigidbody, Vector3 direction, float force)
        {
            localRigidbody.AddForce(DirectionByForce(direction, force), ForceMode.VelocityChange);
        }

        private Vector3 DirectionByForce(Vector3 direction, float force)
        {
            return Vector3.Normalize(direction) * ((float)TimeManager.TickDelta * force);
        }

        private void Jump(bool jumping)
        {
            if (!jumping || !canJump) return;
            var clampedHorizontalVelocity = Vector3.ClampMagnitude(ZeroY(_rigidbody.velocity), _airMaxSpeed);
            _rigidbody.velocity = new Vector3(clampedHorizontalVelocity.x, _rigidbody.velocity.y, clampedHorizontalVelocity.z);
            _rigidbody.AddForce(Vector3.up * _jumpAcceleration, ForceMode.VelocityChange);
            _jumpCooldownTimer = 0;
        }

        private void Rotate(Vector2 rotationInput)
        {
            RotateCameraRigVertical(cameraRig, rotationInput.y);
            var rotation = transform.rotation;
            rotation = Quaternion.Slerp(rotation, ToQuaternion(rotation.eulerAngles + new Vector3(0, rotationInput.x, 0)), slerpSpeed * (float)TimeManager.TickDelta);
            transform.rotation = rotation;
        }

        private void RotateCameraRigVertical(GameObject cameraRigInstance, float input)
        {
            if (cameraRigInstance == null) return;
            var rigRotation = cameraRigInstance.transform.localRotation;
            var rigRotationEuler = rigRotation.eulerAngles;
            var clampedInput = Mathf.Clamp(AngleTranslator(rigRotation.eulerAngles.x - input), -90f, 90f);
            rigRotation = Quaternion.Slerp(rigRotation, ToQuaternion(new Vector3(clampedInput, rigRotationEuler.y, rigRotationEuler.z)), slerpSpeed * (float)TimeManager.TickDelta);
            cameraRigInstance.transform.localRotation = rigRotation;
        }

        private static Vector3 ZeroY(Vector3 input)
        {
            return new Vector3(input.x, 0, input.z);
        }

        private void Climb(Vector2 rawMoveInput)
        {
            //Using Vector2.up here because the expected input is from a keyboard.
            if (rawMoveInput == Vector2.up && _canClimb && _climbTimer < _climbTime)
            {
                var velocity = _rigidbody.velocity;
                velocity = new Vector3(velocity.x, _climbSpeed, velocity.z);
                _rigidbody.velocity = velocity;
                _hasGravity = false;
                return;
            }
            _hasGravity = true;
        }

        private void ApplyTimelessModifiers()
        {
            IsGrounded = Physics.Raycast(transform.position, Vector3.down, (_collider.bounds.size.y / 2) + 0.25f, GroundMask);

            canJump = IsGrounded && _jumpCooldownTimer >= _jumpCooldown;

            _rigidbody.drag = IsGrounded ? _groundDrag : 0;

            if (!Physics.Raycast(transform.position - new Vector3(0, 0.9f, 0), transform.forward,
                    (_collider.bounds.size.y / 2) + 0.1f, ClimbMask))
            {
                _canClimb = false;
                _hasGravity = true;
                return;
            }
            _canClimb = true;
        }

        private void ApplyTimeBasedModifiers()
        {
            _jumpCooldownTimer += (float)TimeManager.TickDelta;
            _climbTimer += (float)TimeManager.TickDelta;

            if (!_hasGravity)
            {
                _climbCooldownTimer = 0f;
                return;
            }
            
            //for climbing
            _climbCooldownTimer += (float)TimeManager.TickDelta;
            if (_climbCooldownTimer >= _climbResetTime)
            {
                _climbTimer = 0f;
            }
            
            //for slopes
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit,
                    (_collider.bounds.size.y / 2) + 0.25f, GroundMask) && Vector3.Angle(Vector3.up, _slopeHit.normal) <= 45f)
            {
                _rigidbody.AddForce(_gravity * (float)TimeManager.TickDelta * -_slopeHit.normal,
                    ForceMode.VelocityChange);
                return;
            }
            
            _rigidbody.AddForce(Vector3.down * (_gravity * (float)TimeManager.TickDelta),
                ForceMode.VelocityChange);
        }

        private void SubscribeToTimeManager(bool subscribe)
        {
            if (TimeManager == null) return;
            if (_subscribed == subscribe) return;
            _subscribed = subscribe;

            if (subscribe)
            {
                TimeManager.OnTick += TimeManager_OnTick;
                TimeManager.OnPostTick += TimeManager_OnPostTick;
                return;
            }
            
            TimeManager.OnTick -= TimeManager_OnTick;
            TimeManager.OnPostTick -= TimeManager_OnPostTick;
        }

        private void TimeManager_OnTick()
        {
            ApplyTimeBasedModifiers();
            ApplyTimelessModifiers();

            if (IsOwner)
            {
                if (_inputDriver == null) return;
                Reconciliation(default, false);
                _inputDriver.UpdateInputData(out var inputData);
                Replicate(inputData, false);
            }

            if (IsServer)
            {
                Replicate(default, true);
            }
        }

        private void TimeManager_OnPostTick()
        {
            if (!IsServer) return;
            var cachedTransform = transform;
            var data = new ReconcileData(cachedTransform.position, _rigidbody.velocity, cachedTransform.rotation,
                cameraRig.transform.localRotation, _jumpCooldownTimer, _climbCooldownTimer, _climbTimer, _hasGravity,
                canJump, _rigidbody.drag, IsGrounded);
            Reconciliation(data, true);
        }

        private void OnDestroy()
        {
            SubscribeToTimeManager(false);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            SubscribeToTimeManager(true);
            _inputDriver = GetComponent<InputDriver>();
            if (!(IsOwner || IsServer))
            {
                _gravity = 0;
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            SubscribeToTimeManager(true);
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _rigidbody.drag = 0f;
            _rigidbody.angularDrag = 0f;
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = false;

            _gravity = gravity;
            _jumpAcceleration = jumpAcceleration;
            _movementForceGround = movementForceGround;
            _movementForceAir = movementForceAir;
            _airMaxSpeed = airMaxSpeed;
            _groundDrag = groundDrag;
            _jumpCooldown = jumpCooldown;
            _climbSpeed = climbSpeed;
            _climbTime = climbTime;
            _climbResetTime = climbResetTime;
        }

        [Reconcile]
        private void Reconciliation(ReconcileData data, bool asServer)
        {
            var cachedTransform = transform;
            cachedTransform.position = data.Position;
            _rigidbody.velocity = data.Velocity;
            cachedTransform.rotation = data.Rotation;
            cameraRig.transform.localRotation = data.CameraRigRotation;
            _jumpCooldownTimer = data.JumpCooldownTimer;
            _climbCooldownTimer = data.ClimbCooldownTimer;
            _climbTimer = data.ClimbTimer;
            _hasGravity = data.HasGravity;
            canJump = data.CanJump;
            _rigidbody.drag = data.Drag;
            IsGrounded = data.IsGrounded;
        }

        [Replicate]
        private void Replicate(InputData inputData, bool asServer, bool replaying = false)
        {
            ApplyInput(inputData);
        }

        private static Quaternion ToQuaternion(Vector3 eulerAngle)
        {
            return Quaternion.Euler(eulerAngle.x, eulerAngle.y, eulerAngle.z);
        }

        private static float AngleTranslator(float input)
        {
            return input switch
            {
                < 0 => input,
                >= 0 and <= 180 => input,
                >= 180 and <= 360 => input - 360,
                _ => 0
            };
        }
    }
}
