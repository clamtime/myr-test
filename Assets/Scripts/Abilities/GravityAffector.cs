using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAffector : Affector
{
    public float gravityScale;
    public Vector3 gravityDirection;

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
                    targetStats.ChangeGravity(
                        targetStats.gravityScale + gravityScale,
                        targetStats.gravityDirection + gravityDirection);
                }
                else
                {
                    targetStats.ChangeGravity(gravityScale, gravityDirection);
                }
            }
        }
    }

    protected override IEnumerator SmoothAffectorApplication(float timeToApply, CharacterStats targetStats)
    {
        float normT = 0.0f;
        float originalScale = targetStats.gravityScale;
        Vector3 originalDirection = targetStats.gravityDirection;

        while (normT < 1.0f)
        {
            if (relative)
            {
                targetStats.ChangeGravity( 
                    originalScale + (gravityScale * normT),
                    originalDirection + (gravityDirection * normT));
            }
            else
            {
                targetStats.ChangeGravity(gravityScale * normT, gravityDirection * normT);
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
