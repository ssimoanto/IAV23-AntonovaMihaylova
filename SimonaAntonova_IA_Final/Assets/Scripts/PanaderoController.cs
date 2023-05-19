using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UCM.IAV.Movimiento;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PanaderoController : MonoBehaviour
{
    //public GameObject tableObjects;
    private NavMeshAgent navMeshAgent;
    private Animation anim;
    Vector3 destino;
    Vector3 stop = new Vector3(0, 0, 0);
    public bool ask = false;
    Transform[] allChildren;


    // Segundos que puede estar merodeando
    public float tiempoDeMerodeo = 5;
    // Segundo en el que comezo a merodear
    public float tiempoComienzoMerodeo = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    // MAp de producto, ingredientes
    Dictionary<string, List<int>> currentProducts = new Dictionary<string, List<int>>();

    void Start()
    {
        anim = GetComponent<Animation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        //allChildren = tableObjects.GetComponentsInChildren<Transform>();
        string[] productos = { "Cake", "Cookie" };
        AddProducts(productos);
    }

    void Update()
    {
        Animations();
    }

    private void Animations()
    {
        if (navMeshAgent.velocity != stop)
        {
            anim.Play("Run");
        }
        else
            anim.Play("Idle");
    }

    public void AddProducts(string[] productos)
    {
        Debug.Log("prooo" + productos[0]);
        for (int i = 0; i < productos.Length; i++)
        {
            Debug.Log("prooo" + productos[i]);

            switch (productos[i])
            {
                case "Cake":
                    Debug.Log("Tarta");
                    List<int> cakeIngredients = new List<int>() { 2, 2 };
                    currentProducts[productos[i]] = cakeIngredients;
                    break;
                case "Cookie":
                    List<int> cookieIngredients = new List<int>() { 2, 2 };
                    currentProducts[productos[i]] = cookieIngredients;
                    break;
                case "Bread":
                    List<int> breadIngredients = new List<int>() { 2, 2 };
                    currentProducts[productos[i]] = breadIngredients;
                    break;
            }
        }
    }

    public void DeleteIngredient(string producto, string ingrediente)
    {
        int ingred = 0;
        if(ingrediente == "Trigo") ingred = 0;
        else if(ingrediente == "Huevo") ingred = 1;
        if (currentProducts[producto][ingred] > 0)
        {
            currentProducts[producto][ingred]--;

            Debug.Log("DeleteIngredient, remaining: "+currentProducts[producto][ingred] + " of " + ingrediente);
        }
    }
    public void DeleteProduct(string producto)
    {
        if (currentProducts[producto][0] <= 0 && currentProducts[producto][1] <= 0)
        {
            currentProducts.Remove(producto);
        }
    }

    public string GetCurrentProduct()
    {
        if (currentProducts.Count == 0)
        {
            Debug.Log("El diccionario de productos est� vac�o");
            return "";
        }
        else
        {
            return currentProducts.Keys.First();
        }
    }

    public string GetCurrentIngredient()
    {
        if (currentProducts[currentProducts.Keys.First()][0] > 0)
        {
            Debug.Log("num trigo: "+ currentProducts[currentProducts.Keys.First()][0]);
            return "Trigo";
        }
        else if (currentProducts[currentProducts.Keys.First()][1] > 0)
        {
            Debug.Log("num webo: " + currentProducts[currentProducts.Keys.First()][1]);
            return "Huevo";
        }
        else return "0";
    }

    public bool HasAllProducts()
    {
        return (currentProducts.Count == 0);
    }
    public bool HasAllIngredients(string producto)
    {
        return (currentProducts[producto][0] == 0 && currentProducts[producto][1] == 0);
    }

    // Genera un nuevo punto de merodeo cada vez que agota su tiempo de merodeo actual
    public void Merodeo()
    {
        tiempoComienzoMerodeo += Time.deltaTime;
        if (tiempoComienzoMerodeo >= tiempoDeMerodeo)
        {
            tiempoComienzoMerodeo = 0;
            navMeshAgent.SetDestination(RandomDir(distanciaDeMerodeo));
        }
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
        while (!(NavMesh.SamplePosition(dir, out hit, 0.1f, NavMesh.GetAreaFromName("Panaderia"))));

        return hit.position;
    }


}
