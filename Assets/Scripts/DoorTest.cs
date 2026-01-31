using UnityEngine;
using UnityEngine.AI;

public class DoorTest : MonoBehaviour
{
    private bool isOpen = false;
    private NavMeshObstacle _navMeshObstacle;
    private SpriteRenderer _spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            isOpen = !isOpen;
            Debug.Log("Door " + (isOpen ? "Opened" : "Closed"));
        }
        _navMeshObstacle.carving = !isOpen;
        _navMeshObstacle.enabled = !isOpen;
        _spriteRenderer.enabled = !isOpen;
    }
}
