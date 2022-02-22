using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clickText;
    [SerializeField] private string dropItemText;
    [SerializeField] private string collectItemText;

    public void SetClickText(bool isCollecting)
    {
        clickText.SetText(isCollecting ? collectItemText : dropItemText);
        clickText.fontStyle = (FontStyles) (isCollecting ? FontStyle.Bold : FontStyle.Normal);
    }
    
}