using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 orgPos = transform.localPosition;
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, orgPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = orgPos;
    }
}
