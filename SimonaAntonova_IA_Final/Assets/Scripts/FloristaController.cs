using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FloristaController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animation anim;
    Vector3 destino;
    Vector3 place = new Vector3(-4, 0.03f, 14.5f);
    Vector3 stop = new Vector3(0, 0, 0);
    bool angry = false;
    string currentPopup;
    string lastPopup;


    public GameObject pozo;
    public GameObject mesa;

    public GameObject bouquet1Prefab;
    public GameObject bouquet2Prefab;
    public GameObject bouquet3Prefab;

    public GameObject flower1Popup;
    public GameObject flower2Popup;
    public GameObject flower3Popup;

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
            if (currentPopup == "Flower1")
            {
                flower1Popup.SetActive(true);
                flower2Popup.SetActive(false);
                flower3Popup.SetActive(false);
            }
            else if (currentPopup == "Flower2")
            {
                flower1Popup.SetActive(false);
                flower3Popup.SetActive(false);
                flower2Popup.SetActive(true);
            }
            else if (currentPopup == "Flower3")
            {
                flower1Popup.SetActive(false);
                flower3Popup.SetActive(true);
                flower2Popup.SetActive(false);
            }
            else
            {
                flower1Popup.SetActive(false);
                flower2Popup.SetActive(false);
                flower3Popup.SetActive(false);
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
                case "Bouquet1":
                    List<int> bouquet1Ingredients = new List<int>() { 2, 1, 0 };
                    currentProducts[productos[i]] = bouquet1Ingredients;
                    break;
                case "Bouquet2":
                    List<int> bouquet2Ingredients = new List<int>() { 1, 1, 1 };
                    currentProducts[productos[i]] = bouquet2Ingredients;
                    break;
                case "Bouquet3":
                    List<int> bouquet3Ingredients = new List<int>() { 0, 1, 1 };
                    currentProducts[productos[i]] = bouquet3Ingredients;
                    break;
            }
        }
    }

    public void DeleteIngredient(string producto, string ingrediente)
    {
        int ingred = 0;
        if (ingrediente == "Flower1") ingred = 0;
        else if (ingrediente == "Flower2") ingred = 1;
        else if (ingrediente == "Flower3") ingred = 2;
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
            case "Bouquet1":
                Instantiate(bouquet1Prefab);
                break;
            case "Bouquet2":
                Instantiate(bouquet2Prefab);
                break;
            case "Bouquet3":
                Instantiate(bouquet3Prefab);
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
            Debug.Log("num trigo: " + currentProducts[currentProducts.Keys.First()][0]);
            return "Flower1";
        }
        else if (currentProducts[currentProducts.Keys.First()][1] > 0)
        {
            Debug.Log("num webo: " + currentProducts[currentProducts.Keys.First()][1]);
            return "Flower2";
        }
        else if (currentProducts[currentProducts.Keys.First()][2] > 0)
        {
            Debug.Log("num webo: " + currentProducts[currentProducts.Keys.First()][2]);
            return "Flower3";
        }
        else return "0";
    }

    public bool HasAllProducts()
    {
        return (currentProducts.Count <= 0);
    }
    public bool HasAllIngredients(string producto)
    {
        return (currentProducts[producto][0] == 0 && currentProducts[producto][1] == 0 && currentProducts[producto][2] == 0);
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
