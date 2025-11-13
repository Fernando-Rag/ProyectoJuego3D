using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelInformacion;
    public TextMeshProUGUI textoInformacion; 
    public Button botonCerrar;
    
    [Header("Cursor")]
    public Image imagenCursor;
    
    [Header("Configuración")]
    public bool pausarJuegoAlMostrar = true;
    
    private bool panelActivo = false;
    
    void Start()
    {
        if (panelInformacion != null)
        {
            panelInformacion.SetActive(false);
        }
        
        if (botonCerrar != null)
        {
            botonCerrar.onClick.AddListener(CerrarInformacion);
        }
    }
    
    void Update()
    {
        // Permitir cerrar con ESC
        if (panelActivo && Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarInformacion();
        }
    }

    public void MostrarInformacion(string mensaje)
    {
        if (panelInformacion != null && textoInformacion != null)
        {
            textoInformacion.text = mensaje;
            panelInformacion.SetActive(true);
            panelActivo = true;
            
            if (pausarJuegoAlMostrar)
            {
                Time.timeScale = 0f;
            }
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            if (imagenCursor != null)
            {
                imagenCursor.enabled = false;
            }
        }
    }

    public void CerrarInformacion()
    {
        if (panelInformacion != null)
        {
            panelInformacion.SetActive(false);
            panelActivo = false;
            
            if (pausarJuegoAlMostrar)
            {
                Time.timeScale = 1f;
            }
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            if (imagenCursor != null)
            {
                imagenCursor.enabled = true;
            }
        }
    }
    
    public bool EstaPanelActivo()
    {
        return panelActivo;
    }
}