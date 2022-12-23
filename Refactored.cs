using System.Collections.Generic;
using System.Linq;
using Extensions;
using General;
using Photon.Pun;
using Terrain;
using UnityEngine;

namespace Players
{
    public class PlayerZee : MonoBehaviourPun
    {
        #region Private Fields
        
        [SerializeField]
        private float xForce, xStoppingForce, xVelocityCap, yForce;

        private bool _isGrounded;

        private float _rayDistance;

        private Vector3 _cursorPos;

        private bool _dashing;

        private float _dashCd;

        private float _dashTimer;

        private float _dashAccel;

        private float _dashSpeed;

        private float _dashDeceleration;

        private bool _dashLightning;

        private bool _lightningMode;

        private float _dashLength;

        private Vector3 _1PassthroughTargetPosition;

        private Vector3 _dashFirstEntry;

        private Vector3 _dashFirstExit;

        private Vector3 _dashSecondEntry;

        private Vector3 _dashSecondExit;

        private Vector3 _dashOrigin;

        private float _maxThickness;

        private int _dashPassthrough;

        private List<RaycastHit2D> _normalWallCheck;
        private List<RaycastHit2D> _invertedWallCheck;
        private List<RaycastHit2D> _offsetCheck;

        private ContactFilter2D _contactFilter;

        private IsMetal _isMetal;

        private float _dashPhysicsTimer;
        private float _thisDashLength;
        private float _thisDashAccelTime;
        private float _thisDashDecelerationTime;
        private float _thisDashAccelLength;
        private float _thisDashDecelerationLength;
        private float _thisDashStopAccelTime;
        private float _thisDashStartDecelerationTime;
        private float _thisDashStopDecelerationTime;
        private float _thisDashOffset;
        private Vector3 _dashDirection;

        private GameObject _dashTargetObject;

        private Collider2D _firstMetalCollider;
        private Collider2D _secondMetalCollider;
        private bool _ignoreCollisionRan;

        private Collider2D _dropCollider;

        private RaycastHit2D _grabBoxcast;

        private float _grabTimer;
        private float _grabCd;
        private bool _isGrabbing;

        private float _jumpTime;
        private float _jumpCooldown;

        private RaycastHit2D _floorCheck;

        private float _lightningResource;

        private float _lightningResourceMax;

        private float _lightningResourceRegen;

        private float _lightningResourceDeplete;

        private float _lightningResourceDashUse;

        private int _firstColliderLayer;

        private int _secondColliderLayer;

        private int _playerColliderLayer;

        private int _lightningResourceAttackUse;

        private enum LookDirection
        {
            Left,
            Right
        }

        private LookDirection _lookDirection;

        private Health.Health _attackTargetHealth;

        private float _attackTimer;

        private IsWood _dashWoodObject;

        private float _grabSavedY;

        private KeyCode _keyLeft;

        private KeyCode _keyRight;
        
        private KeyCode _keyDown;

        private KeyCode _keyUp;

        private KeyCode _keyLightningMode;

        private KeyCode _keyAttack;

        private KeyCode _keyDash;

        private LevelManager _levelManager;
        private Collider2D _collider2D;
        private PhotonView _photonView;
        private Camera _camera;
        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rigidbody2D;
        private Health.Health _health;

        #endregion

        private void TestVariableSettings()
        {
            _dashSpeed = 50;
            _dashAccel = 90;
            _maxThickness = 3;
            _dashDeceleration = 180;
            _dashLength = 6;
            _lightningResourceMax = 100;
            //per second
            _lightningResourceRegen = 10;
            _lightningResourceDeplete = 20;
            _lightningResourceDashUse = 40;
            _lightningResourceAttackUse = 40;
            _keyLeft = KeyCode.A;
            _keyRight = KeyCode.D;
            _keyUp = KeyCode.W;
            _keyDown = KeyCode.S;
            _keyAttack = KeyCode.Space;
            _keyLightningMode = KeyCode.Mouse1;
            _keyDash = KeyCode.Mouse0;
        }
        
