using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedaCiontroller : MonoBehaviour
{
  public int valor=1;
  public GameManager gameManager;
 private void OnTriggerEnter2D(Collider2D other) {
  if(other.CompareTag("Player"))  {
    gameManager.SumarMonedas(valor);
  
    Destroy(gameObject);
  }
 }
}
