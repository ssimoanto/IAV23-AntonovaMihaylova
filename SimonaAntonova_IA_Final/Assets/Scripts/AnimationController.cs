using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    public GameObject tableObjects;
    private NavMeshAgent navMeshAgent;
    private Animation anim;
    Vector3 destino;
    Vector3 stop = new Vector3(0, 0, 0);
    int speed = 0;
    public bool ask = false;
    Transform[] allChildren;


    // Segundos que puede estar merodeando
    public float tiempoDeMerodeo = 5;
    // Segundo en el que comezo a merodear
    public float tiempoComienzoMerodeo = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;

    enum Cake
    {
        
    }
    enum Cookie
    {

    }
    enum Bread
    {

    }
    void Start()
    {
        anim = GetComponent<Animation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        allChildren = tableObjects.GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        if (ask)
        {
            anim.Play("Dizzy");
        }
        else if (navMeshAgent.velocity != stop)
        {
            anim.Play("Run");
        }
        else
            anim.Play("Idle");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Avatar")
        {
            if (other.gameObject.GetComponent<PlayerMovement>().currentObject == "Trigo")
            {
                foreach (Transform child in allChildren)
                {
                    if (child.gameObject.tag == "Trigo")
                    {
                        child.gameObject.SetActive(true);
                        break;
                    }
                }
            }
            else if (other.gameObject.GetComponent<PlayerMovement>().currentObject == "Egg")
            {
                foreach (Transform child in allChildren)
                {
                    if (child.gameObject.tag == "Egg")
                    {
                        child.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    public bool HasAllIngredients()
    {
        return true;
    }

    // Genera un nuevo punto de merodeo cada vez que agota su tiempo de merodeo actual
    public void Merodeo()
    {
        //Vector3 d = transform.position - agente.destination;
        //if (d.magnitude <= agente.stoppingDistance)
        //{
        tiempoComienzoMerodeo += Time.deltaTime;
        if (tiempoComienzoMerodeo >= tiempoDeMerodeo)
        {
            tiempoComienzoMerodeo = 0;
            navMeshAgent.SetDestination(RandomDir(distanciaDeMerodeo));
        }
        //}
    }
    // Genera una posicion aleatoria a cierta distancia dentro de las areas permitidas
    private Vector3 RandomDir(float distance)
    {
        Vector3 dir = UnityEngine.Random.insideUnitSphere * distance;
        dir += transform.position;
        NavMeshHit hit;
        do
        {
            dir = UnityEngine.Random.insideUnitSphere * distance;
            dir += transform.position;
            NavMesh.SamplePosition(dir, out hit, distance, NavMesh.AllAreas);
        }
        while ((1 << NavMesh.GetAreaFromName("Escenario") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Palco Este") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Palco Oeste") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Butacas") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Vestíbulo") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Bambalinas") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Sótano Este") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Sótano Oeste") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Celda") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Sótano Norte") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Música") & hit.mask) == 0 &&
            (1 << NavMesh.GetAreaFromName("Pasillos Escenario") & hit.mask) == 0);

        return hit.position;
    }
}
