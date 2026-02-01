using UnityEngine;

public class CodeDoor : MonoBehaviour
{
private bool isOpen = false;
    private UnityEngine.AI.NavMeshObstacle _navMeshObstacle;
    private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navMeshObstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _navMeshObstacle.carving = !isOpen;
        _navMeshObstacle.enabled = !isOpen;
        _spriteRenderer.enabled = !isOpen;
    }

    public void Open()
    {
        isOpen = true;
        Debug.Log("Door Opened via CodeDoor.Open()");
    }
}
