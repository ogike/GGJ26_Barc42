using Dialogue;
using UI;
using UnityEngine;
using Unity.AI;

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
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    
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
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _isMoving = false;
        _timeSinceLastMove = 0;
        _timeSinceLastStop = 0;
    }
    
    void Update()
    {
        if(DialogueManager.Instance.dialogueIsPlaying) return;
        //if (PauseMenu.Instance.IsPaused) return;
        
        Move();
    }

    private void Move()
    {
        if (UserInput.Instance.GoToPointPressedThisFrame)
        {
            _navMeshAgent.SetDestination(UserInput.Instance.MoveTarget);
            _isMoving = true;
            _timeSinceLastStop = 0;
            _timeSinceLastMove = 0;
            animator.SetBool("isMoving", true);
        }
        Debug.Log(_navMeshAgent.steeringTarget);

        // Start moving if we havent
        // UpdateMoveRotation(inputH, inputV);
        
        // This is overwriting the full velocity of the Rigidbody system. This is not the best, but gives us the most control.
        // _rigidbody.linearVelocity = newFullForce;
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

        public void Teleport(Vector3 position)
        {
            _rigidbody.position = position;
        }
}
