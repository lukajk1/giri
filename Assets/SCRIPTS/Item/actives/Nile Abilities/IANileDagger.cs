using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IANileDagger : ItemActivatable
{
    private Nile nile;
    protected override void Start()
    {
        base.Start();
        if (FindAnyObjectByType<Nile>() == null)
        {
            Debug.LogError("did not find nile class in scene. This ability will not work.");
        }
        else
        {
            nile = FindFirstObjectByType<Nile>();
        }
    }

    public override bool Activate()
    {
        GameObject atk = Instantiate(nile.Dagger, GameState.Instance.Player.AttackOriginMarker.position, Quaternion.identity);
        atk.GetComponent<NileDagger>().Initialize(GetCursorWorldCoords(), nile.GetComponent<PlayerUnit>());

        return true;
    }

}
