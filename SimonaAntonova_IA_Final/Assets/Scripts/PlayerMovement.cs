using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    Vector3 destino;
    Vector3 stop = new Vector3(0, 0, 0);
    RaycastHit hit;

    void Start()
    {
        animator = GetComponent<Animator>();
        //rigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>(); // Punto al que queremos que se mueva el jugador
        destino = transform.position;

    }

    void Update()
    {
        if (navMeshAgent.velocity != stop)
        {
            animator.SetInteger("speed", 4);
        }
        else
            animator.SetInteger("speed", 0);
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                destino = hit.point;
            }
        }
        navMeshAgent.destination = destino;
    }
}