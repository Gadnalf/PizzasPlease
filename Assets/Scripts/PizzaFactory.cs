using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaFactory : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject receiptPrefab;

    public struct PizzaOrder
    {
        public PizzaOrder(int diameter, int slices, List<string> leftIngredients, List<string> rightIngredients, bool wellDone)
        {
            Diameter = diameter;
            Slices = slices;
            LeftIngredients = leftIngredients;
            RightIngredients = rightIngredients;
            WellDone = wellDone;
        }

        public int Diameter { get; set; }
        public int Slices { get; set; }
        public List<string> LeftIngredients { get; set; }
        public List<string> RightIngredients { get; set; }
        public bool WellDone { get; set; }
    }

    public struct GeneratedPizza {
        public GeneratedPizza(GameObject pizza, GameObject receipt, bool good) {
            Pizza = pizza;
            Receipt = receipt;
            CurrentPizzaGood = good;
        }
        public GameObject Pizza { get; private set; }
        public GameObject Receipt { get; private set; }
        public bool CurrentPizzaGood { get; private set; }
    }

    // ORDER THE PIZZA
    public PizzaOrder GenerateNewPizzaOrder() {
        List<string> ingredients = new List<string>();
        return new PizzaOrder(1, 1, ingredients, ingredients, false);
    }

    // SUMMON THE PIZZA
    public GeneratedPizza CreatePizza(PizzaOrder pizzaOrder)
    {
        Vector3 centerPosition = new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z);
        Vector3 centerCameraPosition = Camera.main.ViewportToWorldPoint(centerPosition);
        GameObject instantiatedPizza = Instantiate(pizzaPrefab, centerCameraPosition, Quaternion.identity);

        Vector3 transform = new Vector3(5f, 0f, 0f);
        Vector3 offsetCameraPositon = centerCameraPosition + transform;
        GameObject instantiatedReceipt = Instantiate(receiptPrefab, offsetCameraPositon, Quaternion.identity);
        Color pizzaColor = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );
        instantiatedPizza.GetComponent<Renderer>().material.SetColor("_Color", pizzaColor);

        bool currentPizzaGood = Random.Range(0, 2) != 0;
        if (currentPizzaGood) {
            instantiatedReceipt.GetComponent<Renderer>().material.SetColor("_Color", pizzaColor);
        } else {
            Color receiptColor = new Color(
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f)
            );
            instantiatedReceipt.GetComponent<Renderer>().material.SetColor("_Color", receiptColor);
        }

        return new GeneratedPizza(instantiatedPizza, instantiatedReceipt, currentPizzaGood);
    }
}
