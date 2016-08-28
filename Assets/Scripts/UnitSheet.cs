using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class UnitSheet{
    public Unit [] unitSheetArray = new Unit [200];

    public void ReadInUnits () {
        string line;
        int counter = 0;
        System.IO.StreamReader file = new System.IO.StreamReader ( @"\Users\Derrian\Rts\Assets\Scripts\UPT.txt" );
        while ( ( line = file.ReadLine () ) != null ) {
            string stringType = file.ReadLine ().Trim();
            string name = file.ReadLine ().Trim ();
            string health = file.ReadLine ().Trim ();
            string damage = file.ReadLine ().Trim ();
            string movement = file.ReadLine ().Trim ();
            string spriteName = file.ReadLine ().Trim ();
            Unit.unitType type = Unit.unitType.Land;
            if ( stringType == "Land" ) {
                type = Unit.unitType.Land;
            }
            else if ( stringType == "Air" ) {
                type = Unit.unitType.Air;
            }
            else if ( stringType == "Sea" ) {
                type = Unit.unitType.Sea;
            }
            Debug.Log (type + name + health + damage + movement + spriteName);
          unitSheetArray [counter] = Unit.createUnitPrototype (type, name, int.Parse(health), int.Parse(damage), int.Parse(movement), spriteName);
            counter++;
        }

        Array.Resize<Unit> (ref unitSheetArray, counter);

        Debug.Log (unitSheetArray.Length);
    }
}
