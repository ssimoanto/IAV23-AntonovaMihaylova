/*    
   /*    
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores: Grupo 15:
   Simona Antonova, Adrián Montero y Alejandro Segarra
*/

using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {

        // El radio para llegar al objetivo
        public float rObjetivo;
        float objetivoSpeed;

        // El radio en el que se empieza a ralentizarse
        public float rRalentizado;

        Vector3 objetivoVelocity;

        // El tiempo en el que conseguir la aceleracion objetivo
        float tiempoObjetivo = 0.1f;
        

        public override Direccion GetDireccion()
        {
            Direccion resultado = new Direccion();
            
            //direccion hacia el objetivo
            Vector3 direccion = objetivo.transform.position - agente.transform.position;
            float distancia = direccion.magnitude;

            //si estamos dentro del radio
            if (distancia < rObjetivo)
            {
                return new Direccion();
            }

            if (distancia > rRalentizado)
                objetivoSpeed = agente.velocidadMax;
            else
                objetivoSpeed = agente.velocidadMax * distancia / rRalentizado;

            objetivoVelocity = direccion;
            objetivoVelocity.Normalize();
            objetivoVelocity *= objetivoSpeed;

            //llegar al objetivo
            resultado.lineal = objetivoVelocity - agente.velocidad;
            //en timeToTarget segundos
            resultado.lineal /= tiempoObjetivo;

            //si nos pasamos de la velocidad maxima
            if (resultado.lineal.magnitude > agente.aceleracionMax)
            {
                resultado.lineal.Normalize();
                resultado.lineal *= agente.aceleracionMax;
            }

            resultado.angular = 0;

            return resultado;
        }
    }
}
