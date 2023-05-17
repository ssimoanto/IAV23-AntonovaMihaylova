/*    
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores: Grupo 15:
   Simona Antonova, Adrián Montero y Alejandro Segarra
*/
using UnityEngine.Analytics;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            //se coge la direccion opuesta del objetivo
            direccion.lineal=transform.position-objetivo.transform.position;
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;
            direccion.angular = 0;
            return direccion;
        }
    }
}
