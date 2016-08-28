using UnityEngine;
using System.Collections;
public class PlayerController : MonoBehaviour {
    public int money {get; private set;}
    float timeLeft = 3;

    void  Update () {
        passiveMoney ();
    }
    private void moneyIncrease ( int i ) {
        if ( i > 0 ) {
            money += i;
        }
    }

    public void subtractMoney (int i) {
        if ( money < i ) {
            money = 0;
            return;
        }
        else {
            money -= i;
        }
        
    }

    private void passiveMoney () {
        timeLeft -= Time.deltaTime;
        if ( timeLeft < 0 ) {
            moneyIncrease ( 5);
            timeLeft = 3;
        }
    }
}