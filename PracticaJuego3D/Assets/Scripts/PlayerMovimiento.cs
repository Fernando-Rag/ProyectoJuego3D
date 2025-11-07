using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovimiento : MonoBehaviour
{
    [SerializeField] private bool UsarGetAxisRaw = true;
    [SerializeField] private Transform camara;
    [SerializeField] private float velocidadMovimiento = 5f;
    private CharacterController controlador;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();
        // Si por alguna razón no existe, RequireComponent normalmente lo añade automáticamente en el editor,
        // pero en tiempo de ejecución podemos crearlo para evitar crashes (opcional):
        if (controlador == null)
            controlador = gameObject.AddComponent<CharacterController>();

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;
    }

    void Start()
    {
        // Comprobación informativa
        if (controlador == null)
            Debug.LogError("CharacterController no encontrado/añadido al GameObject.");
    }

    void Update()
    {
        if (controlador == null) return; // seguridad

        // CORRECCIÓN: usar el nombre correcto UsarGetAxisRaw y usar GetAxisRaw cuando corresponda
        float ValorHorizontal = UsarGetAxisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float ValorVertical   = UsarGetAxisRaw ? Input.GetAxisRaw("Vertical")   : Input.GetAxis("Vertical");

        if (camara == null)
        {
            // Si no hay cámara asignada, mover en base al mundo (fallback)
            Vector3 moveFallback = new Vector3(ValorHorizontal, 0f, ValorVertical);
            controlador.Move(moveFallback * velocidadMovimiento * Time.deltaTime);
            return;
        }

        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara  = camara.right;

        adelanteCamara.y = 0f;
        derechaCamara.y  = 0f;

        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        Vector3 direccionplano = (derechaCamara * ValorHorizontal + adelanteCamara * ValorVertical);

        if (direccionplano.sqrMagnitude > 0.0001f)
            direccionplano.Normalize();

        Vector3 desplazamientoXZ = direccionplano * (velocidadMovimiento * Time.deltaTime);

        controlador.Move(desplazamientoXZ);

        //Debug.Log($"ValorHorizontal: {ValorHorizontal:F1} | ValorVertical: {ValorVertical:F1}");
    }
}
