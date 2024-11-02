using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignEntitiesOrderInLayer : MonoBehaviour
{
void Update()
    {
        List<Transform> allModels = new List<Transform>();

        // Add children of "enemies" to the list
        GameObject enemies = GameObject.Find("enemies");
        if (enemies != null)
        {
            foreach (Transform child in enemies.transform)
            {
                Transform model = child.Find("model");
                if (model != null && model.GetComponent<SpriteRenderer>() != null)
                {
                    allModels.Add(model);
                }
            }
        }

        // Add children of "player" to the list
        GameObject player = GameObject.Find("player");
        if (player != null) {
            Transform model = player.transform.Find("model");
            if (model != null && model.GetComponent<SpriteRenderer>() != null)
            {
                allModels.Add(model);
            }
        }

        // Sort the list by y-value in descending order
        allModels.Sort((a, b) => b.position.y.CompareTo(a.position.y));

        // Assign sorting order based on y-value
        for (int i = 0; i < allModels.Count; i++)
        {
            SpriteRenderer sr = allModels[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = i;
            }
        }
    }
}
