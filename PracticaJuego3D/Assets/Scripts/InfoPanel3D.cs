using UnityEngine;
using TMPro;

public class InfoPanel3D : MonoBehaviour
{
    [Header("Referencias")]
    public TextMeshPro textoMensaje;
    
    [Header("Animación (Opcional)")]
    public bool animarEntrada = true;
    public float duracionAnimacion = 0.3f;
    
    private Vector3 escalaOriginal;
    private float tiempoTranscurrido = 0f;

    void Start()
    {
        if (animarEntrada)
        {
            escalaOriginal = transform.localScale;
            transform.localScale = Vector3.zero;
        }
    }

    void Update()
    {
        if (animarEntrada && tiempoTranscurrido < duracionAnimacion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / duracionAnimacion;
            transform.localScale = Vector3.Lerp(Vector3.zero, escalaOriginal, progreso);
        }
    }

    public void ConfigurarTexto(string texto)
    {
        if (textoMensaje != null)
        {
            textoMensaje.text = texto;
        }
        else
        {
            Debug.LogWarning("textoMensaje no está asignado en InfoPanel3D");
        }
    }
}