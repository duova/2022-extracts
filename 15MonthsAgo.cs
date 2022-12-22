using System.Collections.Generic;
using Extensions;
using General;
using Photon.Pun;
using Terrain;
using UnityEngine;

namespace Players
{
    public class PlayerZee : MonoBehaviourPun
    {

        public float xForce;
        public float xStoppingForce;
        public float xVeloCap;
        public float yForce;

        bool IsGrounded;

        float rayDistance;

        Vector3 cursorPos;

        bool dashing;

        float dashCD;

        float dashTimer;

        float dashAccel;

        float dashSpeed;

        float dashDeccel;

        bool dashLightning;

        bool lightningMode;

        float dashLength;

        Vector3 dashTarget;

        Vector3 dashFirstEntry;

        Vector3 dashFirstExit;

        Vector3 dashSecondEntry;

        Vector3 dashSecondExit;

        Vector3 dashOrigin;

        float maxThickness;

        int dashPassthrough;

        List<RaycastHit2D> resultsList;
        List<RaycastHit2D> resultsListReturn;
        List<RaycastHit2D> resultsListSelf;

        ContactFilter2D contactFilter;

        IsMetal isMetal;

        float dashPhysicsTimer;
        float thisDashLength;
        float thisDashAccelTime;
        float thisDashDeccelTime;
        float thisDashAccelLength;
        float thisDashDeccelLength;
        float thisDashStopAccelTime;
        float thisDashStartDeccelTime;
        float thisDashStopDeccelTime;
        float thisDashOffset;
        Vector3 dashDirection;

        GameObject dashTargetObject;

        Collider2D firstMetalCollider;
        Collider2D secondMetalCollider;
        bool ignoreCollisionRan;

        Collider2D dropCollider;

        RaycastHit2D grabBoxcast;

        float grabTimer;
        float grabCD;
        bool isGrabbing;

        float jumpTime;
        float jumpCooldown;

        RaycastHit2D floorCheck;

        float lightningResource;

        float lightningResourceMax;

        float lightningResourceRegen;

        float lightningResourceDeplete;

        float lightningResourceDashUse;

        float lightningResourceAttackUse;

        int firstColliderLayer;

        int secondColliderLayer;

        int playerColliderLayer;

        enum LookDirection { LEFT, RIGHT };

        LookDirection lookDirection;

        Health.Health attackTargetHealth;

        float attackTimer;

        IsWood dashWoodObject;

        float grabSavedY;

        IsProjectile projectileTest;

        LevelManager levelManager;

        void Start()
        {
            dashSpeed = 50;
            dashAccel = 90;
            maxThickness = 3;
            dashDeccel = 180;
            dashLength = 6;
            resultsList = new List<RaycastHit2D>();
            resultsListReturn = new List<RaycastHit2D>();
            resultsListSelf = new List<RaycastHit2D>();
            contactFilter.NoFilter();
            lightningResourceMax = 100;
            //per second
            lightningResourceRegen = 10;
            lightningResourceDeplete = 20;
            lightningResourceDashUse = 40;
            gameObject.GetComponent<Health.Health>().OnDeath += PlayerZee_OnDeath;
            levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        }

        void PlayerZee_OnDeath()
        {
            if (photonView.IsMine)
            {
                levelManager.SubLevelReset();
            }
        }

        void Update()
        {
            if (this.GetComponent<PhotonView>().IsMine)
            {
                GetComponent<Collider2D>().enabled = false;
                floorCheck = Physics2D.BoxCast(transform.position, new Vector2(1, 2), 0, Vector2.down, 0.1f);
                GetComponent<Collider2D>().enabled = true;

                if (floorCheck.collider != null)
                {
                    if (!floorCheck.collider.gameObject.TryGetComponent<IsProjectile>(out projectileTest))
                    {
                        IsGrounded = true;
                    }
                    else
                    {
                        IsGrounded = false;
                    }
                }
                else
                {
                    IsGrounded = false;
                }

                Plane plane = new Plane(Vector3.forward, 0);
                Ray ray = transform.GetChild(0).gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out rayDistance))
                {
                    cursorPos = ray.GetPoint(rayDistance);
                }

                if (lightningResource < lightningResourceMax)
                {
                    lightningResource += lightningResourceRegen * Time.deltaTime;
                }
                else
                {
                    lightningResource = lightningResourceMax;
                }

