using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balacontroller : MonoBehaviour
{
    public float velocidad = 10f;
    private Vector2 direccion;
    public float tiempoVida = 5f; // Tiempo en segundos antes de que la bala se autodestruya

    // Start is called before the first frame update
    void Start()
    {
        // Aplicar la dirección y velocidad al Rigidbody2D
        if (direccion != Vector2.zero)
        {
            GetComponent<Rigidbody2D>().velocity = direccion * velocidad;
        }
        else
        {
            Debug.LogWarning("Dirección de la bala no establecida.");
        }

        // Destruir la bala después de un tiempo de vida para evitar que quede indefinidamente
        Destroy(gameObject, tiempoVida);
    }

    public void EstablecerDireccion(Vector2 nuevaDireccion)
    {
        // Ajustar la escala para orientar la bala correctamente
        transform.localScale = new Vector3(nuevaDireccion.x, transform.localScale.y, transform.localScale.z);

        // Normalizar la dirección y asignarla a la variable
        direccion = nuevaDireccion.normalized;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Pausar el juego o aplicar efectos adicionales
            Time.timeScale = 0f;

            // Aquí puedes llamar a tu GameManager o cualquier otro sistema para restar vida al jugador
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.QuitarVidaJugador();
            }
            else
            {
                Debug.LogWarning("GameManager no encontrado en la escena.");
            }
        }

        // Destruir la bala al colisionar con cualquier objeto
        Destroy(gameObject);
    }
}