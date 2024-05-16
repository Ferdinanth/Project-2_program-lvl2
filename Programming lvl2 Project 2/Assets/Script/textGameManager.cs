using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class textGameManager : MonoBehaviour
{

    private static textGameManager _MgrInstance;

    public static textGameManager myInstance
    {  
        get 
        { 
            _MgrInstance = FindAnyObjectByType<textGameManager>(); 
            if (_MgrInstance == null)
            {
                GameObject myGO = new GameObject("GameManager");
                myGO.AddComponent<textGameManager>();
                DontDestroyOnLoad(myGO);
            }
            return _MgrInstance;
        } 
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeManager()
    {
        textGameManager newMgr = textGameManager.myInstance;

    }
    public List<string> myInventory;
    private string playerName;

     
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetName(string input)
    {
        playerName = input;
    }
    public void SetName(TMP_InputField myInput)
    {
        playerName = myInput.text;
    }
    public string GetName()
    {
        return playerName;
    }
}
