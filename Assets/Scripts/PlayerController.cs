using UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Moving")] //###############################################################################################
    public float baseSpeed = 15;

    public float accelerationTime;
    public AnimationCurve accelerationCurve;
    public float deaccelerationTime;
    public AnimationCurve deaccelerationCurve;

    private float plusRotValue;

    private float lastInputH;
    private float lastInputV;
    private bool rotatedThisUpdate = false;

    private float _timeSinceLastStop;
    private float _timeSinceLastMove;
    private bool _isMoving;
    
    [Header("References")] //#####################################################################################################
    public Transform playerSprite;
    public Transform spritePivot;
    public Animator animator;
    
    private Transform _trans;
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    
    private float _floatingTolerance = 0.001f;
    private Vector2 _last4WayDir;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"More than one {this} in scene!");
            return;
        }

        Instance = this;
    }
    
    void Start()
    {
        _trans = transform;
        _collider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _isMoving = false;
        _timeSinceLastMove = 0;
        _timeSinceLastStop = 0;
    }
    
    void Update()
    {
        //if (PauseMenu.Instance.IsPaused) return;

        //Do things that are always done first
        UpdateCooldowns();
        
        Move();

        if (!_isMoving && _timeSinceLastMove < deaccelerationTime)
        {
            UpdateDeacceleration();
        }
    }

    private void Move()
    {
        //get basic input dir
        float inputH = UserInput.Instance.MoveInput.x;
        float inputV = UserInput.Instance.MoveInput.y;

        //reset movement if no input
        if (inputH == 0 && inputV == 0)
        {
            if (_isMoving)
            {
                _isMoving = false;
                _timeSinceLastMove = 0;
                _timeSinceLastStop = 0;
                    
                animator.SetBool("isMoving", false);
            }

            return; // We dont move
        }

        // Start moving if we havent
        if (!_isMoving)
        {
            _isMoving = true;
            _timeSinceLastStop = 0;
            _timeSinceLastMove = 0;
            animator.SetBool("isMoving", true);
        }
            
        UpdateMoveRotation(inputH, inputV);
        
        //move if there is input
        float finalSpeed = baseSpeed;
            
        if (_timeSinceLastStop < accelerationTime)
        {
            float percent = _timeSinceLastStop / accelerationTime;
            finalSpeed *= accelerationCurve.Evaluate(percent);
        }

        Vector2 newFullForce = new Vector2(inputH, inputV) * finalSpeed;
        // _rigidbody.AddForce(newFullForce);
        
        // This is overwriting the full velocity of the Rigidbody system. This is not the best, but gives us the most control.
        _rigidbody.linearVelocity = newFullForce;
    }
    
    private void UpdateCooldowns()
    {
        if (_isMoving)
        {
            _timeSinceLastStop += Time.deltaTime;
        }
        else
        {
            _timeSinceLastMove += Time.deltaTime;
        }
    }
    
    private void UpdateMoveRotation(float inputH, float inputV)
    {
        // make it so look rot stays
        if (Mathf.Abs(inputH - lastInputH) > _floatingTolerance ||
            Mathf.Abs(inputV - lastInputV) > _floatingTolerance)
        {
            // if (inputH == 0 && inputV == 0)
            //     plusRotValue = 0;
            // else
            //     plusRotValue = 90;

            float lookH = inputH;
            float lookV = inputV;
        
            //restrict diagonal
            if (Mathf.Abs(inputH) > 0 && Mathf.Abs(inputV) > 0)
            {
                //dont rotate
                lookH = _last4WayDir.x;
                lookV = _last4WayDir.y;
            }
            else
            {
                _last4WayDir.x = lookH;
                _last4WayDir.y = lookV;
            }
        
            float rotZ = Mathf.Atan2(lookV, lookH) * Mathf.Rad2Deg;
        
            float finalRot = rotZ - plusRotValue;
            _trans.rotation = Quaternion.Euler(0, 0, finalRot);
            rotatedThisUpdate = true;

            lastInputH = inputH;
            lastInputV = inputV;
        
            SetMecanimRotation(lookH, lookV);
        }
    }

        private void UpdateDeacceleration()
        {
            float finalSpeed = baseSpeed;

            if (_timeSinceLastMove < deaccelerationTime)
            {
                float percent = _timeSinceLastMove / deaccelerationTime;
                finalSpeed *= deaccelerationCurve.Evaluate(percent);
            }
            
            Vector2 newFullForce = new Vector2(lastInputH, lastInputV) * finalSpeed;
            // _rigidbody.AddForce(newFullForce);
        
            // This is overwriting the full velocity of the Rigidbody system. This is not the best, but gives us the most control.
            _rigidbody.linearVelocity = newFullForce;
        }
        
        public void RecenterToSpritePivot()
        {
            Vector3 newPos = spritePivot.position;
            _trans.position = newPos;
            spritePivot.position = newPos;
        }

        private void SetMecanimRotation(float inputH, float inputV)
        {
            float absH = Mathf.Abs(inputH);
            float absV = Mathf.Abs(inputV);

            Vector2 finalVec = Vector2.zero;

            if (absH > absV)
            {
                finalVec = inputH > 0 ? Vector2.right : Vector2.left;
            }
            else if (absH < absV)
            {
                finalVec = inputV > 0 ? Vector2.up : Vector2.down;
            }
        
            animator.SetFloat("lookH", finalVec.x);
            animator.SetFloat("lookV", finalVec.y);
        }
}
