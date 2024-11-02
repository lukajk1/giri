using System.Collections;
using UnityEngine;

public class Cooldown : MonoBehaviour {
    public float Duration { get; set; }
    private float timeElapsed;
    public float TimeElapsed => timeElapsed;

    public bool IsUp = true;
    public float Progress;
    public void Initiate()
    {
        StartCoroutine(CooldownCR());
    }
    private IEnumerator CooldownCR()
    {
        IsUp = false;
        //timeElapsed = 0f;
        while (timeElapsed < Duration)
        {
            while (GameState.Instance.MenusOpen > 0) yield return null;
            Progress = timeElapsed / Duration;
            yield return new WaitForSeconds(0.01f);
            timeElapsed += 0.01f;
        }
        IsUp = false;
        Destroy(this);
    }
}
