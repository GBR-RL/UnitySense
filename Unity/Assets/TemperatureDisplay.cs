using UnityEngine;
using TMPro;

public class TemperatureDisplay : MonoBehaviour
{

    private TextMeshPro textMeshPro;  // Reference to the TextMeshPro component for displaying text.

    // Called when the script instance is being loaded.
    void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();  // Initializing the TextMeshPro component attached to the same GameObject.
    }

    // Public method to update the temperature display text.
    public void UpdateTemperature(float temperature)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = $"{temperature:F1} °C";  // Updating the text with the temperature
            Debug.Log($"Updated temperature: {temperature:F1} °C");
        }
    }
}
