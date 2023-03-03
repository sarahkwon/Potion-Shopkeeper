using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartButton : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClick() {
        gameController.PurchaseAndWin();
    }
}