        private void Start()
        {
            _health = gameObject.GetComponent<Health.Health>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _camera = transform.GetChild(0).gameObject.GetComponent<Camera>();
            _photonView = GetComponent<PhotonView>();
            _collider2D = GetComponent<Collider2D>();
            TestVariableSettings();
            _normalWallCheck = new List<RaycastHit2D>();
            _invertedWallCheck = new List<RaycastHit2D>();
            _offsetCheck = new List<RaycastHit2D>();
            _contactFilter.NoFilter();
            gameObject.GetComponent<Health.Health>().OnDeath += PlayerZee_OnDeath;
            _levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        }

        private void PlayerZee_OnDeath()
        {
            if (!photonView.IsMine) return;
            _levelManager.SubLevelReset();
        }

        private void Update()
        {
            if (!_photonView.IsMine) return;
            
            CheckIfGrounded();

            SetCursorPosition();

            TickLightningResource();

            Run();

            Jump();

            LightningMode();

            Dash(_lightningMode);

            FallThrough(_keyDown);

            Grab();

            Attack(_lightningMode);
        }

        private void CheckIfGrounded()
        {
            _collider2D.enabled = false;
            _floorCheck = Physics2D.BoxCast(transform.position, new Vector2(1, 2), 0, Vector2.down, 0.1f);
            _collider2D.enabled = true;
            _isGrounded = _floorCheck.collider != null && !_floorCheck.collider.gameObject.TryGetComponent<IsProjectile>(out _);
        }

        private void SetCursorPosition()
        {
            var plane = new Plane(Vector3.forward, 0);
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            _cursorPos = plane.Raycast(ray, out _rayDistance) ? ray.GetPoint(_rayDistance) : default;
        }

        private void TickLightningResource()
        {
            if (_lightningResource < _lightningResourceMax)
            {
                _lightningResource += _lightningResourceRegen * Time.deltaTime;
            }
            else
            {
                _lightningResource = _lightningResourceMax;
            }
        }

        private void FallThrough(KeyCode input)
        {
            _collider2D.enabled = false;
            _dropCollider = Physics2D.Raycast((_capsuleCollider2D.bounds.center + new Vector3(0, 1)), Vector2.down, 2.1f).collider;
            _collider2D.enabled = true;
            if (_dropCollider == null) return;
            Physics2D.IgnoreCollision(_collider2D, _dropCollider, Input.GetKey(input));
        }

        private void Run()
        {
            ChangeLookDirection();
            if (_dashing) return;
            
            if (Input.GetKey(KeyCode.A) && _rigidbody2D.velocity.x > -xVelocityCap)
            {
                _rigidbody2D.AddForce(new Vector3(-xForce * 100 * Time.deltaTime, 0));
            }
            else if (_rigidbody2D.velocity.x < 0)
            {
                ApplyStoppingForce(xStoppingForce);
            }

            if (Input.GetKey(KeyCode.D) && _rigidbody2D.velocity.x < xVelocityCap)
            {
                _rigidbody2D.AddForce(new Vector3(xForce * 100 * Time.deltaTime, 0));
            }
            else if (_rigidbody2D.velocity.x > 0)
            {
                ApplyStoppingForce(-xStoppingForce);
            }
        }

        private void ApplyStoppingForce(float stoppingForce)
        {
            _rigidbody2D.AddForce(new Vector3(stoppingForce * 100 * Time.deltaTime * Mathf.Abs(_rigidbody2D.velocity.x), 0));
        }

        private void ChangeLookDirection()
        {
            if (Input.GetKeyDown(_keyLeft))
            {
                _lookDirection = LookDirection.Left;
            }
            else if (Input.GetKeyDown(_keyRight))
            {
                _lookDirection = LookDirection.Right;
            }
        }

        private void Jump()
        {
            if (_dashing) return;
            DelayJumpConditions();
            
            if (Input.GetKeyDown(_keyUp))
            {
                if (_jumpTime > 0 && _jumpCooldown < 0)
                {
                    _rigidbody2D.AddForce(new Vector3(0, yForce * 10));
                    _jumpTime = 0;
                    _jumpCooldown = 1;
                }
            }
            _jumpTime -= Time.deltaTime;
            _jumpCooldown -= Time.deltaTime;
        }

