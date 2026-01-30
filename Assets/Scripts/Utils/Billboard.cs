using System;
using UnityEngine;

namespace Utils
{
    public class Billboard : MonoBehaviour
    {
        private Transform _trans;
        private Transform _camTrans;

        private void Start()
        {
            _trans = transform;
            _camTrans = GameManager.Instance.mainCamera;
        }

        private void LateUpdate()
        {
            _trans.forward = _camTrans.forward;
        }
    }
}
