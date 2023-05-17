/*    
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores: Grupo 15:
   Simona Antonova, Adrián Montero y Alejandro Segarra
*/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para detectar el numero de ratas cercanas al perro
    /// para que este huya adoptando el comportamiento de Huir.cs
    /// </summary>
    public class Deteccion : MonoBehaviour
    {
        public float radio = 3f;

        private List<GameObject> ratas = new List<GameObject>();


        private SphereCollider trigger;

        bool estaHuyendo = false;

        private AudioSource audio;

        void Start()
        {
            audio = transform.gameObject.GetComponent<AudioSource>();
            trigger = transform.gameObject.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.enabled = true;
            trigger.radius = radio;
        }

        void Update()
        {
            Huir huir = GetComponent<Huir>();
            Llegada llegada = GetComponent<Llegada>();

            //si hay mas de dos ratas cerca
            if (ratas.Count >= 2 && !estaHuyendo)
            {
                //ladra
                audio.Play();
                //deja de seguir al jugador
                llegada.enabled = false;
                //huye de la primera rata de la lista
                huir.enabled = true;
                huir.objetivo = ratas[0];
                estaHuyendo = true;
            }
            if (ratas.Count == 0 && estaHuyendo)
            {
                //vuelve a seguir al jugador
                llegada.enabled = true;
                //deja de huir
                huir.enabled = false;
                huir.objetivo = null;
                estaHuyendo = false;
            }
        }

        private void OnTriggerEnter(Collider ratColl)
        {

            Merodear ratComp = ratColl.gameObject.GetComponent<Merodear>();
            if (ratComp != null && !ratas.Contains(ratColl.gameObject))
            {
                ratas.Add(ratColl.gameObject);
            }
        }

        private void OnTriggerExit(Collider ratColl)
        {
            // Si las ratas salen del trigger, se reactivan sus comportamientos por defecto
            Merodear ratComp = ratColl.gameObject.GetComponent<Merodear>();
            if (ratComp != null && ratas.Contains(ratColl.gameObject))
            {
                ratas.Remove(ratColl.gameObject);
            }
        }

    }
}