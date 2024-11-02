using System.Collections;
using System.Collections.Generic;
using TMPro; // Para usar TextMeshPro
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameManager gameManager; // Asegúrate de que esta referencia esté asignada en el Inspector
    public TextMeshProUGUI monedas; // Referencia a las monedas
    public TextMeshProUGUI cantVidas; // Referencia a las vidas, asegúrate de que esté asignada

    private Button botonPausa;
    private Button botonContinuar;

    private void Start()
    {
        // Busca el botón de pausa y agrega el listener
        GameObject botonesContenedor = GameObject.Find("--- Botones ---");

        if (botonesContenedor != null)
        {
            botonPausa = botonesContenedor.transform.Find("pause")?.GetComponent<Button>();
            if (botonPausa != null)
            {
                botonPausa.onClick.AddListener(gameManager.PausarJuego);
            }
            else
            {
                Debug.LogWarning("No se encontró el botón de pausa 'pause'.");
            }

            // Encuentra el botón de continuar
            botonContinuar = botonesContenedor.transform.Find("play")?.GetComponent<Button>();
            if (botonContinuar != null)
            {
                botonContinuar.onClick.AddListener(gameManager.ReanudarJuego);
            }
            else
            {
                Debug.LogWarning("No se encontró el botón de continuar 'play'.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el contenedor de botones '--- Botones ---'.");
        }
    }

    private void Update()
    {
        ActualizarHUD();
    }

    private void ActualizarHUD()
    {
        // Actualiza el texto de monedas en pantalla
        if (monedas != null)
        {
            monedas.text = GameData.monedas.ToString(); // Cambiado para acceder a GameData
        }
        else
        {
            Debug.LogError("La referencia a monedas no está asignada.");
        }

        // Actualiza el texto de vidas en pantalla
        if (cantVidas != null)
        {
            cantVidas.text = "X " + GameData.vidasJugador.ToString(); // Cambiado para acceder a GameData
        }
        else
        {
            Debug.LogError("La referencia a cantVidas no está asignada.");
        }
    }
}
