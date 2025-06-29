using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    [SerializeField] Transform explosionEffectPrefab;
    [SerializeField] Transform collideParticlesPrefab;
    private Queue<EffectInfo> generatedEffects = new Queue<EffectInfo>();

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        if(generatedEffects.Count > 0)
        {
            EffectInfo pInfo = generatedEffects.Peek();
            if (Time.time >= pInfo.decayTime)
            {
                Destroy(pInfo.tf.gameObject);
                generatedEffects.Dequeue();
            }
        }
    }

    public void GenerateCollideVFX(Vector2 position, Vector2 direction)
    {
        float timeStamp = Time.time;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Transform tf = Instantiate(collideParticlesPrefab, position, Quaternion.AngleAxis(angle, Vector3.forward), transform);
        generatedEffects.Enqueue(new EffectInfo(tf, timeStamp + 2f));
    }

    public void GenerateExplosionVFX(Vector2 position, float lifeTime)
    {
        SFXManager.Instance.PlayExplosionSFX();
        float timeStamp = Time.time;
        Transform tf = Instantiate(explosionEffectPrefab, position, Quaternion.identity, transform);
        generatedEffects.Enqueue(new EffectInfo(tf, timeStamp + lifeTime));
    }

    private struct EffectInfo
    {
        public Transform tf;
        public float decayTime;

        public EffectInfo(Transform _tf, float _decayTime)
        {
            tf = _tf;
            decayTime = _decayTime;
        }
    }
}
