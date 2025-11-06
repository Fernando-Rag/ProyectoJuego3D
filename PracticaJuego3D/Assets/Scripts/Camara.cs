using UnityEngine;

public class Camara : MonoBehaviour
{
    public float Sensibilidad = 100f;
    public Transform Player;

    public float RotacionHorizontal = 0f;
    public float RotacionVertical = 0f;

    void Start()
    {






    }

   





    void Update()
    {
        float ValorX = Input.GetAxis("Mouse X") * Sensibilidad * Time.deltaTime;
        float ValorY = Input.GetAxis("Mouse Y") * Sensibilidad * Time.deltaTime;

        RotacionHorizontal += ValorX;
        RotacionVertical -= ValorY;

        RotacionVertical = Mathf.Clamp(RotacionVertical, -80f, 80f);

        transform.localRotation = Quaternion.Euler(RotacionVertical,0f,0f);

        Player.Rotate(Vector3.up * ValorX);



    }
}
