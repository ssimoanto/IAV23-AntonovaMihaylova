using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 destino;
    private Vector3 stop = new Vector3(0, 0, 0);
    Transform headTransform;
    int maxInventario;
    private int numInventario;
    private RaycastHit hit;

    public string currentObject;

    GameObject currentIngredient;

    public GameObject eggPrefab;
    public GameObject trigoPrefab;
    public GameObject flower1Prefab;
    public GameObject flower2Prefab;
    public GameObject flower3Prefab;

    Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();

    private void Awake()
    {
        maxInventario = 1;
        numInventario = 0;
        map.Add("Huevo", eggPrefab);
        map.Add("Trigo", trigoPrefab);
        map.Add("Flower1", flower1Prefab);
        map.Add("Flower2", flower2Prefab);
        map.Add("Flower3", flower3Prefab);
        //headTransform = transform;
        //headTransform.position = headTransform.position + new Vector3(0, 5, 0);
        //PlaceObjectHere
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
        if (numInventario > 0)
        {
            //foreach (Transform child in transform)
            //{
            //    if(child.name != "Mesh")
            //    {
            //        Destroy(child);
            //    }
            //}

            Destroy(currentIngredient);
            numInventario = 0;
            currentObject = "";
        }
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
                currentIngredient = Instantiate(map[other.tag]);
                currentIngredient.transform.SetParent(transform);
                currentIngredient.transform.SetPositionAndRotation(transform.position + new Vector3(0, 3, -0.5f), currentIngredient.transform.rotation);
            }
            else if (other.tag == "Bouquet1" || other.tag == "Bouquet2" || other.tag == "Bouquet3" || other.tag == "Cake" || other.tag == "Cookie" || other.tag == "Bread")
            {
                currentObject = other.tag;
                other.gameObject.GetComponent<Collider>().enabled = false;
                numInventario++;
                currentIngredient = other.transform.gameObject;
                currentIngredient.transform.SetParent(transform);
                currentIngredient.transform.SetPositionAndRotation(transform.position + new Vector3(0, 3, 0.5f), transform.rotation);
            }
        }
    }
}