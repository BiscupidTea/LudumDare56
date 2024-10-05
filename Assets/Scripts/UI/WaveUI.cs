using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private WaveController _controller;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private float fillDuration = .25f;

    private void OnEnable()
    {
        if (_controller)
        {
            _controller.onCharging += EnableLoadingTime;
            _controller.onCharged += DisableLoadingTime;
            _controller.onLoadPercentage += UpdateLoadTimeFill;
        }
    }

    private void OnDisable()
    {
        _controller.onCharging -= EnableLoadingTime;
        _controller.onCharged -= DisableLoadingTime;
        _controller.onLoadPercentage -= UpdateLoadTimeFill;
    }

    private void EnableLoadingTime()
    {
        loadingBarFill.enabled = true;
        loadingBarFill.fillAmount = 0f;
    }

    private void DisableLoadingTime()
    {
        Invoke(nameof(TurnOffLoadingTime), fillDuration);
    }

    private void TurnOffLoadingTime()
    {
        loadingBarFill.fillAmount = 0f;
        loadingBarFill.enabled = false;
    }

    private void UpdateLoadTimeFill(float percentage)
    {
        if (percentage == 0)
        {
            loadingBarFill.fillAmount = 0f;
            return;
        }


        StartCoroutine(LerpFill(loadingBarFill.fillAmount, percentage));
    }

    private IEnumerator LerpFill(float from, float to)
    {
        var start = Time.time;
        var now = start;
        while (start > now)
        {
            loadingBarFill.fillAmount = Mathf.Lerp(from, to, (now - start));
            yield return null;
            now = Time.time;
        }

        loadingBarFill.fillAmount = to;
    }
}
