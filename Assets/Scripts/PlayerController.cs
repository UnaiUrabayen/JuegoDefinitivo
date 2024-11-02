using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private new Rigidbody2D rigidbody2D;
   public LayerMask groundLayer;
   public float rayLength = 0.5f;

   public Animator animator;
   public float jumpForce = 7f;

   private bool mirandoDerecha = true;
   private bool puedeSaltar = false;
   public float velocidad = 5f;

   private BoxCollider2D boxCollider;
   public LayerMask suelo;

   private bool puedeMoverse = true;

   void Start()
   {
      Physics2D.gravity = new Vector2(0, -15f);
      rigidbody2D = GetComponent<Rigidbody2D>();
      animator = GetComponent<Animator>();
      boxCollider = GetComponent<BoxCollider2D>();
   }

   void Update()
   {
      if (puedeMoverse)
      {
         procesarMovimieento();
      }
   }

   void procesarMovimieento()
   {
      float inputMovimiento = Input.GetAxis("Horizontal");
      rigidbody2D.velocity = new Vector2(inputMovimiento * velocidad, rigidbody2D.velocity.y);

      RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);

      if (hit.collider != null && Input.GetKeyDown(KeyCode.Space))
      {
         rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
      }

      animator.SetBool("isRunning", inputMovimiento != 0f);
      GestionarOrientacion(inputMovimiento);

      puedeSaltar = estaEnSuelo();
      animator.SetBool("estaSuelo", puedeSaltar);
      animator.SetFloat("velocidadSalto", rigidbody2D.velocity.y);
      
      procesarSalto();
   }

   void OnDrawGizmos()
   {
      Gizmos.color = Color.cyan;
      Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
   }

   void GestionarOrientacion(float inputMovimiento)
   {
      if ((mirandoDerecha && inputMovimiento < 0) || (!mirandoDerecha && inputMovimiento > 0))
      {
         mirandoDerecha = !mirandoDerecha;
         transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
      }
   }

   void procesarSalto()
   {
      if (Input.GetKeyDown(KeyCode.Space) && puedeSaltar && estaEnSuelo())
      {
         // Implementa lógica de salto si es necesario.
      }
   }

   bool estaEnSuelo()
   {
      RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);
      return raycastHit.collider != null;
   }

   public void DesactivarMovimiento(float duration)
   {
      StartCoroutine(DisableMovementCourtine(duration));
   }

   private IEnumerator DisableMovementCourtine(float duration)
   {
      puedeMoverse = false;
      yield return new WaitForSeconds(duration);
      puedeMoverse = true;
   }
}
