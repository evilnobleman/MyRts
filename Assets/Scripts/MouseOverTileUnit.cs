using UnityEngine;
using UnityEngine.UI;

public class MouseOverTileUnit : MonoBehaviour
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
    void Update(){
        Tile tile = mC.getTileUnderMouse();
        string type = "No Unit";
        if (tile != null){
            if (tile.unit != null){
                
                type = tile.unit.Name + "(" + tile.unit.getUnitType() + ")";
            }
        }


        myText.text = "Unit: " + type;
    }
}
