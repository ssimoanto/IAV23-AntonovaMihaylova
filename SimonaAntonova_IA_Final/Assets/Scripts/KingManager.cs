using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KingManager : MonoBehaviour
{

    // Segundos que puede estar merodeando
    public float tiempoDeMerodeo = 5;
    // Segundo en el que comezo a merodear
    public float tiempoComienzoMerodeo = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    NavMeshAgent navMeshAgent;

    // Sprites productos pedido
    public GameObject imageCake;
    public GameObject imageCookie;
    public GameObject imageBread;
    public GameObject imageBouquet1;
    public GameObject imageBouquet2;
    public GameObject imageBouquet3;


    public GameObject panadero;
    public GameObject florista;

    string[] productos = { "", "" };
    List<string> names1 = new List<string>();
    List<string> names2 = new List<string>();

    private void Awake()
    {
        names1.Add("Cake");
        names1.Add("Cookie");
        //names1.Add("Bread");
        //names2.Add("Bouquet1");
        //names2.Add("Bouquet2");
        //names2.Add("Bouquet3");
        int num = Random.Range(0, 2);
        names1.RemoveAt(num);
        num = Random.Range(0, 2);
        //names2.RemoveAt(num);
        panadero.GetComponent<PanaderoController>().AddProducts(names1);
        //florista.GetComponent<FloristaController>().AddProducts(names2);

        // Iconos
        for (int i = 0; i < 2; i++)
        {
            switch (names1[i])
            {
                case "Cake":
                    imageCake.GetComponent<Image>().enabled = true;
                    break;
                case "Cookie":
                    imageCookie.GetComponent<Image>().enabled = true;
                    break;
                case "Bread":
                    imageBread.GetComponent<Image>().enabled = true;
                    break;
            }
            //switch (names2[i])
            //{
            //    case "Bouquet1":
            //        imageBouquet1.GetComponent<Image>().enabled = true;
            //        break;
            //    case "Bouquet2":
            //        imageBouquet2.GetComponent<Image>().enabled = true;
            //        break;
            //    case "Bouquet3":
            //        imageBouquet3.GetComponent<Image>().enabled = true;
            //        break;
            //}
        }
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public bool CheckProducts()
    {
        return (names1.Count == 0 && names2.Count == 0);
    }
    public void setUIoff(string product)
    {

        switch (product)
        {
            case "Cake":
                imageCake.GetComponent<Image>().enabled = false;
                break;
            case "Cookie":
                imageCookie.GetComponent<Image>().enabled = false;
                break;
            case "Bread":
                imageBread.GetComponent<Image>().enabled = false;
                break;
        }
        //switch (product)
        //{
        //    case "Bouquet1":
        //        imageBouquet1.GetComponent<Image>().enabled = false;
        //        break;
        //    case "Bouquet2":
        //        imageBouquet2.GetComponent<Image>().enabled = false;
        //        break;
        //    case "Bouquet3":
        //        imageBouquet3.GetComponent<Image>().enabled = false;
        //        break;
        //}
    }
    public bool IsProductValid(string producto)
    {
        foreach (var item in names1)
        {
            if (producto == item)
            {
                return true;
            }
        }
        foreach (var item in names2)
        {
            if (producto == item)
            {
                return true;
            }
        }
        return false;

    }

    public string DeleteProduct(string producto)
    {
        foreach (var item in names1)
        {
            if (producto == item)
            {
                names1.Remove(item);
                return "Panadero";
            }
        }
        foreach (var item in names2)
        {
            if (producto == item)
            {

                names2.Remove(item);
                return "Florista";
            }
        }
        return ("");
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

        dir = UnityEngine.Random.insideUnitSphere * distance;
        dir += transform.position;
        NavMesh.SamplePosition(dir, out hit, distance, NavMesh.AllAreas);


        return hit.position;
    }
}
