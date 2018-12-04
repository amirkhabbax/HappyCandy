using UnityEngine;
using System.Collections;
using ArabicSupport;
using UnityEngine.UI;

public class persianText : MonoBehaviour {
    public string s;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(s);

    }
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(s);
    }
}
