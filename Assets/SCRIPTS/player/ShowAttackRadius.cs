using UnityEngine;

public class ShowAttackRadius : MonoBehaviour
{
    [SerializeField] private GenAttackRadius attackRadiusScript;
    private GameObject attackRadius;
    private bool isShowingRadius = false;

    void Start()
    {
        attackRadius = attackRadiusScript.gameObject;
        attackRadius.SetActive(false);
    }

    void Update()
    {
        bool shouldShowRadius = Input.GetKey(GameState.Instance.KeyMapInstance.KeyMap["show attack radius"]) && GameState.Instance.MenusOpen == 0;

        if (shouldShowRadius && !isShowingRadius)
        {
            // Generate and show the radius only when we start showing it
            attackRadiusScript.GenerateAttackRadius();
            attackRadius.SetActive(true);
            isShowingRadius = true;
        }
        else if (!shouldShowRadius && isShowingRadius)
        {
            // Hide the radius when we should no longer show it
            attackRadius.SetActive(false);
            isShowingRadius = false;
        }
    }
}