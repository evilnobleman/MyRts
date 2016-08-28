using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MoneyUI : MonoBehaviour {
    Text myText;
	// Use this for initialization
	void Start () {
        myText = GetComponent<Text> ();

        if (myText== null){
            Debug.Log ("No text component for money ui");
            this.enabled = false;
            return;
        }

        
	}
	
	// Update is called once per frame
	void Update () {
        PlayerController pc = GameObject.FindObjectOfType<PlayerController> ();

        myText.text = "Money: $" + pc.money;
	}
}
