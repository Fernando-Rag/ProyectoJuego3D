using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [TextArea(3, 10)]
    public string mensajeInformacion = "Información del objeto";
    
    [Header("Visual Feedback")]
    public Color colorHighlight = Color.yellow;
    private Color colorOriginal;
    private Renderer objectRenderer;
    
    private bool isHighlighted = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            colorOriginal = objectRenderer.material.color;
        }
    }

    public void OnHoverEnter()
    {
        if (!isHighlighted && objectRenderer != null)
        {
            objectRenderer.material.color = colorHighlight;
            isHighlighted = true;
        }
    }

    public void OnHoverExit()
    {
        if (isHighlighted && objectRenderer != null)
        {
            objectRenderer.material.color = colorOriginal;
            isHighlighted = false;
        }
    }

    public void OnInteract()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.MostrarInformacion(mensajeInformacion);
        }
        
        Debug.Log("Interactuando con: " + gameObject.name);
    }
}