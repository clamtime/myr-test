using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAffector : Affector
{
    public Vector3 size;

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
                    targetStats.ChangeSize(targetStats.size + size);
                }
                else
                {
                    targetStats.ChangeSize(size);
                }
            }
        }
    }

    protected override IEnumerator SmoothAffectorApplication(float timeToApply, CharacterStats targetStats)
    {
        float normT = 0.0f;
        Vector3 originalSize = targetStats.size;

        while (normT < 1.0f)
        {
            if (relative)
            {
                targetStats.ChangeSize(originalSize + (size * normT));
            }
            else
            {
                targetStats.ChangeSize(size * normT);
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