        private void DelayJumpConditions()
        {
            if (_isGrabbing)
            {
                _jumpTime = 0.5f;
            }
            else if (_isGrounded)
            {
                _jumpTime = 0.35f;
            }
        }

        private void LightningMode()
        {
            if (Input.GetKey(_keyLightningMode))
            {
                if (!_lightningMode)
                {
                    _health.SetDamageFactor(_health.damageFactor * 0.75f);
                }
                _lightningMode = true;
                _lightningResource -= _lightningResourceDeplete * Time.deltaTime;
            }
            else
            {
                if (_lightningMode)
                {
                    _health.SetDamageFactor(_health.damageFactor * (1f / 0.75f));
                }
                _lightningMode = false;
            }
        }

        //The dash logic is complicated as it's behaviour is dependent various factors.
        //This includes the mode the player is in, the materials of the layers of walls they intend to pass through, and it's mixed with precise acceleration to make it feel right.
        private void Dash(bool lightningMode)
        {
            _dashCd -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Mouse1) && _dashCd <= 0)
            {
                InitiateDash(lightningMode);
            }

            DashProcess();
        }

        private void DashProcess()
        {
            if (!_dashing) return;
            _dashPhysicsTimer += Time.deltaTime;
            if (_dashLightning)
            {
                LightningDashProcess();
            }
            else
            {
                NormalDashProcess();
            }
        }

        private void NormalDashProcess()
        {
            if (_dashPhysicsTimer >= _thisDashStopDecelerationTime)
            {
                //stop dash
                _dashing = false;
                CheckAndBreakWood(_dashTargetObject);
                _rigidbody2D.gravityScale = 1;
                _rigidbody2D.velocity = Vector3.zero;
            }
            else if (_dashPhysicsTimer <= _thisDashStopDecelerationTime &&
                     _dashPhysicsTimer >= _thisDashStartDecelerationTime)
            {
                //deccelerate
                _rigidbody2D.velocity +=
                    Vector3Extension.getV3fromV2(-_dashDirection * (_dashDeceleration * Time.deltaTime));
            }
            else if (_dashPhysicsTimer <= _thisDashStopAccelTime)
            {
                //accelerate
                _rigidbody2D.velocity +=
                    Vector3Extension.getV3fromV2(_dashDirection * (_dashAccel * Time.deltaTime));
            }
        }

        private void LightningDashProcess()
        {
            if (_dashPassthrough == 0)
            {
                NormalDashProcess();
            }
            else
            {
                PassthroughDashProcess();
            }
        }

        private void PassthroughDashProcess()
        {
            if (_dashPhysicsTimer >= _thisDashStopDecelerationTime)
            {
                //stop dash
                _dashing = false;
                CheckAndBreakWood(_dashTargetObject);
                _ignoreCollisionRan = false;
                _rigidbody2D.gravityScale = 1;
                _rigidbody2D.velocity = Vector3.zero;
                gameObject.layer = _playerColliderLayer;
                if (_firstMetalCollider != null)
                {
                    _firstMetalCollider.gameObject.layer = _firstColliderLayer;
                }

                if (_secondMetalCollider != null)
                {
                    _secondMetalCollider.gameObject.layer = _secondColliderLayer;
                }
            }
            else if (_dashPhysicsTimer <= _thisDashStopDecelerationTime &&
                     _dashPhysicsTimer >= _thisDashStartDecelerationTime)
            {
                //deccelerate
                _rigidbody2D.velocity +=
                    Vector3Extension.getV3fromV2(-_dashDirection * (_dashDeceleration * Time.deltaTime));
            }
            else if (_dashPhysicsTimer <= _thisDashStopAccelTime)
            {
                //accelerate
                _rigidbody2D.velocity +=
                    Vector3Extension.getV3fromV2(_dashDirection * (_dashAccel * Time.deltaTime));
            }

            if (_ignoreCollisionRan) return;
            _firstColliderLayer = _firstMetalCollider.gameObject.layer;
            if (_secondMetalCollider != null)
            {
                _secondColliderLayer = _secondMetalCollider.gameObject.layer;
            }

            var cachedGo = gameObject;
            _playerColliderLayer = cachedGo.layer;
            cachedGo.layer = 9;

            DealDamageAndSetLayerForWalls(_firstMetalCollider, 20f);

            DealDamageAndSetLayerForWalls(_secondMetalCollider, 40f);

            _ignoreCollisionRan = true;
        }

        private void DealDamageAndSetLayerForWalls(Collider2D target, float damage)
        {
            if (target == null) return;
            target.gameObject.layer = 10;
            if (!target.gameObject.TryGetComponent(out _attackTargetHealth)) return;
            if (!_attackTargetHealth.isPlayerHealth)
            {
                _attackTargetHealth.Damage(damage);
            }
        }

        private void InitiateDash(bool lightningMode)
        {
            _dashing = true;
            _dashCd = 6;
            _dashTimer = 1.2f;
            _dashLightning = _lightningResource > _lightningResourceDashUse && lightningMode;

            if (_dashLightning)
            {
                _lightningResource -= _lightningResourceDashUse;
            }

            var position = transform.position;
            _dashOrigin = position;
            _dashPhysicsTimer = 0;
            _rigidbody2D.velocity = new Vector2(0, 0);

            var dashDirection = Vector3.Normalize(_cursorPos - position);
            _offsetCheck = new List<RaycastHit2D>(Physics2D.RaycastAll(
                position + dashDirection * 1.5f,
                -dashDirection, 1.5f));
            
            foreach (var result in _offsetCheck.Where(result => result.collider == _collider2D))
            {
                _thisDashOffset = 1.5f - result.distance;
            }

            _rigidbody2D.gravityScale = 0;

            _collider2D.enabled = false;

            var offsetDashPosition = position + dashDirection * _thisDashOffset;
            var firstWallCheck =
                Physics2D.Raycast(offsetDashPosition,
                    dashDirection, _dashLength);
            
            _invertedWallCheck = new List<RaycastHit2D>(Physics2D.RaycastAll(
                position + dashDirection * (_thisDashOffset + 40),
                -dashDirection, 40));
            
            _normalWallCheck = new List<RaycastHit2D>(Physics2D.RaycastAll(
                offsetDashPosition,
                dashDirection, 40));
            
            _collider2D.enabled = true;

            //calculate dash target
            if (_dashLightning)
            {
                if (firstWallCheck.collider != null)
                {
                    CheckWalls(firstWallCheck, position);
                }
                else
                {
                    SetDashTargetWithoutContact(position);
                    _dashPassthrough = 0;
                }
            }
            else
            {
                if (firstWallCheck.collider == null)
                {
                    SetDashTargetWithoutContact(position);
                }
                else
                {
                    _1PassthroughTargetPosition = position + Vector3.Normalize(_cursorPos - position) * firstWallCheck.distance;
                    _dashTargetObject = firstWallCheck.collider.gameObject;
                }
            }

            CalculateDashProperties(position);
        }

        private void CalculateDashProperties(Vector3 position)
        {
            _dashDirection = Vector3.Normalize(_1PassthroughTargetPosition - position);
            _thisDashLength = Vector3.Magnitude(_1PassthroughTargetPosition - position);
            _thisDashAccelTime = _dashSpeed / _dashAccel;
            _thisDashDecelerationTime = _dashSpeed / _dashDeceleration;
            _thisDashAccelLength = 0.5f * _dashAccel * _thisDashAccelTime * _thisDashAccelTime;
            _thisDashDecelerationLength = 0.5f * _dashDeceleration * _thisDashDecelerationTime * _thisDashDecelerationTime;
            if (_thisDashAccelLength + _thisDashDecelerationLength > _thisDashLength)
            {
                _thisDashStopAccelTime = Mathf.Sqrt((_thisDashLength / (_thisDashAccelLength + _thisDashDecelerationLength)) *
                                                    _thisDashAccelTime * _thisDashAccelTime);
                _thisDashStartDecelerationTime = _thisDashStopAccelTime;
                _thisDashStopDecelerationTime = _thisDashStartDecelerationTime + Mathf.Sqrt(
                    (_thisDashLength / (_thisDashAccelLength + _thisDashDecelerationLength)) * _thisDashDecelerationTime *
                    _thisDashDecelerationTime);
            }
            else
            {
                _thisDashStopAccelTime = _thisDashAccelTime;
                _thisDashStartDecelerationTime = _thisDashAccelTime +
                                                 ((_dashLength - (_thisDashAccelLength + _thisDashDecelerationLength)) /
                                                  _dashSpeed);
                _thisDashStopDecelerationTime = _thisDashStartDecelerationTime + _thisDashDecelerationTime;
            }
        }

        private void SetDashTargetWithoutContact(Vector3 position)
        {
            _1PassthroughTargetPosition = position + Vector3.Normalize(_cursorPos - position) * _dashLength;
            _dashTargetObject = null;
        }

        private void CheckWalls(RaycastHit2D firstWallCheck, Vector3 position)
        {
            for (var i = 0; i < _invertedWallCheck.Count; i++)
            {
                if (_invertedWallCheck[i].collider != firstWallCheck.collider) continue;
                if (Vector3.Magnitude(_invertedWallCheck[i].point - firstWallCheck.point) <=
                    _maxThickness &&
                    firstWallCheck.collider.gameObject.TryGetComponent(out _isMetal))
                {
                    FirstPassthrough(firstWallCheck, position, i);
                }
                else
                {
                    _1PassthroughTargetPosition = position +
                                                  Vector3.Normalize(_cursorPos - position) * firstWallCheck.distance;
                    _firstMetalCollider = null;
                    _dashTargetObject = firstWallCheck.collider.gameObject;
                }
            }
        }

        private void FirstPassthrough(RaycastHit2D firstWallCheck, Vector3 position, int i)
        {
            _firstMetalCollider = firstWallCheck.collider;
            _dashPassthrough = 1;
            _dashFirstEntry = position +
                              Vector3.Normalize(_cursorPos - position) * firstWallCheck.distance;
            _dashFirstExit = (Vector3)_invertedWallCheck[i].point +
                             (Vector3.Normalize(_cursorPos - position) * _thisDashOffset);
            _1PassthroughTargetPosition = position + Vector3.Normalize(_cursorPos - position) *
                (firstWallCheck.distance + _dashLength);
            _dashTargetObject = null;
            foreach (var result in _normalWallCheck.Where(result => i != 0)
                         .Where(result => _invertedWallCheck[i - 1].collider == result.collider))
            {
                HandleValidWalls(firstWallCheck, position, i, result);
            }
        }

        private void HandleValidWalls(RaycastHit2D firstWallCheck, Vector3 position, int i, RaycastHit2D result)
        {
            if (result.distance <= firstWallCheck.distance + _dashLength)
            {
                if (result.collider.gameObject.TryGetComponent(
                        out _isMetal) &&
                    Vector3.Magnitude(
                        result.point - _invertedWallCheck[i - 1].point) <=
                    _maxThickness)
                {
                    SecondPassthrough(position, i, result);
                }
                else
                {
                    _1PassthroughTargetPosition = position +
                                                  Vector3.Normalize(_cursorPos - position) *
                                                  result.distance;
                    _secondMetalCollider = null;
                    _dashTargetObject = result.collider.gameObject;
                }
            }
            else
            {
                _1PassthroughTargetPosition = position +
                                              Vector3.Normalize(_cursorPos - position) *
                                              (firstWallCheck.distance + _dashLength);
                _dashTargetObject = null;
            }
        }

        private void SecondPassthrough(Vector3 position, int i, RaycastHit2D result)
        {
            _secondMetalCollider = result.collider;
            _dashPassthrough = 2;
            _dashSecondEntry = position +
                               Vector3.Normalize(_cursorPos - position) *
                               result.distance;
            _dashSecondExit = (Vector3)_invertedWallCheck[i - 1].point +
                              (Vector3.Normalize(_cursorPos - position) *
                               _thisDashOffset);
            _1PassthroughTargetPosition = position +
                                          Vector3.Normalize(_cursorPos - position) *
                                          (result.distance + _dashLength);
            _dashTargetObject = null;
            _collider2D.enabled = false;
            result.collider.enabled = false;
            if (Physics2D.Raycast(result.point,
                    Vector3.Normalize(_cursorPos - position), _dashLength)
                .collider)
            {
                _1PassthroughTargetPosition = (Vector3)result.point +
                                              (Vector3.Normalize(_cursorPos - position) *
                                               (Physics2D.Raycast(result.point,
                                                    Vector3.Normalize(
                                                        _cursorPos - position),
                                                    _dashLength).distance -
                                                _thisDashOffset));
                _dashTargetObject = Physics2D.Raycast(result.point,
                        Vector3.Normalize(_cursorPos - position),
                        _dashLength)
                    .collider.gameObject;
            }

            _collider2D.enabled = true;
            result.collider.enabled = true;
        }

        private void Grab()
        {
            if (Input.GetKey(KeyCode.A))
            {
                _collider2D.enabled = false;
                _grabBoxcast = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, Vector2.left, 0.25f);
                _collider2D.enabled = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _collider2D.enabled = false;
                _grabBoxcast = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, Vector2.right, 0.25f);
                _collider2D.enabled = true;
            }

            if (_grabBoxcast.collider != null && _grabTimer > 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (Input.GetKey(_keyLeft) || Input.GetKey(_keyRight)) && !_dashing)
            {
                if (_grabBoxcast.collider.isTrigger) return;
                //change value if grab timer changed
                if (_grabTimer < 3.5f && Input.GetKeyDown(_keyUp))
                {
                    _grabTimer = 0;
                    _grabCd = 3;
                }
                else
                {
                    _grabTimer -= Time.deltaTime;
                    if (_isGrabbing == false)
                    {
                        _grabSavedY = transform.position.y;
                    }
                    _isGrabbing = true;
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
                    transform.position = new Vector3(transform.position.x, _grabSavedY);
                    _grabCd = 3;
                }
            }
            else
            {
                _grabTimer = 0;
                _isGrabbing = false;
                _grabCd -= Time.deltaTime;
                if (_grabCd < 0)
                {
                    _grabTimer = 4;
                }
            }
        }

        private void Attack(bool lightningMode)
        {
            _attackTimer += Time.deltaTime;
            if (!Input.GetKeyDown(_keyAttack) || !(_attackTimer >= 0.7f)) return;
            _attackTimer = 0;
            if (_lookDirection == LookDirection.Left)
                AttackInDirection(lightningMode, Vector3.left);
            else if (_lookDirection == LookDirection.Right)
                AttackInDirection(lightningMode, Vector3.right);
        }

        private void AttackInDirection(bool lightningMode, Vector3 direction)
        {
            _collider2D.enabled = false;
            var hitCollider = Physics2D.Raycast(transform.position, direction, 3).collider;
            if (hitCollider != null && hitCollider.gameObject
                    .TryGetComponent(out _attackTargetHealth) && !_attackTargetHealth.isPlayerHealth)
            {
                if (lightningMode)
                {
                    _attackTargetHealth.Damage(15);
                    _lightningResource -= _lightningResourceAttackUse;
                }
                else
                {
                    _attackTargetHealth.Damage(10);
                }
            }

            _collider2D.enabled = true;
        }

        private void CheckAndBreakWood(GameObject dashTargetObject)
        {
            if (dashTargetObject == null) return;
            if (dashTargetObject.TryGetComponent<IsWood>(out _dashWoodObject))
            {
                _dashWoodObject.BreakWood();
            }
        }

        private void OnDisable()
        {
            if (gameObject.GetComponent<Health.Health>() != null)
            {
                gameObject.GetComponent<Health.Health>().OnDeath -= PlayerZee_OnDeath;
            }
        }
    }
}
