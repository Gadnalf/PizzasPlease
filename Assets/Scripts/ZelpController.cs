using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZelpController : MonoBehaviour
{
    public struct Review
    {
        public Review(int stars, string text)
        {
            Stars = stars;
            TextContent = text;
        }

        public int Stars { get; set; }
        public string TextContent { get; set; }
    }

    Queue<Review> lastNReviews;

    public void Start()
    {
        lastNReviews = new Queue<Review>();
    }
}
