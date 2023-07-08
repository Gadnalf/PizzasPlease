using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PizzaFactory : MonoBehaviour
{
    public GameObject crust;
    // private List<string> PossibleSauce = new List<string>({"tomato sauce",  "pesto sauce"});
    // private List<string> PossibleIngredients = new List<string>({"cheese", "pepperoni", "mushrooms", "peppers", "pineapple", "anchovies", "olives"});

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



    PizzaOrder GeneratePizzaOrder(){
        string[] PossibleSauce = {"tomato sauce",  "pesto sauce"};
        string[] PossibleIngredients = {"cheese", "pepperoni", "mushrooms", "peppers", "pineapple", "anchovies", "olives"};

        Debug.Log("It's pizza time!");
        int diameter;
        int slices;
        List<string> leftIngredients = new List<string>();
        List<string> rightIngredients = new List<string>();
        bool wellDone;


        // set diameter
        int personal = 10;
        int small = 12;
        int medium = 14;
        int large = 16; 
        int[] sizes = {small, medium, large};
        diameter = sizes[Random.Range(0,2)];
        Debug.Log("diameter" + diameter);

        // set number of slices
        slices = Random.Range(1,6) * 2;

        // set left ingredients
        int sauceChoice = Random.Range(0,2);
        if(sauceChoice < 2){
            leftIngredients.Add(PossibleSauce[sauceChoice]);
        } 
        

        int numLeftIngredients = Random.Range(0,PossibleIngredients.Length-1);
        

        for(int i = 0; i < numLeftIngredients; i ++){
            string topping = PossibleIngredients[Random.Range(0,PossibleIngredients.Length-1)];
            if(!leftIngredients.Contains(topping)){
                leftIngredients.Add(topping);
            }
        }
        Debug.Log("leftIngredients" + leftIngredients);

        // set right ingredients
        sauceChoice = Random.Range(0,2);
        if(sauceChoice < 2){
            leftIngredients.Add(PossibleSauce[sauceChoice]);
        } // else no sauce

        int numRightIngredients = Random.Range(0,PossibleIngredients.Length-1);
        

        for(int i = 0; i < numRightIngredients; i ++){
            string topping = PossibleIngredients[Random.Range(0,PossibleIngredients.Length-1)];
            if(!rightIngredients.Contains(topping)){
                rightIngredients.Add(topping);
            }
        }
        Debug.Log("rightIngredients" + rightIngredients);

        // is well done
        if (Random.Range(0,1) == 0){
            wellDone = false;
        } else {
            wellDone = true;
        }
        Debug.Log("wellDone" + wellDone);
        return new PizzaOrder(diameter, slices, leftIngredients, rightIngredients, wellDone);

    }

    // SUMMON THE PIZZA
    public GameObject CreatePizza(PizzaOrder pizzaOrder)
    {
        GeneratePizzaOrder()
        GameObject pizza = Instantiate(crust);
        return pizza;
    }
}
