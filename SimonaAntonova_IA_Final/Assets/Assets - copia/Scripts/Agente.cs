/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine; 

/// <summary>
/// La clase Agente es responsable de modelar los agentes y gestionar todos los comportamientos asociados para combinarlos (si es posible) 
/// </summary>
    public class Agente : MonoBehaviour {
        /// <summary>
        /// Combinar por peso
        /// </summary>
        [Tooltip("Combinar por peso")]
        public bool combinarPorPeso = false;

        /// <summary>
        /// Combinar por prioridad
        /// </summary>
        [Tooltip("Combinar por prioridad")]
        public bool combinarPorPrioridad = false;

        /// <summary>
        /// Umbral de prioridad para tener el valor en cuenta
        /// </summary>
        [Tooltip("Umbral de prioridad")]
        public float umbralPrioridad = 0.2f;

        /// <summary>
        /// Velocidad máxima
        /// </summary>
        [Tooltip("Velocidad (lineal) máxima")]
        public float velocidadMax;

        /// <summary>
        /// Rotación máxima
        /// </summary>
        [Tooltip("Rotación (velocidad angular) máxima")]
        public float rotacionMax;

        /// <summary>
        /// Aceleración máxima
        /// </summary>
        [Tooltip("Aceleración (lineal) máxima")]
        public float aceleracionMax;

        /// <summary>
        /// Aceleración angular máxima
        /// </summary>
        [Tooltip("Aceleración angular máxima")]
        public float aceleracionAngularMax;

        /// <summary>
        /// Velocidad (se puede dar una velocidad de inicio).
        /// </summary>
        [Tooltip("Velocidad")]
        public Vector3 velocidad;

        /// <summary>
        /// Rotación (o velocidad angular; se puede dar una rotación de inicio)
        /// </summary>
        [Tooltip("Rotación (velocidad angular)")]
        public float rotacion;

        /// <summary>
        /// Orientacion (hacia donde encara el agente)
        /// </summary>
        [Tooltip("Orientación")]
        public float orientacion;

        /// <summary>
        /// Valor de dirección (instrucciones de movimiento)
        /// </summary>
        [Tooltip("Dirección (instrucciones de movimiento)")]
        public Direccion direccion;

        /// <summary>
        /// Grupos de direcciones, organizados según su prioridad
        /// </summary>
        [Tooltip("Grupos de direcciones")]
        private Dictionary<int, List<Direccion>> grupos;

        /// <summary>
        /// Componente de cuerpo rígido (si la tiene el agente)
        /// </summary>
        [Tooltip("Cuerpo rígido")]
        private Rigidbody cuerpoRigido;

        /// <summary>
        /// Constante del tiempo de giro
        /// </summary>
        [Tooltip("Tiempo de giro (al cambiar de direccion)")]
        private float tiempoGiroSuave = 0.1f;

        /// <summary>
        /// Variable de referencia para damping
        /// </summary>
        [Tooltip("Referencia para el giro")]
        float velocidadGiroSuave;

        /// <summary>
        /// Al comienzo, se inicialian algunas variables
        /// </summary>
        void Start()
        {
            // Descomentar estas líneas si queremos ignorar los valores iniciales de velocidad y rotación
            //velocidad = Vector3.zero; 
            //rotacion = 0.0f
            direccion = new Direccion();
            grupos = new Dictionary<int, List<Direccion>>();

            cuerpoRigido = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// En cada tick fijo, si hay movimiento dinámico, uso el simulador físico aplicando las fuerzas que corresponda para moverlo.
        /// Un cuerpo rígido se puede mover con movePosition, cambiando la velocity o aplicando fuerzas, que es lo habitual y que permite respetar otras fuerzas que estén actuando sobre él a la vez.
        /// </summary>
        public virtual void FixedUpdate()
        {
            if (cuerpoRigido.isKinematic)
                return; // El movimiento será cinemático, fotograma a fotograma con Update

            // Limitamos la aceleración al máximo que acepta este agente (aunque normalmente vendrá ya limitada)
            if (direccion.lineal.sqrMagnitude > aceleracionMax)
                direccion.lineal = direccion.lineal.normalized * aceleracionMax; 

            // La opción por defecto sería usar ForceMode.Force, pero eso implicaría que el comportamiento de dirección tuviese en cuenta la masa a la hora de calcular la aceleración que se pide
            cuerpoRigido.AddForce(direccion.lineal, ForceMode.Acceleration);

            // Limitamos la aceleración angular al máximo que acepta este agente (aunque normalmente vendrá ya limitada)
            if (direccion.angular > aceleracionAngularMax)
                direccion.angular = aceleracionAngularMax;

            // Rotamos el objeto siempre sobre su eje Y (hacia arriba), asumiendo que el agente está sobre un plano y quiere mirar a un lado o a otro
            // La opción por defecto sería usar ForceMode.Force, pero eso implicaría que el comportamiento de dirección tuviese en cuenta la masa a la hora de calcular la aceleración que se pide
            cuerpoRigido.AddTorque(transform.up * direccion.angular, ForceMode.Acceleration);

            LookDirection();

            // Aunque también se controlen los máximos en el LateUpdate, entiendo que conviene también hacerlo aquí, en FixedUpdate, que puede llegar a ejecutarse más veces

            // Limito la velocidad lineal al terminar 
            if (cuerpoRigido.velocity.magnitude > velocidadMax)
                cuerpoRigido.velocity = cuerpoRigido.velocity.normalized * velocidadMax;

            // Limito la velocidad angular al terminar
            if (cuerpoRigido.angularVelocity.magnitude > rotacionMax)
                cuerpoRigido.angularVelocity = cuerpoRigido.angularVelocity.normalized * rotacionMax;
            if (cuerpoRigido.angularVelocity.magnitude < -rotacionMax)
                cuerpoRigido.angularVelocity = cuerpoRigido.angularVelocity.normalized * -rotacionMax;
        }

        /// <summary>
        /// En cada tick, hace lo básico del movimiento cinemático del agente
        /// Un objeto que no atiende a físicas se mueve a base de trasladar su transformada.
        /// Al no haber Freeze Rotation, ni rozamiento ni nada... seguramente vaya todo mucho más rápido en cinemático que en dinámico
        /// </summary>
        public virtual void Update()
        {
            if (!cuerpoRigido.isKinematic)
                return; // El movimiento será dinámico, controlado por la física y FixedUpdate

            // Limito la velocidad lineal antes de empezar
            if (velocidad.magnitude > velocidadMax)
                velocidad= velocidad.normalized * velocidadMax;

            // Limito la velocidad angular antes de empezar
            if (rotacion > rotacionMax)
                rotacion = rotacionMax;
            if (rotacion < -rotacionMax)
                rotacion = -rotacionMax;

            Vector3 desplazamiento = velocidad * Time.deltaTime;
            transform.Translate(desplazamiento, Space.World);

            orientacion += rotacion * Time.deltaTime;
            // Vamos a mantener la orientación siempre en el rango canónico de 0 a 360 grados
            if (orientacion < 0.0f)
                orientacion += 360.0f;
            else if (orientacion > 360.0f)
                orientacion -= 360.0f;

            LookDirection();

            // Elimino la rotación totalmente, dejándolo en el estado inicial, antes de rotar el objeto lo que nos marque la variable orientación
            transform.rotation = new Quaternion();
            transform.Rotate(Vector3.up, orientacion);

        }

        /// <summary>
        /// En cada parte tardía del tick, hace tareas de corrección numérica 
        /// </summary>
        public virtual void LateUpdate()
        {
            if (combinarPorPrioridad)
            {
                direccion = GetPrioridadDireccion();
                grupos.Clear();
            }

            if (cuerpoRigido != null) {
                return; // El movimiento será dinámico, controlado por la física y FixedUpdate
            }

            // Limitamos la aceleración al máximo que acepta este agente (aunque normalmente vendrá ya limitada)
            if (direccion.lineal.sqrMagnitude > aceleracionMax)
                direccion.lineal = direccion.lineal.normalized * aceleracionMax;

            // Limitamos la aceleración angular al máximo que acepta este agente (aunque normalmente vendrá ya limitada)
            if (direccion.angular > aceleracionAngularMax)
                direccion.angular = aceleracionAngularMax;

            // Aquí se calcula la próxima velocidad y rotación en función de las aceleraciones  
            velocidad += direccion.lineal * Time.deltaTime;
            rotacion += direccion.angular * Time.deltaTime;

            // Opcional: Esto es para actuar con contundencia si nos mandan parar (no es muy realista)
            if (direccion.angular == 0.0f) 
                rotacion = 0.0f; 
            if (direccion.lineal.sqrMagnitude == 0.0f) 
                velocidad = Vector3.zero; 

            /// En cada parte tardía del tick, encarar el agente (al menos para el avatar).... si es que queremos hacer este encaramiento
            transform.LookAt(transform.position + velocidad);

            // Se deja la dirección vacía para el próximo fotograma
            direccion = new Direccion();
        }


        /// <summary>
        /// Establece la dirección tal cual
        /// </summary>
        public void SetDireccion(Direccion direccion)
        {
            this.direccion = direccion;
        }

        /// <summary>
        /// Establece la dirección por peso
        /// </summary>
        /// <param name="direccion"></param>
        /// <param name="peso"></param>
        public void SetDireccion(Direccion direccion, float peso)
        {
            this.direccion.lineal += (peso * direccion.lineal);
            this.direccion.angular += (peso * direccion.angular);
        }

        /// <summary>
        /// Establece la dirección por prioridad
        /// </summary>
        /// <param name="direccion"></param>
        /// <param name="prioridad"></param>
        public void SetDireccion(Direccion direccion, int prioridad)
        {
            if (!grupos.ContainsKey(prioridad))
            {
                grupos.Add(prioridad, new List<Direccion>());
            }
            grupos[prioridad].Add(direccion);
        }

        /// <summary>
        /// Devuelve el valor de dirección calculado por prioridad
        /// </summary>
        /// <returns></returns>
        private Direccion GetPrioridadDireccion()
        {
            Direccion direccion = new Direccion();
            List<int> gIdList = new List<int>(grupos.Keys);
            gIdList.Sort();
            foreach (int gid in gIdList)
            {
                direccion = new Direccion();
                foreach (Direccion direccionIndividual in grupos[gid])
                {
                    // Dentro del grupo la mezcla es por peso
                    direccion.lineal += direccionIndividual.lineal;
                    direccion.angular += direccionIndividual.angular;
                }
                // Sólo si el resultado supera un umbral, entonces nos quedamos con esta salida y dejamos de mirar grupos con menos prioridad
                if (direccion.lineal.magnitude > umbralPrioridad
                     || Mathf.Abs(direccion.angular) > umbralPrioridad)
                {
                    return direccion;
                }
            }
            return direccion;
        }

        /// <summary>
        /// Calculates el Vector3 dado un cierto valor de orientación
        /// </summary>
        /// <param name="orientacion"></param>
        /// <returns></returns>
        public Vector3 OriToVec(float orientacion)
        {
            Vector3 vector = Vector3.zero;
            vector.x = Mathf.Sin(orientacion * Mathf.Deg2Rad) * 1.0f; //  * 1.0f se añade para asegurar que el tipo es float
            vector.z = Mathf.Cos(orientacion * Mathf.Deg2Rad) * 1.0f; //  * 1.0f se añade para asegurar que el tipo es float
            return vector.normalized;
        }

        private void LookDirection()
        {
            if (direccion.lineal.x != 0 || direccion.lineal.z != 0)
            {
                //Rotación del personaje hacia donde camina (suavizado)
                float anguloDestino = Mathf.Atan2(direccion.lineal.x, direccion.lineal.z) * Mathf.Rad2Deg;
                //Esto es raro pero Brackeys dice que funciona
                float anguloSuave = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloDestino, ref velocidadGiroSuave, tiempoGiroSuave);

                transform.rotation = Quaternion.Euler(0f, anguloSuave, 0f);
            }
        }
    }
}
