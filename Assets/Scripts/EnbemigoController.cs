using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    public float distanciaMovimiento = 5f;
    public float velocidad = 2f;
    public float tiempoQuieto = 3f;
    public float knockbackduration = 1f;
    private Vector2 posicionInicial;
    private bool moviendoIzquierda;
    private float tiempoEsperaActual;
    private Animator animator;
    public GameManager gameManager;
    public GameObject prefabBala;
    public float distanciaDisparo = 1f;
    private float tiempoDisparoActual;
    private float tiemposDisparoMin = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        posicionInicial = transform.position;
        tiempoEsperaActual = tiempoQuieto;
        moviendoIzquierda = true;
        tiempoDisparoActual = Random.Range(tiemposDisparoMin, tiempoQuieto);

        // Asegúrate de que gameManager está asignado para evitar errores de referencia
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
                Debug.LogWarning("GameManager no asignado en EnemigoController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoEsperaActual > 0)
        {
            tiempoEsperaActual -= Time.deltaTime;
            return;
        }

        // Determinar el destino de movimiento
        Vector2 destino = moviendoIzquierda ? posicionInicial + Vector2.left * distanciaMovimiento 
                                            : posicionInicial + Vector2.right * distanciaMovimiento;

        transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

        // Cambiar dirección si el enemigo alcanza su destino
        if (Vector2.Distance(transform.position, destino) < 0.1f)
        {
            moviendoIzquierda = !moviendoIzquierda;
            tiempoEsperaActual = tiempoQuieto;
        }

        gestionarGiro();

        // Disparo de bala
        if (tiempoDisparoActual > 0)
        {
            tiempoDisparoActual -= Time.deltaTime;
            if (tiempoDisparoActual <= 0)
            {
                animator.SetBool("disparando", true);
                StartCoroutine(ResetDisparando());
                DisparandoBala();
                tiempoDisparoActual = Random.Range(tiemposDisparoMin, tiempoQuieto);
            }
        }
    }

    void gestionarGiro()
    {
        if (tiempoEsperaActual <= 0)
        {
            animator.SetBool("isRuning", true);
            transform.localScale = moviendoIzquierda ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
        else
        {
            animator.SetBool("isRuning", false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.DesactivarMovimiento(knockbackduration);
            }

            // Pausar el juego y reducir vida al jugador
            Time.timeScale = 0f;
            if (gameManager != null)
            {
                gameManager.QuitarVidaJugador();
            }
            else
            {
                Debug.LogWarning("GameManager no asignado en OnCollisionEnter2D.");
            }
        }
    }

    void DisparandoBala()
    {
        Vector2 direccionDisparo = moviendoIzquierda ? Vector2.left : Vector2.right;
        Vector2 posicionBala = (Vector2)transform.position + direccionDisparo * distanciaDisparo;

        if (prefabBala != null)
        {
            GameObject bala = Instantiate(prefabBala, posicionBala, Quaternion.identity);
            Balacontroller balaController = bala.GetComponent<Balacontroller>();
            if (balaController != null)
            {
                balaController.EstablecerDireccion(direccionDisparo);
            }
            else
            {
                Debug.LogWarning("El prefabBala no tiene el componente Balacontroller.");
            }
        }
        else
        {
            Debug.LogWarning("PrefabBala no asignado en el inspector.");
        }
    }

    private IEnumerator ResetDisparando()
    {
        yield return null;
        animator.SetBool("disparando", false);
    }
}