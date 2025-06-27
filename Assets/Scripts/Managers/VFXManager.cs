using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [SerializeField] Transform collideParticlesPrefab;
    private Queue<ParticleInfo> particles = new Queue<ParticleInfo>();

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        if(particles.Count > 0)
        {
            ParticleInfo pInfo = particles.Peek();
            if (Time.time >= pInfo.decayTime)
            {
                Destroy(pInfo.tf.gameObject);
                particles.Dequeue();
            }
        }
    }

    public void GenerateCollideVFX(Vector2 position, Vector2 direction)
    {
        float timeStamp = Time.time;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Transform tf = Instantiate(collideParticlesPrefab, position, Quaternion.AngleAxis(angle, Vector3.forward), transform);
        particles.Enqueue(new ParticleInfo(tf, timeStamp));
    }

    private struct ParticleInfo
    {
        public Transform tf;
        public float decayTime;

        public ParticleInfo(Transform _tf, float _generateTime)
        {
            tf = _tf;
            decayTime = _generateTime + 2f;
        }
    }
}