                RunLeft();

                RunRight();

                Jump();

                LightningMode();

                Dash(lightningMode);

                Fall();

                Grab();

                Attack(lightningMode);
            }
        }

        public void Fall()
        {
            GetComponent<Collider2D>().enabled = false;
            dropCollider = Physics2D.Raycast((GetComponent<CapsuleCollider2D>().bounds.center + new Vector3(0, 1)), Vector2.down, 2.1f).collider;
            GetComponent<Collider2D>().enabled = true;
            if (Input.GetKey(KeyCode.S) && dropCollider != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), dropCollider, true);
            }
            else if (dropCollider != null)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), dropCollider, false);
            }
        }

        public void RunLeft()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lookDirection = LookDirection.LEFT;
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (this.gameObject.GetComponent<Rigidbody2D>().velocity.x > -xVeloCap && !dashing)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-xForce * 100 * Time.deltaTime, 0));
                }
            }
            else if (this.gameObject.GetComponent<Rigidbody2D>().velocity.x < 0 && !dashing)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(xStoppingForce * 100 * Time.deltaTime * Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x), 0));
            }
        }

        public void RunRight()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                lookDirection = LookDirection.RIGHT;
            }

            if (Input.GetKey(KeyCode.D))
            {
                if (this.gameObject.GetComponent<Rigidbody2D>().velocity.x < xVeloCap && !dashing)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(xForce * 100 * Time.deltaTime, 0));
                }
            }
            else if (this.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0 && !dashing)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(-xStoppingForce * 100 * Time.deltaTime * Mathf.Abs(this.gameObject.GetComponent<Rigidbody2D>().velocity.x), 0));
            }
        }

        public void Jump()
        {
            if (isGrabbing && !dashing)
            {
                jumpTime = 0.5f;
            }
            else if (IsGrounded && !dashing)
            {
                jumpTime = 0.35f;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (jumpTime > 0 && jumpCooldown < 0)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, yForce * 10));
                    jumpTime = 0;
                    jumpCooldown = 1;
                }
            }
            jumpTime -= Time.deltaTime;
            jumpCooldown -= Time.deltaTime;
        }

        public void LightningMode()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (lightningMode == false)
                {
                    gameObject.GetComponent<Health.Health>().SetDamageFactor(gameObject.GetComponent<Health.Health>().damageFactor * 0.75f);
                }
                lightningMode = true;
                lightningResource -= lightningResourceDeplete * Time.deltaTime;
            }
            else
            {
                if (lightningMode == true)
                {
                    gameObject.GetComponent<Health.Health>().SetDamageFactor(gameObject.GetComponent<Health.Health>().damageFactor * (1f / 0.75f));
                }
                lightningMode = false;
            }
        }


        public void Dash(bool lightningMode)
        {
            dashCD -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Mouse1) && dashCD <= 0)
            {
                dashing = true;
                dashCD = 6;
                dashTimer = 1.2f;
                if (lightningResource > lightningResourceDashUse)
                {
                    dashLightning = lightningMode;
                }
                else
                {
                    dashLightning = false;
                }
                if (dashLightning)
                {
                    lightningResource -= lightningResourceDashUse;
                }
                dashOrigin = transform.position;
                dashPhysicsTimer = 0;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                resultsListSelf = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position + Vector3.Normalize(cursorPos - transform.position) * 1.5f, -Vector3.Normalize(cursorPos - transform.position), 1.5f));
                foreach (RaycastHit2D result in resultsListSelf)
                {
                    if (result.collider == GetComponent<Collider2D>())
                    {
                        thisDashOffset = 1.5f - result.distance;
                    }
                }

                GetComponent<Rigidbody2D>().gravityScale = 0;

                GetComponent<Collider2D>().enabled = false;
                RaycastHit2D dashRay = Physics2D.Raycast(transform.position + Vector3.Normalize(cursorPos - transform.position) * thisDashOffset, Vector3.Normalize(cursorPos - transform.position), dashLength);
                resultsListReturn = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position + Vector3.Normalize(cursorPos - transform.position) * (thisDashOffset + 40), -Vector3.Normalize(cursorPos - transform.position), 40));
                resultsList = new List<RaycastHit2D>(Physics2D.RaycastAll(transform.position + Vector3.Normalize(cursorPos - transform.position) * thisDashOffset, Vector3.Normalize(cursorPos - transform.position), 40));
                GetComponent<Collider2D>().enabled = true;

                //calculate dash target
                if (dashLightning)
                {
                    if (dashRay.collider == null)
                    {
                        dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * dashLength;
                        dashTargetObject = null;
                        dashPassthrough = 0;
                    }
                    else
                    {
                        for (int i = 0; i < resultsListReturn.Count; i++)
                        {
                            if (resultsListReturn[i].collider == dashRay.collider)
                            {
                                if (Vector3.Magnitude(resultsListReturn[i].point - dashRay.point) <= maxThickness && dashRay.collider.gameObject.TryGetComponent<IsMetal>(out isMetal))
                                {
                                    firstMetalCollider = dashRay.collider;
                                    dashPassthrough = 1;
                                    dashFirstEntry = transform.position + Vector3.Normalize(cursorPos - transform.position) * dashRay.distance;
                                    dashFirstExit = (Vector3)resultsListReturn[i].point + (Vector3.Normalize(cursorPos - transform.position) * thisDashOffset);
                                    dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * (dashRay.distance + dashLength);
                                    dashTargetObject = null;
                                    foreach (RaycastHit2D result in resultsList)
                                    {
                                        if (i != 0)
                                        {
                                            if (resultsListReturn[i - 1].collider == result.collider)
                                            {
                                                if (result.distance <= dashRay.distance + dashLength)
                                                {
                                                    {
                                                        if (result.collider.gameObject.TryGetComponent<IsMetal>(out isMetal) && Vector3.Magnitude(result.point - resultsListReturn[i - 1].point) <= maxThickness)
                                                        {
                                                            secondMetalCollider = result.collider;
                                                            dashPassthrough = 2;
                                                            dashSecondEntry = transform.position + Vector3.Normalize(cursorPos - transform.position) * result.distance;
                                                            dashSecondExit = (Vector3)resultsListReturn[i - 1].point + (Vector3.Normalize(cursorPos - transform.position) * thisDashOffset);
                                                            dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * (result.distance + dashLength);
                                                            dashTargetObject = null;
                                                            GetComponent<Collider2D>().enabled = false;
                                                            result.collider.enabled = false;
                                                            if (Physics2D.Raycast(result.point, Vector3.Normalize(cursorPos - transform.position), dashLength).collider)
                                                            {
                                                                dashTarget = (Vector3)result.point + (Vector3.Normalize(cursorPos - transform.position) * (Physics2D.Raycast(result.point, Vector3.Normalize(cursorPos - transform.position), dashLength).distance - thisDashOffset));
                                                                dashTargetObject = Physics2D.Raycast(result.point, Vector3.Normalize(cursorPos - transform.position), dashLength).collider.gameObject;
                                                            }
                                                            GetComponent<Collider2D>().enabled = true;
                                                            result.collider.enabled = true;
                                                        }
                                                        else
                                                        {
                                                            dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * result.distance;
                                                            secondMetalCollider = null;
                                                            dashTargetObject = result.collider.gameObject;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * (dashRay.distance + dashLength);
                                                    dashTargetObject = null;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * dashRay.distance;
                                    firstMetalCollider = null;
                                    dashTargetObject = dashRay.collider.gameObject;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dashRay.collider == null)
                    {
                        dashTarget = transform.position + (Vector3.Normalize(cursorPos - transform.position) * dashLength);
                        dashTargetObject = null;
                    }
                    else
                    {
                        dashTarget = transform.position + Vector3.Normalize(cursorPos - transform.position) * dashRay.distance;
                        dashTargetObject = dashRay.collider.gameObject;
                    }
                }
                dashDirection = Vector3.Normalize(dashTarget - transform.position);
                thisDashLength = Vector3.Magnitude(dashTarget - transform.position);
                thisDashAccelTime = dashSpeed / dashAccel;
                thisDashDeccelTime = dashSpeed / dashDeccel;
                thisDashAccelLength = 0.5f * dashAccel * thisDashAccelTime * thisDashAccelTime;
                thisDashDeccelLength = 0.5f * dashDeccel * thisDashDeccelTime * thisDashDeccelTime;
                if (thisDashAccelLength + thisDashDeccelLength > thisDashLength)
                {
                    thisDashStopAccelTime = Mathf.Sqrt((thisDashLength / (thisDashAccelLength + thisDashDeccelLength)) * thisDashAccelTime * thisDashAccelTime);
                    thisDashStartDeccelTime = thisDashStopAccelTime;
                    thisDashStopDeccelTime = thisDashStartDeccelTime + Mathf.Sqrt((thisDashLength / (thisDashAccelLength + thisDashDeccelLength)) * thisDashDeccelTime * thisDashDeccelTime);
                }
                else
                {
                    thisDashStopAccelTime = thisDashAccelTime;
                    thisDashStartDeccelTime = thisDashAccelTime + ((dashLength - (thisDashAccelLength + thisDashDeccelLength)) / dashSpeed);
                    thisDashStopDeccelTime = thisDashStartDeccelTime + thisDashDeccelTime;
                }
            }

            if (dashing)
            {
                dashPhysicsTimer += Time.deltaTime;
                if (dashLightning == true)
                {
                    if (dashPassthrough == 0)
                    {
                        if (dashPhysicsTimer >= thisDashStopDeccelTime)
                        {
                            //stop dash
                            dashing = false;
                            CheckAndBreakWood(dashTargetObject);
                            GetComponent<Rigidbody2D>().gravityScale = 1;
                            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        }
                        else if (dashPhysicsTimer <= thisDashStopDeccelTime && dashPhysicsTimer >= thisDashStartDeccelTime)
                        {
                            //deccelerate
                            GetComponent<Rigidbody2D>().velocity += Vector3Extension.getV3fromV2(-dashDirection * dashDeccel * Time.deltaTime);
                        }
                        else if (dashPhysicsTimer <= thisDashStopAccelTime)
                        {
                            //accelerate
                            GetComponent<Rigidbody2D>().velocity += Vector3Extension.getV3fromV2(dashDirection * dashAccel * Time.deltaTime);
                        }
                    }
                    else
                    {
                        if (ignoreCollisionRan == false)
                        {
                            firstColliderLayer = firstMetalCollider.gameObject.layer;
                            if (secondMetalCollider != null)
                            {
                                secondColliderLayer = secondMetalCollider.gameObject.layer;
                            }
                            playerColliderLayer = gameObject.layer;
                            gameObject.layer = 9;

                            if (firstMetalCollider != null)
                            {
                                firstMetalCollider.gameObject.layer = 10;
                                if (firstMetalCollider.gameObject.TryGetComponent<Health.Health>(out attackTargetHealth))
                                {
                                    if (!attackTargetHealth.isPlayerHealth)
                                    {
                                        attackTargetHealth.Damage(20);
                                    }
                                }
                            }
                            if (secondMetalCollider != null)
                            {
                                secondMetalCollider.gameObject.layer = 10;
                                if (dashPassthrough == 2 && secondMetalCollider.gameObject.TryGetComponent<Health.Health>(out attackTargetHealth))
                                {
                                    if (!attackTargetHealth.isPlayerHealth)
                                    {
                                        attackTargetHealth.Damage(40);
                                    }
                                }
                            }
                            ignoreCollisionRan = true;
                        }

                        if (dashPhysicsTimer >= thisDashStopDeccelTime)
                        {
                            //stop dash
                            dashing = false;
                            CheckAndBreakWood(dashTargetObject);
                            ignoreCollisionRan = false;
                            GetComponent<Rigidbody2D>().gravityScale = 1;
                            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                            gameObject.layer = playerColliderLayer;
                            if (firstMetalCollider != null)
                            {
                                firstMetalCollider.gameObject.layer = firstColliderLayer;
                            }
                            if (secondMetalCollider != null)
                            {
                                secondMetalCollider.gameObject.layer = secondColliderLayer;
                            }
                        }
                        else if (dashPhysicsTimer <= thisDashStopDeccelTime && dashPhysicsTimer >= thisDashStartDeccelTime)
                        {
                            //deccelerate
                            GetComponent<Rigidbody2D>().velocity += Vector3Extension.getV3fromV2(-dashDirection * dashDeccel * Time.deltaTime);
                        }
                        else if (dashPhysicsTimer <= thisDashStopAccelTime)
                        {
                            //accelerate
                            GetComponent<Rigidbody2D>().velocity += Vector3Extension.getV3fromV2(dashDirection * dashAccel * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    if (dashPhysicsTimer >= thisDashStopDeccelTime)
                    {
                        //stop dash
                        dashing = false;
                        CheckAndBreakWood(dashTargetObject);
                        GetComponent<Rigidbody2D>().gravityScale = 1;
                        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    }
                    else if (dashPhysicsTimer <= thisDashStopDeccelTime && dashPhysicsTimer >= thisDashStartDeccelTime)
                    {
                        //deccelerate
                        GetComponent<Rigidbody2D>().velocity += Vector3Extension.getV3fromV2(-dashDirection * dashDeccel * Time.deltaTime);
                    }
                    else if (dashPhysicsTimer <= thisDashStopAccelTime)
                    {
                        //accelerate
                        GetComponent<Rigidbody2D>().velocity += Vector3Extension.getV3fromV2(dashDirection * dashAccel * Time.deltaTime);
                    }
                }
            }
        }

        void Grab()
        {
            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Collider2D>().enabled = false;
                grabBoxcast = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, Vector2.left, 0.25f);
                GetComponent<Collider2D>().enabled = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Collider2D>().enabled = false;
                grabBoxcast = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, Vector2.right, 0.25f);
                GetComponent<Collider2D>().enabled = true;
            }

            if (grabBoxcast.collider != null && grabTimer > 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !dashing)
            {
                if (grabBoxcast.collider.isTrigger == false)
                {
                    //change value if grab timer changed
                    if (grabTimer < 3.5f && Input.GetKeyDown(KeyCode.W))
                    {
                        grabTimer = 0;
                        grabCD = 3;
                    }
                    else
                    {
                        grabTimer -= Time.deltaTime;
                        if (isGrabbing == false)
                        {
                            grabSavedY = transform.position.y;
                        }
                        isGrabbing = true;
                        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                        transform.position = new Vector3(transform.position.x, grabSavedY);
                        grabCD = 3;
                    }
                }
            }
            else
            {
                grabTimer = 0;
                isGrabbing = false;
                grabCD -= Time.deltaTime;
                if (grabCD < 0)
                {
                    grabTimer = 4;
                }
            }
        }

        void Attack(bool lightningMode)
        {
            attackTimer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && attackTimer >= 0.7f)
            {
                attackTimer = 0;
                if (lookDirection == LookDirection.LEFT)
                {
                    GetComponent<Collider2D>().enabled = false;
                    if (Physics2D.Raycast(transform.position, Vector3.left, 3).collider)
                    {
                        if (Physics2D.Raycast(transform.position, Vector3.left, 3).collider.gameObject.TryGetComponent<Health.Health>(out attackTargetHealth))
                        {
                            if (!attackTargetHealth.isPlayerHealth)
                            {
                                if (lightningMode)
                                {
                                    attackTargetHealth.Damage(15);
                                    lightningResource -= lightningResourceAttackUse;
                                }
                                else
                                {
                                    attackTargetHealth.Damage(10);
                                }
                            }
                        }
                    }
                    GetComponent<Collider2D>().enabled = true;
                }
                else if (lookDirection == LookDirection.RIGHT)
                {
                    GetComponent<Collider2D>().enabled = false;
                    if (Physics2D.Raycast(transform.position, Vector3.right, 3).collider)
                    {
                        if (Physics2D.Raycast(transform.position, Vector3.right, 3).collider.gameObject.TryGetComponent<Health.Health>(out attackTargetHealth))
                        {
                            if (!attackTargetHealth.isPlayerHealth)
                            {
                                if (lightningMode)
                                {
                                    attackTargetHealth.Damage(15);
                                }
                                else
                                {
                                    attackTargetHealth.Damage(10);
                                }
                            }
                        }
                    }
                    GetComponent<Collider2D>().enabled = true;
                }
            }
        }

        void CheckAndBreakWood(GameObject dashTargetObject)
        {
            if (dashTargetObject != null)
            {
                if (dashTargetObject.TryGetComponent<IsWood>(out dashWoodObject))
                {
                    dashWoodObject.BreakWood();
                }
            }
        }

        void OnDisable()
        {
            if (gameObject.GetComponent<Health.Health>() != null)
            {
                gameObject.GetComponent<Health.Health>().OnDeath -= PlayerZee_OnDeath;
            }
        }
    }
}
