using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affector : MonoBehaviour
{
    [Tooltip("Should the affector set the value directly or change the value by an amount?")]
    public bool relative;
    [Tooltip("Should the affector set/change the value instantly or should it happen over a brief period?")]
    public bool smooth;
    public float smoothTime;

    protected GameObject targetGameObject;

    /// <summary>
    /// Apply an affectors effect to a GameObject.
    /// </summary>
    /// <param name="target">The game object to apply the affector to.</param>
    protected virtual void ApplyAffector(GameObject target)
    {
        targetGameObject = target;
        if (smooth)
        {
            if (relative)
            {
                Debug.Log("Applying affector to: " + target.name + " changing value smoothly.");

            }
            else
            {
                Debug.Log("Applying affector to: " + target.name + " setting value smoothly.");
            }
        }
        else
        {
            if (relative)
            {
                Debug.Log("Applying affector to: " + target.name + " changing value instantly.");

            }
            else
            {
                Debug.Log("Applying affector to: " + target.name + " setting value instantly.");

            }
        }
    }

    /// <summary>
    /// Applies an affector over time.
    /// </summary>
    /// <param name="timeToApply">The amount of time to apply over.</param>
    protected virtual IEnumerator SmoothAffectorApplication(float timeToApply, CharacterStats targetStats)
    {
        float normT = 0.0f;
        while (normT < 1.0f)
        {
            Debug.Log("Lerping affector " + targetGameObject + ", currentTime: " + " normT");
            normT += Time.deltaTime / timeToApply;
            yield return null;
        }
    }

    protected virtual void OnDestroy()
    {
        if (targetGameObject ?? false)
        {
            Debug.Log("Removing affector from: " + targetGameObject);
        }
    }
}
