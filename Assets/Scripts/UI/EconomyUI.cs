using TMPro;
using UnityEngine;

public class EconomyUI : MonoBehaviour
{
    [SerializeField] private EconomyManager _manager;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _manager.refreshGoldEvent += HandleRefreshGold;
    }

    private void OnDisable()
    {
        _manager.refreshGoldEvent -= HandleRefreshGold;
    }

    private void HandleRefreshGold(int gold)
    {
        _text.text = gold.ToString();
    }
}
