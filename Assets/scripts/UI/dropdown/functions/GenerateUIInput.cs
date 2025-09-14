using UnityEngine;
using TMPro;

public class GenerateUIInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField widthInput, heightInput;
    public void GenerateMap()
    {
        int.TryParse(widthInput.text, out int m_width);
        int.TryParse(heightInput.text, out int m_height);

        EventBus.OnMapGenrated?.Invoke(m_width, m_height);
    }
}
