using UnityEngine;

// Asegura que el GameObject tenga un CharacterController
[RequireComponent(typeof(CharacterController))]
public class PlayerMovimiento : MonoBehaviour
{
    // Referencias
    [Header("Referencias")]
    [SerializeField] private Transform camara;
    private CharacterController controlador;

    // Movimiento
    [Header("Movimiento")]
    [SerializeField] private bool UsarGetAxisRaw = true;
    [SerializeField] private float velocidadMovimiento = 6f;

    // Correr (sprint)
    [Header("Correr")]
    [Tooltip("Multiplicador aplicado sobre velocidadMovimiento cuando se corre (Shift)")]
    [SerializeField] private float multiplicadorCorrer = 2f;
    [SerializeField] private bool permitirCorrer = true;
    private bool estaCorriendo = false;
    public bool EstaCorriendo => estaCorriendo;

    // Gravedad
    [Header("Gravedad")]
    [SerializeField] private float Gravedad = -60f;
    private Vector3 velocidadVertical;

    // Salto
    [Header("Salto")]
    [SerializeField] private float Salto = 3f;


    // Estado del jugador
    [Header("Estado")]
    [SerializeField] private bool EstandoEnSuelo = false;
    public bool EstaEnSuelo => EstandoEnSuelo;





    // Awake se llama cuando la instancia del script se carga
    private void Awake()
    {
        controlador = GetComponent<CharacterController>();
        if (controlador == null)
            controlador = gameObject.AddComponent<CharacterController>();

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;
    }





    // Start se llama antes de la primera actualización del frame
    void Start()
    {
        if (controlador == null)
            Debug.LogError("CharacterController no encontrado/añadido al GameObject.");
    }






    // Actualizar se llama una vez por frame
    void Update()
    {
        if (controlador == null) return;

        MoverJugadorEnPlano();

        // Actualizamos el estado de suelo justo después del movimiento horizontal
        EstandoEnSuelo = controlador.isGrounded;

        Saltar();

        AplicarGravedad();
    }





    // Mover el jugador en el plano horizontal
    private void MoverJugadorEnPlano()
    {
        if (controlador == null) return; // seguridad

        float ValorHorizontal = UsarGetAxisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float ValorVertical   = UsarGetAxisRaw ? Input.GetAxisRaw("Vertical")   : Input.GetAxis("Vertical");

        // Detectar si se está manteniendo Shift (correr). Usamos GetKey para mantener mientras se presiona.
        if (permitirCorrer)
            estaCorriendo = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        else
            estaCorriendo = false;

        float velocidadActual = velocidadMovimiento * (estaCorriendo ? multiplicadorCorrer : 1f);

        if (camara == null)
        {
            Vector3 moveFallback = new Vector3(ValorHorizontal, 0f, ValorVertical);
            controlador.Move(moveFallback * velocidadActual * Time.deltaTime);
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

        Vector3 desplazamientoXZ = direccionplano * (velocidadActual * Time.deltaTime);

        controlador.Move(desplazamientoXZ);
    }





    // Aplicar gravedad al jugador
    private void AplicarGravedad()
    {
        velocidadVertical.y += Gravedad * Time.deltaTime;
        controlador.Move (velocidadVertical * Time.deltaTime);

        // Evitar acumulación excesiva de velocidad hacia abajo
        if (controlador.isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f;
        }

        // Actualizamos también aquí por si la comprobación del suelo cambió con el movimiento vertical
        EstandoEnSuelo = controlador.isGrounded;
    }





    // Saltar
    public void Saltar()
    {
        if (Input.GetButtonDown("Jump") && EstandoEnSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(-2f * Gravedad * Salto); // Altura de salto
        }
    }



}