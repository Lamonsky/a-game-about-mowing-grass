using UnityEngine;

public class ShopScript : MonoBehaviour
{
    
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InteractWithObject(GameObject gameObject)
    {
        var playerScript = gameObject.GetComponent<PlayerMovementScript>();
        if (playerScript != null)
        {
            Debug.Log("Shop: Zmieniam prêdkoœæ gracza!");
            playerScript.SetSpeed(100f);
        }
        else
        {
            Debug.LogWarning("Shop: Obiekt nie posiada PlayerMovementScript!");
        }
        

    }

}
