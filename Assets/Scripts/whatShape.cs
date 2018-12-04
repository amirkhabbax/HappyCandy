using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whatShape : MonoBehaviour {

    public string Answer;
    public Texture2D cursor;
    gameAI ga;

    void Awake()
    {
        ga = GameObject.Find("GameManager").gameObject.GetComponent<gameAI>();
    }
    void OnMouseOver()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        if (Input.GetMouseButtonDown(0))
        {
            ga.setUserSelected(this.gameObject);
        }
        
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

}
