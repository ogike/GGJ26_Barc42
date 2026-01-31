using Dialogue;
using UI;
using UnityEngine;
using Unity.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Moving")] //###############################################################################################
    public float baseSpeed = 15;

    private bool _isMoving;
    
    [Header("References")] //#####################################################################################################
    public Transform playerSprite;
    public Transform spritePivot;
    public Animator animator;
    
    private Transform _trans;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;


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
        _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _isMoving = false;
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
        }

        _isMoving = _navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", _isMoving);

        if (_isMoving) UpdateMoveRotation();
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


    public void Teleport(Vector3 position)
    {
        _trans.position = position;
    }
}
