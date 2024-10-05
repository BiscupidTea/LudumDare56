using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private WaveController _controller;
    [SerializeField] private Image _timer;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private float fillDuration = .25f;

    private void OnEnable()
    {
        if (_controller)
        {
            _controller.onCharging += EnableLoadingTime;
            _controller.onCharged += DisableLoadingTime;
            _controller.onLoadPercentage += UpdateLoadTimeFill;
            _controller.WaveChange += HandleChangeWave;
        }
    }

    private void OnDisable()
    {
        _controller.onCharging -= EnableLoadingTime;
        _controller.onCharged -= DisableLoadingTime;
        _controller.onLoadPercentage -= UpdateLoadTimeFill;
        _controller.WaveChange -= HandleChangeWave;
    }

    private void EnableLoadingTime()
    {
        _timer.gameObject.SetActive(true);
        loadingBarFill.fillAmount = 0f;
    }

    private void DisableLoadingTime()
    {
        Invoke(nameof(TurnOffLoadingTime), fillDuration);
    }

    private void TurnOffLoadingTime()
    {
        loadingBarFill.fillAmount = 0f;
        _timer.gameObject.SetActive(false);
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

    private void HandleChangeWave(int actualWave, int maxWave)
    {
        _waveText.text = $"{actualWave} / {maxWave}";
    }
}
