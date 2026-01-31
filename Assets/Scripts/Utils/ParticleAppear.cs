using System;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAppearOnEnable : MonoBehaviour
    {
        private ParticleSystem _particle;
        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            PlayParticle();
        }
        
        public void PlayParticle(){
            if(!_particle) _particle = GetComponent<ParticleSystem>();
            _particle.Play();
        }
    }
}
