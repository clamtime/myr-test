using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAffector : Affector
{
    public Color color;

    public override void ApplyAffector(GameObject target)
    {
        transform.SetParent(target.transform);
        targetGameObject = target;
        if (target.TryGetComponent(out CharacterStats targetStats))
        {
            if (smooth)
            {
                StartCoroutine(SmoothAffectorApplication(smoothTime, targetStats));
            }
            else
            {
                if (relative)
                {
                    targetStats.ChangeColor(targetStats.color + color);
                }
                else
                {
                    targetStats.ChangeColor(color);
                }
            }
        }
    }

    protected override IEnumerator SmoothAffectorApplication(float timeToApply, CharacterStats targetStats)
    {
        float normT = 0.0f;
        Color originalColor = targetStats.color;

        while (normT < 1.0f)
        {
            if (relative)
            {
                targetStats.ChangeColor(originalColor + (color * normT));
            }
            else
            {
                targetStats.ChangeColor(color * normT);
            }

            normT += Time.deltaTime / timeToApply;
            yield return null;
        }
    }

    protected override void OnDestroy()
    {
        if (targetGameObject ?? false)
        {

        }
    }
}
