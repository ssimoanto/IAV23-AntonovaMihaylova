using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PanaderoController : MonoBehaviour
{
    //public GameObject tableObjects;
    private NavMeshAgent navMeshAgent;
    private Animation anim;
    Vector3 destino;
    Vector3 place = new Vector3(-4, 0.03f, 14.5f);
    Vector3 stop = new Vector3(0, 0, 0);
    bool angry = false;
    string currentPopup;
    string lastPopup;


    public GameObject nevera;
    public GameObject mesa;
    public GameObject horno;

    public GameObject cakePrefab;
    public GameObject cookiePrefab;
    public GameObject breadPrefab;

    public GameObject trigoPopup;
    public GameObject huevoPopup;

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
    }

    void Update()
    {
        Animations();
        currentPopup = GetCurrentIngredient();
        if (currentPopup != lastPopup)
        {
            if (currentPopup == "Trigo")
            {
                trigoPopup.SetActive(true);
                huevoPopup.SetActive(false);
            }
            else if (currentPopup == "Huevo")
            {
                trigoPopup.SetActive(false);
                huevoPopup.SetActive(true);
            }
            else
            {
                trigoPopup.SetActive(false);
                huevoPopup.SetActive(false);
            }
        }
    }
    public void changeAngry()
    {
        angry = !angry;
    }
    private void Animations()
    {
        if (angry)
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

    public void AddProducts(List<string> productos)
    {
        for (int i = 0; i < productos.Count; i++)
        {
            switch (productos[i])
            {
                case "Cake":
                    List<int> cakeIngredients = new List<int>() { 2, 2 };
                    currentProducts[productos[i]] = cakeIngredients;
                    break;
                case "Cookie":
                    List<int> cookieIngredients = new List<int>() { 1, 1 };
                    currentProducts[productos[i]] = cookieIngredients;
                    break;
                case "Bread":
                    List<int> breadIngredients = new List<int>() { 3, 0 };
                    currentProducts[productos[i]] = breadIngredients;
                    break;
            }
        }
    }

    public void DeleteIngredient(string producto, string ingrediente)
    {
        int ingred = 0;
        if (ingrediente == "Trigo") ingred = 0;
        else if (ingrediente == "Huevo") ingred = 1;
        if (currentProducts[producto][ingred] > 0)
        {
            currentProducts[producto][ingred]--;
            Debug.Log("DeleteIngredient, remaining: " + currentProducts[producto][ingred] + " of " + ingrediente);
        }
    }
    public void DeleteProduct(string producto)
    {
        switch (producto)
        {
            case "Cake":
                Instantiate(cakePrefab);
                break;
            case "Cookie":
                Instantiate(cookiePrefab);
                break;
            case "Bread":
                Instantiate(breadPrefab);
                break;
        }
        currentProducts.Remove(producto);
        Debug.Log("Delete product, remaining: " + GetCurrentProduct());
    }

    public string GetCurrentProduct()
    {
        if (currentProducts.Count == 0)
        {
            Debug.Log("El diccionario de productos está vacío");
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
            return "Trigo";
        }
        else if (currentProducts[currentProducts.Keys.First()][1] > 0)
        {
            return "Huevo";
        }
        else return "0";
    }

    public bool HasAllProducts()
    {
        return (currentProducts.Count <= 0);
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
            if (!angry)
                navMeshAgent.SetDestination(RandomDir(distanciaDeMerodeo));
        }
    }

    // Genera una posicion aleatoria a cierta distancia dentro de las areas permitidas
    private Vector3 RandomDir(float distance)
    {
        Vector3 dir = UnityEngine.Random.insideUnitSphere * distance;
        dir += transform.position;
        NavMeshHit hit;

        dir = UnityEngine.Random.insideUnitSphere * distance;
        dir += transform.position;
        NavMesh.SamplePosition(dir, out hit, distance, NavMesh.AllAreas);


        return hit.position;
    }
}
