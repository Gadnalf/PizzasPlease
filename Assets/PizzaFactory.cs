using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaFactory : MonoBehaviour
{
    public GameObject crust;

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

    // SUMMON THE PIZZA
    public GameObject CreatePizza(PizzaOrder pizzaOrder)
    {
        GameObject pizza = Instantiate(crust);
        return pizza;
    }
}
