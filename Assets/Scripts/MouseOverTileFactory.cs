using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseOverTileFactory : MonoBehaviour
{
    Text myText;
    MouseController mC;

    void Start()
    {
        myText = GetComponent<Text>();

        if (myText == null)
        {
            Debug.Log("No text on this");
            this.enabled = false;
            return;
        }
        mC = GameObject.FindObjectOfType<MouseController>();


    }

    // Update is called once per frame
    void Update()
    {
        Tile tile = mC.getTileUnderMouse();
        string type = "No Factory";
        if (tile != null){
            if (tile.factory != null){
                type = tile.factory.getFactoryType().ToString();
            }
        }


        myText.text = "Factory Type: " + type;
    }
}
