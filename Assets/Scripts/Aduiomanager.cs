using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Aduiomanager : MonoBehaviour
{
    public static Aduiomanager Instance{get;private set;}

    private AudioSource audioSource;

    private GameManager gameManager;
    public AudioClip audioClip;  // Añadido: variable para el clip de audio a reproducir

    private void Awake() {
        if(Instance==null){
            Instance=this;
        }else{
            Debug.Log("Mas de ina sintancia de Audiomanager");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
   public void ReproducirSonido(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }
}
