using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class MouseController : MonoBehaviour {

    Vector3 lastFramePosition;
    Vector3 currFramePosition;
    bool canBuildFactory = false;
    bool hasSelect = false;
    Factory.factoryType fType;
    int value;
    Sprite [] selectionSprites;
    UnitSheet unitSheet;
    Tile selectedTile;
    Unit selectedUnit;
    Tile buildUnitSelectedTile;
    bool canBuildUnit = false;
    bool hasUnitSelected = false;
    void Start () {
        unitSheet = new UnitSheet ();

        selectionSprites = Resources.LoadAll<Sprite> ( "Sprites/SelectionSprites" );

        //foreach ( Sprite s in selectionSprites ) {
        //    Debug.Log ( s.name );
        //}
        unitSheet.ReadInUnits ();
    }
    void Update () {
        updateCamera ();
        buildFactory ();
        selectFactory ();
        selectUnit ();
        moveUnitOnClick ();
    }

    public Tile getTileUnderMouse () {
        int x = Mathf.FloorToInt ( currFramePosition.x );
        int y = Mathf.FloorToInt ( currFramePosition.y );

        //Debug.Log(x + ", " + y);

        return World.current.getTileAt ( x , y );
    }

    public Vector3 getCurrentMousePosition () {
        return currFramePosition;
    }

    public void buildFactoryModeButton () {
        canBuildFactory = true;
        value = GameObject.Find ( "FactoryBuildMenu" ).GetComponentInChildren<Dropdown> ().value;
        setFactoryType ( value );
    }

    public void updateCamera () {
        if ( !EventSystem.current.IsPointerOverGameObject () ) {
            currFramePosition = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
            currFramePosition.z = 0;
            if ( Input.GetMouseButton ( 1 ) || Input.GetMouseButton ( 2 ) ) {
                Vector3 diff = lastFramePosition - currFramePosition;
                Camera.main.transform.Translate ( diff );
            }
            lastFramePosition = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
            lastFramePosition.z = 0;

            //Debug.Log(lastFramePosition);
        }
    }

    private void buildFactory () {
        if ( !EventSystem.current.IsPointerOverGameObject () ) {
            if ( canBuildFactory && Input.GetMouseButtonDown ( 0 ) ) {
                hasSelect = false;
                World.current.createFactoryOnWorld ( getTileUnderMouse ().x , getTileUnderMouse ().y , fType );
                canBuildFactory = false;
                hasSelect = true;

            }
        }
    }


    private void setFactoryType ( int i ) {
        if ( i == 0 ) {
            fType = Factory.factoryType.Barracks;
        }
        else if ( i == 3 ) {
            fType = Factory.factoryType.Sea;
        }
        else if ( i == 2 ) {
            fType = Factory.factoryType.Armor;
        }
        else if ( i == 1 ) {
            fType = Factory.factoryType.Air;
        }
        else {
            Debug.Log ( "No factory type found" );
        }
    }

    public void onDropdownChanged () {
        value = GameObject.Find ( "FactoryBuildMenu" ).GetComponentInChildren<Dropdown> ().value;
        setFactoryType ( value );
    }

    private void selectFactory () {
        selectedTile = getTileUnderMouse ();
        if ( !EventSystem.current.IsPointerOverGameObject () ) {
            if ( selectedTile.factory != null ) {
                if ( Input.GetMouseButtonDown ( 0 ) && hasSelect == false ) {
                    buildUnitSelectedTile = selectedTile;
                    createSelectionCircle (selectedTile);
                    selectedTile.factory.isSelected = true;
                    Debug.Log ( selectedTile.factory.isSelected );
                    populateUnitDropdown ( selectedTile );
                    hasSelect = true;

                }
                else if ( Input.GetMouseButtonDown ( 0 ) && hasSelect == true ) {
                    Destroy ( GameObject.Find ( "Selection_Square" ) );
                    depopulateUnitDropdown ( selectedTile );
                    hasSelect = false;

                    //Debug.Log ("Setting to false.....");
                }
            }
        }
    }

    private void selectUnit () {
        if ( !EventSystem.current.IsPointerOverGameObject () ) {
            if ( selectedTile.unit != null ) {
                Tile tile = selectedTile;
                if ( Input.GetMouseButtonDown ( 0 ) && hasSelect == false ) {
                    createSelectionCircle (tile);
                    tile.unit.isSelected = true;
                    selectedUnit = tile.unit;
                    hasSelect = true;
                    hasUnitSelected = true;
                    
                }
                else if ( Input.GetMouseButtonDown ( 0 ) && hasSelect == true ) {
                    Destroy ( GameObject.Find ( "Selection_Square") );
                    tile.unit.isSelected = false;
                    hasSelect = false;
                    hasUnitSelected = false;
                    //Debug.Log ("Setting to false.....");
                }
            }
        }
    }

    public void buildUnitButton () {
        Dropdown dropdown = GameObject.Find ( "UnitBuildMenu" ).GetComponentInChildren<Dropdown> ();
        if ( buildUnitSelectedTile != null && buildUnitSelectedTile.factory.isSelected ) {
            buildUnitSelectedTile.factory.createUnit (unitSheet.unitSheetArray[dropdown.value]);
        }
        else{
            Debug.Log ("No factory selected");
        }
    }

    private void populateUnitDropdown (Tile tile) {
        if (tile.factory.isSelected) {
            Dropdown dropdown =  GameObject.Find ( "UnitBuildMenu" ).GetComponentInChildren<Dropdown> ();
            dropdown.options.Clear();
            dropdown.options.Capacity = 0;
            List<Dropdown.OptionData> tempList = new List<Dropdown.OptionData> ();
            tempList.Capacity = unitSheet.unitSheetArray.Length;
            for ( int i = 0; i < unitSheet.unitSheetArray.Length; i++ ) {
                tempList.Add(new Dropdown.OptionData( unitSheet.unitSheetArray [ i ].Name));
                Debug.Log (tempList.Count);
            }

            dropdown.options = tempList;
        }
    }

    private void depopulateUnitDropdown (Tile tile) {
        tile.factory.isSelected = false;
        Dropdown dropdown = GameObject.Find ( "UnitBuildMenu" ).GetComponentInChildren<Dropdown> ();
        dropdown.options.Clear ();
        dropdown.options.Capacity = 0;
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        options.Add (new Dropdown.OptionData("No Factory"));
        dropdown.AddOptions (options);
    }

    private void createSelectionCircle (Tile tile) {
        GameObject Select = new GameObject ();
        Select.name = "Selection_Square";
        Vector3 pos = new Vector3 ( tile.x - .2f , tile.y - .452f , 0 );
        Select.transform.localScale = new Vector3 (1.6f,1.6f);
        Select.transform.Translate ( pos );
        Debug.Log ( tile.x + ", " + tile.y );
        if ( Select.GetComponent<SpriteRenderer> () == null ) {
            Select.AddComponent<SpriteRenderer> ();
        }
        SpriteRenderer Select_sr = Select.GetComponent<SpriteRenderer> ();
        foreach (Sprite s in selectionSprites) {
            if (s.name == Select.name) {
                Select_sr.sprite = s;
                break;
            }
        }
    }

    public void moveUnitOnClick () {
        if (selectedUnit != null) {
            if (selectedUnit.isSelected) {
                if ( Input.GetMouseButtonDown ( 0 ) ) {
                    
                    selectedUnit.moveUnit (getTileUnderMouse().x, getTileUnderMouse().y);
                    Debug.Log ( "Should be moving" );
                }
            }
        }
    }
}
