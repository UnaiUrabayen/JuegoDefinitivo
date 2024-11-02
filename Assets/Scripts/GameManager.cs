using System.Collections;
using System.Collections.Generic;
using TMPro; // Para usar TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int VidasTotales = 2; // Total de vidas disponibles
    public Image corazon; // UI para mostrar el corazón
    public Sprite corazonLleno; // Corazón lleno
    public Sprite corazonVacio; // Corazón vacío
    public TextMeshProUGUI cantVidas; // Texto para mostrar las vidas

    private GameObject botonPausa; // Botón de pausa
    private GameObject botonContinuar; // Botón de continuar
    private GameObject personaje; // Referencia al jugador

    void Start()
    {
        // Asegúrate de que las vidas se establecen al comienzo
        GameData.vidasJugador = Mathf.Clamp(GameData.vidasJugador, 0, VidasTotales);
        ActualizarCorazon();
        ActualizarTextoVidas();

        GameObject botonesContenedor = GameObject.Find("--- Botones ---");

        if (botonesContenedor != null)
        {
            botonPausa = botonesContenedor.transform.Find("pause")?.gameObject;
            botonContinuar = botonesContenedor.transform.Find("play")?.gameObject;
        }

        if (botonPausa != null) botonPausa.SetActive(true);
        if (botonContinuar != null) botonContinuar.SetActive(false);

        personaje = GameObject.FindGameObjectWithTag("Player");
    }

    public void SumarMonedas(int monedasASumar)
    {
        GameData.monedas += monedasASumar;

        while (GameData.monedas >= 10)
        {
            GameData.vidasJugador++;
            GameData.monedas -= 10;
            ActualizarCorazon();
            ActualizarTextoVidas();
        }
    }

    public void QuitarVidaJugador()
    {
        if (GameData.vidasJugador > 0)
        {
            GameData.vidasJugador--;
            ActualizarCorazon();
            ActualizarTextoVidas();

           GameOver();
            
        }
        if(GameData.vidasJugador<=0){
             reiniciaVariables();
            CambiarEscena("Inicial");
   
        }
        
    }

void reiniciaVariables(){
          GameData.vidasJugador=2;
          GameData.monedas=0;
          Time.timeScale = 1f;
          ActualizarCorazon();
          ActualizarTextoVidas();
}

    void ActualizarTextoVidas()
    {
        if (cantVidas != null)
        {
            cantVidas.text = "X " + GameData.vidasJugador;
        }
    }

    void ActualizarCorazon()
    {
        corazon.sprite = GameData.vidasJugador > 0 ? corazonLleno : corazonVacio;
    }

    void GameOver()
    {
        if (GameData.vidasJugador >0)
        {
            // Desactivar al personaje
            if (personaje != null)
            {
                personaje.SetActive(false); // Desactivar el objeto del jugador en lugar de destruirlo
            }

            // Mostrar el panel de Game Over
            GameObject hud = GameObject.Find("--- HUD ---");
            GameObject panelMuerte = hud.transform.Find("Muerte").gameObject;
            panelMuerte.SetActive(true);
            Time.timeScale = 0f;

            // Configurar los botones del menú de Game Over
            Button reiniciar = panelMuerte.transform.Find("volver").GetComponent<Button>();
            Button salir = panelMuerte.transform.Find("exit").GetComponent<Button>();

            reiniciar.onClick.AddListener(ReiniciarEscena);
            salir.onClick.AddListener(Salirjeugo);
            corazon.gameObject.SetActive(false);
        }
    }

    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void PausarJuego()
    {
        botonPausa.SetActive(false);
        botonContinuar.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReanudarJuego()
    {
        botonPausa.SetActive(true);
        botonContinuar.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReiniciarEscena()
    {
        Time.timeScale = 1;

        // Reiniciar la escena sin resetear las vidas
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salirjeugo()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
