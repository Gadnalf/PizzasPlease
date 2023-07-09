using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class PizzaFactory : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject receiptPrefab;

    public GameObject crust;
    public GameObject leftCheese;
    public GameObject rightCheese;
    public List<GameObject> sauces;
    public List<GameObject> ingredients;
    public List<GameObject> slices;

    public float ingredientOffset = 3; //inches
    public float radialIngredientOffset = 3f; //offset around circle
    public float ingredientOffsetScaleFactor; //to game world scale
    public float ingredientSkew = 3;
    public float crustOffset = 1;

    public float pizzaSlideInSpeed = 0.3f;

    public struct PizzaOrder
    {
        public PizzaOrder(int diameter, int slices, bool[] leftIngredients, bool[] rightIngredients, bool wellDone)
        {
            Diameter = diameter;
            Slices = slices;
            LeftIngredients = leftIngredients;
            RightIngredients = rightIngredients;
            WellDone = wellDone;
        }

        public int Diameter { get; set; }
        public int Slices { get; set; }
        public bool[] LeftIngredients { get; set; }
        public bool[] RightIngredients { get; set; }
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
        Debug.Log("It's pizza time!");
        int diameter;
        int slices;
        bool[] leftIngredients = new bool[1 + sauces.Count + ingredients.Count];
        bool[] rightIngredients = new bool[1 + sauces.Count + ingredients.Count];
        bool wellDone;


        // set diameter
        // int personal = 10;
        int small = 12;
        int medium = 14;
        int large = 16; 
        int[] sizes = {small, medium, large};
        diameter = sizes[Random.Range(0,2)];
        Debug.Log("diameter" + diameter);

        // set number of slices
        slices = Random.Range(0,6) * 2;

        // set left ingredients
        if (Random.Range(0f, 1f) > 0.5)
        {
            leftIngredients[0] = true;
        }

        int sauceChoice = Random.Range(1, sauces.Count + 1);
        if (sauceChoice <= 2)
        {
            leftIngredients[sauceChoice] = true;
        }

        int numLeftIngredients = Random.Range(0, ingredients.Count);
        for (int i = 0; i < numLeftIngredients; i++)
        {
            leftIngredients[Random.Range(1 + sauces.Count, ingredients.Count)] = true;
        }
        Debug.Log("leftIngredients" + string.Join(", ", leftIngredients.ToArray()));

        // set right ingredients
        if (Random.Range(0f, 1f) > 0.5)
        {
            rightIngredients[0] = true;
        }

        sauceChoice = Random.Range(1, sauces.Count + 1);
        if (sauceChoice <= 2)
        {
            rightIngredients[sauceChoice] = true;
        }

        int numRightIngredients = Random.Range(0, ingredients.Count);
        for (int i = 0; i < numRightIngredients; i++)
        {
            rightIngredients[Random.Range(1 + sauces.Count, ingredients.Count)] = true;
        }
        Debug.Log("rightIngredients" + string.Join(", ", rightIngredients.ToArray()));

        // is well done
        if (Random.Range(0f,1f) > 0.5){
            wellDone = false;
        } else {
            wellDone = true;
        }
        Debug.Log("wellDone" + wellDone);

        return new PizzaOrder(diameter, slices, leftIngredients, rightIngredients, wellDone);
    }

    // MESS UP THE PIZZA
    public PizzaOrder MessUpPizzaOrder(PizzaOrder pizzaOrder)
    {
        float rand = Random.Range(0f, 1f);
        if (rand < 0.3)
        {
            // Flip something
            int i = Random.Range(0, pizzaOrder.LeftIngredients.Length);
            if (Random.Range(0f, 1f) > 0.5)
            {
                pizzaOrder.LeftIngredients[i] = !pizzaOrder.LeftIngredients[i];
            }
            else
            {
                pizzaOrder.RightIngredients[i] = !pizzaOrder.RightIngredients[i];
            }
        }
        else if (rand < 0.6)
        {
            if (pizzaOrder.Slices == 0 || Random.Range(0f, 1f) > 0.5)
            {
                pizzaOrder.Slices += 2;
            }
            else
            {
                pizzaOrder.Slices -= 2;
            }
        }
        else if (rand < 0.75)
        {
            pizzaOrder.WellDone = !pizzaOrder.WellDone;
        }
        else if (rand < 0.90)
        {
            if (pizzaOrder.LeftIngredients != pizzaOrder.RightIngredients)
            {
                if (Random.Range(0f, 1f) > 0.5)
                {
                    pizzaOrder.LeftIngredients = pizzaOrder.RightIngredients;
                }
                else
                {
                    pizzaOrder.RightIngredients = pizzaOrder.LeftIngredients;
                }
            }
            else
            {
                pizzaOrder.WellDone = !pizzaOrder.WellDone;
            }
        }
        else
        {
            PizzaOrder newPizzaOrder = GenerateNewPizzaOrder();
            if (newPizzaOrder.Equals(pizzaOrder))
            {
                pizzaOrder.WellDone = !pizzaOrder.WellDone;
            }
            return newPizzaOrder;
        }
        return pizzaOrder;
    }

    // SUMMON THE PIZZA
    public GeneratedPizza CreatePizza(PizzaOrder pizzaOrder)
    {
        Vector3 centerPosition = new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z);
        Vector3 centerCameraPosition = Camera.main.ViewportToWorldPoint(centerPosition);
        GameObject instantiatedPizza;

        Vector3 transform = new Vector3(-5f, 0f, 0f);
        Vector3 offsetCameraPositon = centerCameraPosition + transform;
        GameObject instantiatedReceipt = Instantiate(receiptPrefab, offsetCameraPositon, Quaternion.identity);
        GameObject gameText = instantiatedReceipt.transform.GetChild(0).GetChild(0).gameObject;

        Debug.Log(gameText.name);

        string orderString = GenerateReceiptStringFromPizzaOrder(pizzaOrder);
        Debug.Log(orderString);

        gameText.GetComponent<TextMeshProUGUI>().text = GenerateReceiptStringFromPizzaOrder(pizzaOrder);

        bool currentPizzaGood = Random.Range(0, 2) != 0;
        if (!currentPizzaGood)
        {
            instantiatedPizza = InstantiatePizza(MessUpPizzaOrder(pizzaOrder));
        }
        else
        {
            instantiatedPizza = InstantiatePizza(pizzaOrder);
        }

        // Receipt has spherical interpolation
        instantiatedPizza.GetComponent<DraggableObjectBehaviour>().animateSlide(new Vector2(centerCameraPosition.x , 15), centerCameraPosition, pizzaSlideInSpeed);
        instantiatedReceipt.GetComponent<DraggableObjectBehaviour>().animateSlide(new Vector2(offsetCameraPositon.x, 15), offsetCameraPositon, pizzaSlideInSpeed, true);

        return new GeneratedPizza(instantiatedPizza, instantiatedReceipt, currentPizzaGood);
    }

    public GameObject InstantiatePizza(PizzaOrder order)
    {
        GameObject pizza = Instantiate(crust);
        Vector2 origin = pizza.transform.position;

        // Handle right
        float angleOffset = 0.15f;
        bool left = false;
        for (int i = 0; i < order.LeftIngredients.Length; i++)
        {
            if (order.LeftIngredients[i])
            {
                AddToppingForIndex(i);
            }
        }

        // left
        angleOffset = 0.15f;
        left = true;
        for (int i = 0; i < order.RightIngredients.Length; i++)
        {
            if (order.RightIngredients[i])
            {
                AddToppingForIndex(i);
            }
        }

        if (order.Slices != 0) {
            AddSlices(slices[order.Slices/2 - 1]);
        }

        if (order.WellDone)
        {
            pizza.GetComponent<Renderer>().material.SetColor("_Color", new Color(150f/256f, 75f/256f, 0));
        }

        pizza.GetComponent<PizzaTrashBehaviour>().eventSystem = gameObject;
        pizza.transform.localScale = pizza.transform.localScale * (order.Diameter / 16f);

        return pizza;

        void AddToppingForIndex(int index)
        {
            if (index == 0)
            {
                AddCheese();
            }
            else if (index < sauces.Count + 1)
            {
                AddSauce(sauces[index - 1]);
            }
            else
            {
                AddTopping(ingredients[index - sauces.Count - 1]);
            }
        }

        void AddSlices(GameObject ingredient) {
            GameObject slices = Instantiate(ingredient);
            slices.transform.position = origin;
            slices.transform.parent = pizza.transform;
            slices.GetComponent<Renderer>().sortingOrder = 4;
        }

        void AddSauce(GameObject ingredient)
        {
            GameObject newIngredient = Instantiate(ingredient);
            Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + origin);
            newIngredient.transform.position = origin;
            newIngredient.transform.parent = pizza.transform;
            newIngredient.GetComponent<Renderer>().sortingOrder = 1;
            if (!left) {
                newIngredient.transform.Rotate(new Vector3(0, 0, 180));
            }
        }

        void AddCheese()
        {
            GameObject newIngredient = !left ? Instantiate(leftCheese) : Instantiate(rightCheese);
            Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + origin);
            newIngredient.transform.position = origin;
            newIngredient.transform.parent = pizza.transform;
            newIngredient.GetComponent<Renderer>().sortingOrder = 2;
        }

        void AddTopping(GameObject ingredient)
        {
            AddIngredientsRadially(ingredient, angleOffset);
            angleOffset += ingredientSkew;
            angleOffset *= -1;
        }

        void AddIngredientsRadially(GameObject ingredient, float angleOffset)
        {
            float angle = angleOffset;
            for (float radius = ingredientOffset; radius < (order.Diameter - crustOffset); radius += ingredientOffset)
            {
                //Debug.Log("Current circumference: " + 2 * 3.14f * radius);
                float ingredientAngle = 360/Mathf.RoundToInt(2 * 3.14f * radius / radialIngredientOffset);
                angle += ingredientAngle / 2;
                // get angle based on circumference
                for (int i = 0; i < (360 / ingredientAngle); i++)
                {
                    angle += ingredientAngle;
                    //Debug.Log("Current angle: " + angle);
                    if (left && (angle % 360 < 180) || !left && (angle % 360 >= 180))
                    {
                        // rotate a vector?
                        Vector2 rotationVector = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.down;
                        Vector2 offsetVector = rotationVector * (ingredientOffsetScaleFactor * (radius + Random.Range(-0.2f*ingredientOffset, 0.2f*ingredientOffset)));
                        Vector2 ingredientPos = origin + offsetVector;
                        GameObject newIngredient = Instantiate(ingredient);
                        //Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + ingredientPos);
                        newIngredient.transform.position = (Vector3)ingredientPos + new Vector3(0, 0, -1);
                        newIngredient.GetComponent<Renderer>().sortingOrder = 3;
                        newIngredient.transform.parent = pizza.transform;
                        newIngredient.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
                    }
                }
            }
        }
    }

    string GenerateReceiptStringFromPizzaOrder(PizzaOrder order) {
        int number = Random.Range(10000, 100000);
        string output = "Order #" + number + ":\n\n";
        string header = "Order #" + number + ":\n\n";

        string size = "1x " + order.Diameter + " inch Pizza, " + order.Slices + " slices\n";

        output += size;

        // Concatenate lists here
        Dictionary<string, int> ingredientList = new Dictionary<string, int>();

        for (int i = 0; i < order.LeftIngredients.Length; i++) {
            string ingredient;
            if (i == 0)
            {
                ingredient = "cheese";
            }
            else if (i < sauces.Count + 1)
            {
                ingredient = sauces[i-1].name;
            }
            else
            {
                ingredient = ingredients[i - sauces.Count - 1].name;
            }
            
            ingredientList[ingredient] = 0;
            if (order.LeftIngredients[i])
            {
                ingredientList[ingredient] += 1;
            }
            if (order.RightIngredients[i])
            {
                ingredientList[ingredient] += 2;
            }
        }

        string GenerateSidedOption(string side, string ingredient) {
            string[] options = {
                "{0} side {1}",
                "{1}({0})",
                "{1}, {0} side only",
                "{0} half {1}",
                "{1}, only on {0} side",
                "{1}, only on {0} half"
            };

            int index = Random.Range(0, options.Length);
            return System.String.Format(options[index], side, ingredient);
        }

        string GenerateWholeOption(string ingredient) {
            string[] options = {
                "{0}",
                "{0}, both sides",
                "not not {0}",
            };
            int index = Random.Range(0, options.Length);
            return System.String.Format(options[index], ingredient);
        }

        foreach (string ingredient in ingredientList.Keys) {
            if (ingredientList[ingredient] == 0) {
                continue;
            }
            string processedIngredient = ingredient.Replace('_', ' ');
            output += "- ";
            switch(ingredientList[ingredient]) {
                case 1:
                    output += GenerateSidedOption("left", processedIngredient);
                    break;
                case 2:
                    output += GenerateSidedOption("right", processedIngredient);
                    break;
                case 3:
                    output += GenerateWholeOption(processedIngredient);
                    break;
            }
            output += "\n";
        }

        if (order.WellDone) {
            output += "Cooked well done\n";
        }

        return output;
    }
}
