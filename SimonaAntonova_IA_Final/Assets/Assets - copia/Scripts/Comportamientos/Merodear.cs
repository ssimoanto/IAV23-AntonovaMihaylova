/*    
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores: Grupo 15:
   Simona Antonova, Adrián Montero y Alejandro Segarra
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : ComportamientoAgente
    {
        public override Direccion GetDireccion()
        {

            Direccion direccion = new Direccion();

            //cambiar la orientacion de manera aleatoria
            direccion.lineal = agente.velocidadMax * randomBin();
            direccion.lineal.Normalize();

            return direccion;
        }

        //direccion random
        private Vector3 randomBin()
        {
            return new Vector3(Random.Range(-2.0f, 2.0f), 0, Random.Range(-2.0f, 2.0f));
        }
    }
}