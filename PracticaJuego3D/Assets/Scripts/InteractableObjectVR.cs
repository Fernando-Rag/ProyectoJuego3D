using UnityEngine;

public class InteractableObjectVR : MonoBehaviour
{
    [Header("Configuración de Información")]
    [TextArea(3, 10)]
    public string mensajeInformacion = "Información del objeto";
    
    [Header("UI 3D")]
    public GameObject panelInfoPrefab; // El prefab del panel 3D
    private GameObject panelInfoInstancia;
    private bool panelActivo = false;
    
    [Header("Posición del Panel")]
    public Vector3 offsetPosicion = new Vector3(0, 1.5f, 0); // Arriba del objeto
    public bool mirarHaciaJugador = true;
    
    [Header("Visual Feedback")]
    public Color colorHighlight = Color.yellow;
    private Color colorOriginal;
    private Renderer objectRenderer;
    private bool isHighlighted = false;
    
    private Transform jugador;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            colorOriginal = objectRenderer.material.color;
        }
        
        // Buscar al jugador/cámara
        if (Camera.main != null)
        {
            jugador = Camera.main.transform;
        }
    }

    void Update()
    {
        // Si el panel está activo y debe mirar al jugador
        if (panelActivo && panelInfoInstancia != null && mirarHaciaJugador && jugador != null)
        {
            // Hacer que el panel siempre mire al jugador
            Vector3 direccion = jugador.position - panelInfoInstancia.transform.position;
            direccion.y = 0; // Mantener el panel vertical
            if (direccion != Vector3.zero)
            {
                panelInfoInstancia.transform.rotation = Quaternion.LookRotation(direccion);
            }
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
        if (!panelActivo)
        {
            MostrarPanel();
        }
        else
        {
            OcultarPanel();
        }
    }

    void MostrarPanel()
    {
        if (panelInfoPrefab != null)
        {
            // Crear el panel en el mundo
            Vector3 posicion = transform.position + offsetPosicion;
            panelInfoInstancia = Instantiate(panelInfoPrefab, posicion, Quaternion.identity);
            
            // Configurar el texto del panel
            InfoPanel3D infoPanelScript = panelInfoInstancia.GetComponent<InfoPanel3D>();
            if (infoPanelScript != null)
            {
                infoPanelScript.ConfigurarTexto(mensajeInformacion);
            }
            
            // Hacer que mire al jugador inicialmente
            if (mirarHaciaJugador && jugador != null)
            {
                Vector3 direccion = jugador.position - panelInfoInstancia.transform.position;
                direccion.y = 0;
                if (direccion != Vector3.zero)
                {
                    panelInfoInstancia.transform.rotation = Quaternion.LookRotation(direccion);
                }
            }
            
            panelActivo = true;
            Debug.Log("Panel mostrado: " + gameObject.name);
        }
    }

    void OcultarPanel()
    {
        if (panelInfoInstancia != null)
        {
            Destroy(panelInfoInstancia);
            panelInfoInstancia = null;
            panelActivo = false;
            Debug.Log("Panel ocultado: " + gameObject.name);
        }
    }

    void OnDestroy()
    {
        // Limpiar el panel si el objeto se destruye
        if (panelInfoInstancia != null)
        {
            Destroy(panelInfoInstancia);
        }
    }
}