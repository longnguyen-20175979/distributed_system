using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconCountdown : MonoBehaviour
{
    enum IconType { Shield, Magnet }
    [SerializeField] private IconType iconType;

    private Image circle;

    private void OnEnable()
    {
        circle = transform.GetChild(0).GetComponent<Image>();
        circle.fillAmount = 1;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (circle.fillAmount >= 0)
        {
            if (iconType == IconType.Shield)
            {
                circle.fillAmount -= Time.deltaTime / Constants.SHIELD_DURATION;
                yield return new WaitForSeconds(Time.deltaTime / Constants.SHIELD_DURATION);
            }
            else
            {
                circle.fillAmount -= Time.deltaTime / Constants.MAGNET_DURATION;
                yield return new WaitForSeconds(Time.deltaTime / Constants.MAGNET_DURATION);
            }
            if (circle.fillAmount == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
