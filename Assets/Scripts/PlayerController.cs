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
            _timeSinceLastStop = 0;
            _timeSinceLastMove = 0;
        }

        _isMoving = _navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", _isMoving);

        if (_isMoving) UpdateMoveRotation();
        
        // This is overwriting the full velocity of the Rigidbody system. This is not the best, but gives us the most control.
        // _rigidbody.linearVelocity = newFullForce;
    }
    
    private void UpdateMoveRotation()
    {
        Vector2 steeringTarget = _navMeshAgent.steeringTarget;
        Vector2 direction = (steeringTarget - (Vector2)_trans.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _trans.rotation = Quaternion.Euler(0, 0, angle);

        animator.SetFloat("lookH", direction.x);
        animator.SetFloat("lookV", direction.y);
    }

}
