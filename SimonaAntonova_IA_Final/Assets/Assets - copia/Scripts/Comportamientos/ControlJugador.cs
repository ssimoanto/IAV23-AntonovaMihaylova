/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado y modificado por los 
   integrantes del grupo 15 (Simona Antonova, Adrián Montero y Simona Antonova)
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    /// <summary>
    /// El comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>

        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();

            if (!Input.GetMouseButton(0)) return direccion;

            // Punto al que queremos que se mueva el jugador
            Vector3 target = Vector3.zero;

            // Raycast del punto seleccionado en el mapa
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                target = hit.point;

            // Direccion del movimiento
            direccion.lineal.x = (target.x - transform.position.x);
            direccion.lineal.z = (target.z - transform.position.z);

            // Resto de calculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            return direccion;
        }
    }
}