using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] UIDocument uiDocument;
    private VisualElement rootElement;

    private CharacterController cc;
    private Vector3 velocity;
    private GameObject highlightedItem;
    private Color originalColor;
    private TextMeshPro pickupCounterTMP;

    public float speed = 5.0f;
    public float jump = 1f;
    public float gravity = -9.81f;
    public float pickupRange = 2f;

    public float pickedupItemCount = 0;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        rootElement = uiDocument.rootVisualElement;
    }


    void Update()
    {
        UpdateSpeedUI();

        MovePlayer();

        HighlightItem();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenShop();
        }

    }

    private void UpdateSpeedUI()
    {
        var SpeedLabel = rootElement.Q<Label>(name: "SpeedLabel");
        if (SpeedLabel != null)
        {
            SpeedLabel.text = "Prêdkoœæ: " + speed.ToString("F1");
        }
    }

    private void MovePlayer()
    {
        var move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        if (cc.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            cc.height = cc.height / 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            cc.height = cc.height * 2;
        }

        velocity.y += gravity * Time.deltaTime;

        var finalMove = move * speed;

        finalMove.y = velocity.y;

        cc.Move(finalMove * Time.deltaTime);
    }

    private void TryPickupItem()
    {
        var PickupItemsCountLabel = rootElement.Q<Label>(name: "PickupItemsCountLabel");
        RaycastHit hit;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = Camera.main.transform.forward;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, pickupRange))
        {
            GameObject item = hit.collider.gameObject;
            if (item.CompareTag("PickupItem"))
            {
                var pickupScript = item.GetComponent<PickupItemScript>();

                Destroy(hit.collider.gameObject);

                pickedupItemCount += pickupScript.pickupValue;

                if (PickupItemsCountLabel != null)
                {
                    PickupItemsCountLabel.text = "Przedmioty: " + pickedupItemCount.ToString();
                }
                else
                {
                    Debug.LogWarning("Nie znaleziono obiektu PickupCounterTMP lub nie przypisano komponentu TextMeshPro!");
                }
                Debug.Log("Podniesiono przedmiot!");
            }
        }
    }

    private void OpenShop() 
    {
        RaycastHit hit;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = Camera.main.transform.forward;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, pickupRange))
        {
            GameObject shop = hit.collider.gameObject;
            if (shop.CompareTag("Shop"))
            {
                var shopScript = shop.GetComponent<ShopScript>();
                shopScript.InteractWithObject(this.gameObject);
            }
        }
    }

    private void HighlightItem()
    {
        RaycastHit hit;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = Camera.main.transform.forward;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickupItem") || hit.collider.CompareTag("Shop"))
            {
                GameObject item = hit.collider.gameObject;
                if (highlightedItem != item)
                {
                    RemoveHighlight();
                    highlightedItem = item;
                    var renderer = highlightedItem.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalColor = renderer.material.color;
                        renderer.material.color = Color.yellow;
                    }
                }
                return;
            }
        }
        RemoveHighlight();
    }

    private void RemoveHighlight()
    {
        if (highlightedItem != null)
        {
            var renderer = highlightedItem.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
            highlightedItem = null;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }

}
