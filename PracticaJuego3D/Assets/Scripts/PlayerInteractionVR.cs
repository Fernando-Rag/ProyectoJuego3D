using UnityEngine;

public class PlayerInteractionVR : MonoBehaviour
{
    [Header("Configuración de Raycast")]
    public Camera playerCamera;
    public float distanciaInteraccion = 5f;
    public LayerMask capasInteractuables;
    
    [Header("Input")]
    public bool usarTouch = false; // Para móvil VR
    
    private InteractableObjectVR objetoActual;
    private RaycastHit hitInfo;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    void Update()
    {
        DetectarObjetoInteractivo();
        
        // Detectar input
        bool inputInteraccion = false;
        
        if (usarTouch)
        {
            // Para VR móvil con touch o botón de Google Cardboard
            inputInteraccion = Input.GetMouseButtonDown(0) || 
                              (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
        }
        else
        {
            // Para PC o VR con controladores
            inputInteraccion = Input.GetMouseButtonDown(0);
        }
        
        if (inputInteraccion && objetoActual != null)
        {
            objetoActual.OnInteract();
        }
    }

    void DetectarObjetoInteractivo()
    {
        // Raycast desde el centro de la cámara (donde mira el usuario)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hitInfo, distanciaInteraccion, capasInteractuables))
        {
            InteractableObjectVR interactable = hitInfo.collider.GetComponent<InteractableObjectVR>();
            
            if (interactable != null)
            {
                if (objetoActual != interactable)
                {
                    if (objetoActual != null)
                    {
                        objetoActual.OnHoverExit();
                    }
                    
                    objetoActual = interactable;
                    objetoActual.OnHoverEnter();
                }
            }
            else
            {
                LimpiarObjetoActual();
            }
        }
        else
        {
            LimpiarObjetoActual();
        }
    }

    void LimpiarObjetoActual()
    {
        if (objetoActual != null)
        {
            objetoActual.OnHoverExit();
            objetoActual = null;
        }
    }

    void OnDrawGizmos()
    {
        if (playerCamera != null)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            Gizmos.color = objetoActual != null ? Color.green : Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * distanciaInteraccion);
        }
    }
}