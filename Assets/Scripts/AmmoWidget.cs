using UnityEngine;
using TMPro;
public class AmmoWidget : MonoBehaviour
{
    public TMPro.TMP_Text ammoText;

    public void Refresh(int ammoCount, int clipSize)
    {
        ammoText.text = ammoCount + " / " + clipSize;
    }
}
