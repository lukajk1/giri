using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseParticleSystem : MonoBehaviour
{
    private ParticleSystem myParticleSystem;
    void Start() {
        myParticleSystem = gameObject.GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (GameState.Instance.MenusOpen > 0) {
            myParticleSystem.Pause();
        }

        if (myParticleSystem.isPaused && GameState.Instance.MenusOpen == 0){
            myParticleSystem.Play();
        }
    }
}
