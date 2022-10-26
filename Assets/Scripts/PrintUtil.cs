using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintUtil : MonoBehaviour
{
    private static PrintUtil _instance;

    public static PrintUtil Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PrintUtil();
            }
            return _instance;
        }
    }

    private PrintUtil()
    {

    }

    public void Print(object message){
        print(message);
    }
}
