using UnityEngine;

public class HideWalls : MonoBehaviour
{
    void Awake()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
    }
}
