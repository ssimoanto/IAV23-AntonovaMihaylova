/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimadorAnimal: MonoBehaviour
{
    Animator animator;

    [SerializeField]
    Rigidbody rigid;

    [SerializeField]
    float threshold = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigid.velocity.magnitude >= threshold)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);
    }
}
