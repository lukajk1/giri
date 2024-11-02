using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private Cooldown cd;
    [SerializeField] private float delay = 3f;
    void Start()
    {
        cd = gameObject.AddComponent<Cooldown>();
        cd.Duration = delay;
        cd.Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        if (cd == null)
        {
            Destroy(gameObject);
        }
    }
}
