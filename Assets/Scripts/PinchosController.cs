using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchosController : MonoBehaviour
{
    public GameManager gameManager;
    public float knockbackDuration = 1f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            
            // Desactivar el movimiento del jugador cuando colisiona con los pinchos
            playerController.DesactivarMovimiento(knockbackDuration);
              Time.timeScale = 0f;

            // Quitar vida al jugador
            gameManager.QuitarVidaJugador();
        }
    }
}
