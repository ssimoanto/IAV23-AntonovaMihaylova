/*    
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autores: Grupo 15:
   Simona Antonova, Adri�n Montero y Alejandro Segarra
*/
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducci�n de la fuerza de repulsi�n
        [SerializeField]
        float coefReduc;

        // GameObject vacio que contiene las ratas
        public GameObject rataGO;
        // Entidades potenciales de las que huir
        private GameObject[] objetivos;


        private void Start()
        {
            //gameObject padre de las ratas
            rataGO = transform.parent.gameObject;
            //ratas
            objetivos = new GameObject[rataGO.transform.childCount];
            for (int i = 0; i < rataGO.transform.childCount; i++)
            {
                objetivos[i] = rataGO.transform.GetChild(i).gameObject;
            }
        }
        public override Direccion GetDireccion()
        {
            Direccion result = new Direccion();

            Vector3 dir;
            float distancia;
            float fuerza;
            //para cada rata
            foreach (GameObject gO in objetivos)
            {
                //comprobar si esta cerca
                dir = gO.transform.position - transform.position;
                distancia = dir.magnitude;
                if (distancia < umbral)
                {
                    //fuerza de repulsion
                    fuerza = Math.Min(coefReduc / (distancia * distancia), agente.aceleracionMax);
                    result.lineal += fuerza * -dir.normalized;
                }
            }

            return result;
        }
    }
}