/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Clase auxiliar para representar la dirección (instrucciones) con la que corregir el movimiento dinámicamente, mediante aceleraciones.
    /// </summary>
    public class Direccion
    {
        public float angular;
        public Vector3 lineal;
        public Direccion()
        {
            angular = 0.0f;
            lineal = new Vector3();
        }
    }
}