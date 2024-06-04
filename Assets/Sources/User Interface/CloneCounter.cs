using TMPro;
using UnityEngine;

public class CloneCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _clonesTextCount;

    public void UpdateCounter(int used, int total)
    {
        _clonesTextCount.text = $"{used}/{total}";
    }
}
