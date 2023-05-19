using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 destino;
    private Vector3 stop = new Vector3(0, 0, 0);

    int maxInventario;
    private int numInventario;
    public string currentObject;
    private RaycastHit hit;

    private void Awake()
    {
        maxInventario = 1;
        numInventario = 0;
    }

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseInventario();
        }
        navMeshAgent.destination = destino;
    }

    public void ReleaseInventario()
    {
        numInventario = 0;
        currentObject = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (numInventario < maxInventario)
        {
            if (other.tag == "Trigo" || other.tag == "Flower1" || other.tag == "Flower2" || other.tag == "Flower3" || other.tag == "Huevo")
            {
                currentObject = other.tag;
                //other.gameObject.SetActive(false);
                other.transform.localScale = Vector3.zero;
                numInventario++;
            }
            //if (other.tag == "Bouquet1" || other.tag == "Bouquet2" || other.tag == "Bouquet3" || other.tag == "Cake" || other.tag == "Cookie"|| other.tag == "Bread")
            //{
            //    currentObject = other.tag;
            //    //other.gameObject.SetActive(false);
            //    other.transform.localScale = Vector3.zero;
            //    numInventario++;
            //}
        }
    }

    void TrigoInstanciate()
    {

    }
    void HuevoInstanciate()
    {

    }
    void Flower1Instanciate()
    {

    }
    void Flower2Instanciate()
    {

    }
    void Flower3Instanciate()
    {

    }
}