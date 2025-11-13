using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Raycast")]
    public Camera playerCamera;
    public float distanciaInteraccion = 3f;
    public LayerMask capasInteractuables;
    
    private InteractableObject objetoActual;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        // Bloquear y ocultar el cursor del sistema
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        DetectarObjetoInteractivo();
        
        // Detectar clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            if (objetoActual != null)
            {
                objetoActual.OnInteract();
            }
        }
    }

    void DetectarObjetoInteractivo()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaInteraccion, capasInteractuables))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            
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
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * distanciaInteraccion);
        }
    }
}