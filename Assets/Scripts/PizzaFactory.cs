using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PizzaFactory : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject receiptPrefab;

    public GameObject crust;
    public GameObject leftCheese;
    public GameObject rightCheese;
    public List<GameObject> sauces;
    public List<GameObject> ingredients;

    private Dictionary<string, GameObject> sauceTable = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> ingredientsTable = new Dictionary<string, GameObject>();

    public float ingredientOffset = 3; //inches
    public float radialIngredientOffset = 3f; //offset around circle
    public float ingredientOffsetScaleFactor; //to game world scale
    public float ingredientSkew = 3;
    public float crustOffset = 1;

    public float pizzaSlideInSpeed = 0.3f;

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

    public void Start()
    {
        // for faster lookup (probably)
        foreach (GameObject sauce in sauces) {
            sauceTable.Add(sauce.name.ToLower(), sauce);
            Debug.Log("[PizzaFactory] Adding sauce: " + sauce.name);
        }
        foreach (GameObject ingredient in ingredients) {
            ingredientsTable.Add(ingredient.name.ToLower(), ingredient);
            Debug.Log("[PizzaFactory] Adding ingredient: " + ingredient.name);
        }
    }

    // ORDER THE PIZZA
    public PizzaOrder GenerateNewPizzaOrder() {
        string[] PossibleSauce = sauceTable.Keys.ToArray();
        string[] PossibleIngredients = ingredientsTable.Keys.ToArray();

        Debug.Log("It's pizza time!");
        int diameter;
        int slices;
        List<string> leftIngredients = new List<string>();
        List<string> rightIngredients = new List<string>();
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
        slices = Random.Range(1,6) * 2;

        // set left ingredients
        int sauceChoice = Random.Range(0, PossibleSauce.Length);
        if(sauceChoice < 2){
            leftIngredients.Add(PossibleSauce[sauceChoice]);
        }

        if (Random.Range(0f, 1f) > 0.5)
        {
            leftIngredients.Add(leftCheese.name);
        }

        int numLeftIngredients = Random.Range(0, PossibleIngredients.Length);
        

        for(int i = 0; i < numLeftIngredients; i ++){
            string topping = PossibleIngredients[Random.Range(0, PossibleIngredients.Length)];
            if(!leftIngredients.Contains(topping)){
                leftIngredients.Add(topping);
            }
        }
        Debug.Log("leftIngredients" + string.Join(", ", leftIngredients.ToArray()));

        // set right ingredients
        sauceChoice = Random.Range(0, PossibleSauce.Length + 1);
        if(sauceChoice < 2){
            rightIngredients.Add(PossibleSauce[sauceChoice]);
        } // else no sauce

        if (Random.Range(0f, 1f) > 0.5)
        {
            leftIngredients.Add(rightCheese.name);
        }

        int numRightIngredients = Random.Range(0, PossibleIngredients.Length);
        

        for(int i = 0; i < numRightIngredients; i ++){
            string topping = PossibleIngredients[Random.Range(0, PossibleIngredients.Length)];
            if(!rightIngredients.Contains(topping)){
                rightIngredients.Add(topping);
            }
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

    // SUMMON THE PIZZA
    public GeneratedPizza CreatePizza(PizzaOrder pizzaOrder)
    {
        Vector3 centerPosition = new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z);
        Vector3 centerCameraPosition = Camera.main.ViewportToWorldPoint(centerPosition);
        GameObject instantiatedPizza = InstantiatePizza(pizzaOrder);

        instantiatedPizza.GetComponents<PizzaTrashBehaviour>()[0].eventSystem = gameObject;

        Vector3 transform = new Vector3(-5f, 0f, 0f);
        Vector3 offsetCameraPositon = centerCameraPosition + transform;
        GameObject instantiatedReceipt = Instantiate(receiptPrefab, offsetCameraPositon, Quaternion.identity);
        Color pizzaColor = Color.green;

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

        instantiatedPizza.GetComponent<DraggableObjectBehaviour>().animateSlide(new Vector2(centerCameraPosition.x , 15), centerCameraPosition, pizzaSlideInSpeed);
        instantiatedReceipt.GetComponent<DraggableObjectBehaviour>().animateSlide(new Vector2(offsetCameraPositon.x, 15), offsetCameraPositon, pizzaSlideInSpeed);

        return new GeneratedPizza(instantiatedPizza, instantiatedReceipt, currentPizzaGood);
    }

    public GameObject InstantiatePizza(PizzaOrder order)
    {
        Debug.Log("Instantiating pizza with order: " + order);
        GameObject pizza = Instantiate(crust);
        Vector2 origin = pizza.transform.position;

        float angleOffset = 0.1f;
        foreach (string ingredient in order.LeftIngredients)
        {
            Debug.Log("Adding left ingredient: " + ingredient);

            GameObject nextIngredient;
            bool isIngredient = ingredientsTable.TryGetValue(ingredient, out nextIngredient);


            if (isIngredient)
            {
                AddIngredientsRadially(nextIngredient, true, angleOffset);
                angleOffset += ingredientSkew;
                angleOffset *= -1;
            }
            else {
                bool isSauce = sauceTable.TryGetValue(ingredient, out nextIngredient);
                if (isSauce)
                {
                    GameObject newIngredient = Instantiate(nextIngredient);
                    Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + origin);
                    newIngredient.transform.position = origin;
                    newIngredient.transform.parent = pizza.transform;
                    newIngredient.GetComponent<Renderer>().sortingOrder = 1;
                }
                else
                {
                    GameObject newIngredient = Instantiate(leftCheese);
                    Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + origin);
                    newIngredient.transform.position = origin;
                    newIngredient.transform.parent = pizza.transform;
                    newIngredient.GetComponent<Renderer>().sortingOrder = 2;
                }
            }

            if (order.WellDone)
            {
                pizza.GetComponent<Renderer>().material.SetColor("_Color", new Color(150f/256f, 75f/256f, 0));
            }
        }

        angleOffset = 0;
        foreach (string ingredient in order.RightIngredients)
        {
            // yes this is duplicating code, no I am not going to refactor this
            Debug.Log("Adding right ingredient: " + ingredient);

            GameObject nextIngredient;
            bool isIngredient = ingredientsTable.TryGetValue(ingredient, out nextIngredient);

            if (isIngredient)
            {
                AddIngredientsRadially(nextIngredient, false, angleOffset);
                angleOffset += ingredientSkew;
                angleOffset *= -1;
            }
            else
            {
                bool isSauce = sauceTable.TryGetValue(ingredient, out nextIngredient);
                if (isSauce) {
                    GameObject newIngredient = Instantiate(nextIngredient);
                    Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + origin);
                    newIngredient.transform.position = origin;
                    newIngredient.transform.parent = pizza.transform;
                    newIngredient.GetComponent<Renderer>().sortingOrder = 1;
                    newIngredient.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                }
                else
                {
                    GameObject newIngredient = Instantiate(rightCheese);
                    Debug.Log("Creating new ingredient: " + newIngredient + "at pos: " + origin);
                    newIngredient.transform.position = origin;
                    newIngredient.transform.parent = pizza.transform;
                    newIngredient.GetComponent<Renderer>().sortingOrder = 2;
                }
            }
        }
        return pizza;

        void AddIngredientsRadially(GameObject ingredient, bool left, float angleOffset)
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
}
