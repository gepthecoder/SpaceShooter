using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image foregroundImage;
    [SerializeField] private float fillSpeedInSec = .5f;

    private void Awake()
    {

        GetComponentInParent<EnemyHealth>().OnHealthPctChanged += EnemyHealth_OnHealthPctChanged;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0f, 180f, 0f);
    }

    private void EnemyHealth_OnHealthPctChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < fillSpeedInSec)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangePct, pct, elapsed / fillSpeedInSec);
            yield return null;
        }
        foregroundImage.fillAmount = pct;
    }
}
