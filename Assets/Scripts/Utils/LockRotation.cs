using UnityEngine;

namespace Utils
{
    public class LockRotation : MonoBehaviour
    {
        private Transform _transform;
        private Quaternion _lockedRotation;

        void Start()
        {
            _transform = GetComponent<Transform>();
            _lockedRotation = transform.rotation;
        }

        void LateUpdate()
        {
            _transform.rotation = _lockedRotation;
        }
    }
}
