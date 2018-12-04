using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameAI : MonoBehaviour {


    public Text timeText;
    public Text answerText;
    public Text numqText;
    public Text numCText;
    public Transform winLoseCanvas;

    public GameObject[] blackShapes = new GameObject[3];
    public GameObject[] geometryShapes = new GameObject[21];
    public GameObject[] candyShapes = new GameObject[5];
    public int exitCount;

    private float currentTime;
    private State currentState;
    private string currentAction;

    GameObject[] options = new GameObject[3];
    public float spinSpeed = 150f;
    State[] states = new State[5];
    int reward;
    float alpha = 0.5f;
    float gamma = 0.5f;
    float epsilon = 0.1f;
    float[,] SF = new float[5,2];
    int chunkCounter = 0;
    bool hasrotation = false;
    bool iscandyShaped = false;
    bool hasColor = false;
    whatShape ws;
    persianText ps;
    GameObject answer;
    int correctAns = 0;
    GameObject userSelected = null;
    int lastcorrectCounts = 0;

    public void setUserSelected( GameObject uS)
    {
        userSelected = uS;
    }

    // Use this for initialization
    void Start () {
        ps = answerText.GetComponent<persianText>();
        InitializeStates();
        currentState = states[0];
        currentAction = "";
        currentTime = 0f;
        makeQuestion();
    }
    void clean()
    {
        GameObject[] killEmAll;
        killEmAll = GameObject.FindGameObjectsWithTag("clickable");
        for (int i = 0; i < killEmAll.Length; i++)
        {
            Destroy(killEmAll[i].gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (hasrotation)
        {
            GameObject[] RotateEmAll;
            RotateEmAll = GameObject.FindGameObjectsWithTag("clickable");
            for (int i = 0; i < RotateEmAll.Length; i++)
            {
                RotateEmAll[i].transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(userSelected != null)
            {
                if (userSelected.name == answer.name + "(Clone)" && userSelected.tag == "clickable")
                {
                    correctAns++;
                    numCText.text = correctAns.ToString();
                    clean();
                    measureLevel();
                    makeQuestion();
                }
                else if (userSelected.name != answer.name + "(Clone)" && userSelected.tag == "clickable")
                {
                    clean();
                    measureLevel();
                    makeQuestion();
                }
            }

        }

    }
    void makeQuestion()
    {
        chunkCounter++;
        numqText.text = chunkCounter.ToString();
        if(!hasColor)
        {
            int rand = Random.Range(0, 3);
            int temp = rand;
            options[0] = blackShapes[temp];
            do
            {
                rand = Random.Range(0,3);
            } while (rand == temp);
            int temp2 = rand;
            options[1] = blackShapes[temp2];
            do
            {
                rand = Random.Range(0,3);
            } while (rand == temp || rand == temp2);
            options[2] = blackShapes[rand];
            rand = Random.Range(0, 3);
            answer = options[rand];
            ws = answer.GetComponent<whatShape>();
            string ans = ws.Answer;
            ps.s = ans;
            Vector3 p = new Vector3(-5f, -1.5f, 0f);
            Instantiate(options[0], p, Quaternion.identity);
            p = new Vector3(0f, -1.5f, 0f);
            Instantiate(options[1], p, Quaternion.identity);
            p = new Vector3(5f, -1.5f, 0f);
            Instantiate(options[2], p, Quaternion.identity);
        }else if(hasColor && !iscandyShaped)
        {
            int rand = Random.Range(0, 21);
            int temp = rand;
            options[0] = geometryShapes[temp];
            do
            {
                rand = Random.Range(0, 21);
            } while (rand == temp);
            int temp2 = rand;
            options[1] = geometryShapes[temp2];
            do
            {
                rand = Random.Range(0, 21);
            } while (rand == temp || rand == temp2);
            options[2] = geometryShapes[rand];
            rand = Random.Range(0, 3);
            answer = options[rand];
            ws = answer.GetComponent<whatShape>();
            string ans = ws.Answer;
            ps.s = ans;
            Vector3 p = new Vector3(-5f, -1.5f, 0f);
            Instantiate(options[0], p, Quaternion.identity);
            p = new Vector3(0f, -1.5f, 0f);
            Instantiate(options[1], p, Quaternion.identity);
            p = new Vector3(5f, -1.5f, 0f);
            Instantiate(options[2], p, Quaternion.identity);
        }else if (iscandyShaped)
        {
            int rand = Random.Range(0, 5);
            int temp = rand;
            options[0] = candyShapes[temp];
            do
            {
                rand = Random.Range(0, 5);
            } while (rand == temp);
            int temp2 = rand;
            options[1] = candyShapes[temp2];
            do
            {
                rand = Random.Range(0, 5);
            } while (rand == temp || rand == temp2);
            options[2] = candyShapes[rand];
            rand = Random.Range(0, 3);
            answer = options[rand];
            ws = answer.GetComponent<whatShape>();
            string ans = ws.Answer;
            ps.s = ans;
            Vector3 p = new Vector3(-5f, -1.5f, 0f);
            Instantiate(options[0], p, Quaternion.identity);
            p = new Vector3(0f, -1.5f, 0f);
            Instantiate(options[1], p, Quaternion.identity);
            p = new Vector3(5f, -1.5f, 0f);
            Instantiate(options[2], p, Quaternion.identity);
        }
        
    }
    void measureLevel()
    {
        float wi = chunkCounter % 5;
        if (wi == 0) wi = 5;
        float fi = correctAns / chunkCounter;
        SF[(int)wi-1, 0] = fi;
        SF[(int)wi-1, 1] = wi;
    }

     void FixedUpdate()
      {
        if(chunkCounter > exitCount)
        {
            if (winLoseCanvas.gameObject.activeInHierarchy == false)
            {
                winLoseCanvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (chunkCounter == 6)
            {
                currentAction = Exploration(currentState);
                doAction(currentAction);
                lastcorrectCounts = correctAns;
            }
            else if (chunkCounter > 6 && (chunkCounter - 1) % 5 == 0)
            {
                reward = calculateReward();
                Exploitation();
                doAction(currentAction);
                lastcorrectCounts = correctAns;
            }
        }

          currentTime = Time.timeSinceLevelLoad - currentTime;
          timeText.text = currentTime.ToString("00.00");

      }

    void InitializeStates()
    {
        states[0] = new State(1);
        states[0].AddActions("DoNothing", 4, 1);
        states[0].AddActions("AddColor", 7, 2);

        states[1] = new State(2);
        states[1].AddActions("DoNothing", 4, 2);
        states[1].AddActions("RemoveColor", 3, 1);
        states[1].AddActions("AddRotation", 7, 3);

        states[2] = new State(3);
        states[2].AddActions("DoNothing", 4, 3);
        states[2].AddActions("RemoveRotation", 3, 2);
        states[2].AddActions("CandyShapes", 7, 4);

        states[3] = new State(4);
        states[3].AddActions("DoNothing", 4, 4);
        states[3].AddActions("GeometryShapes", 3, 3);
        states[3].AddActions("AddRotation", 7, 5);

        states[4] = new State(5);
        states[4].AddActions("DoNothing", 7, 5);
        states[4].AddActions("RemoveRotation", 3, 4);


    }
    void Exploitation()
    {
        float this_Q;
        float next_Q;
        float new_Q;
        string new_action;
        State newState = findNextState(currentState , currentAction);
        new_action = Exploration(newState);
        this_Q = getQValue(currentState, currentAction);
        next_Q = getQValue(newState, new_action);
        new_Q = this_Q + alpha * (reward + gamma * next_Q - this_Q);
        setQValue(currentState, currentAction, new_Q);
        currentState = newState;
        currentAction = new_action;
       // Debug.Log("state:" + currentState.GetNumber());
      //  Debug.Log("action:" + currentAction);
    }

    string Exploration(State cs)
    {
        ArrayList actions = cs.GetActions();
        int n = actions.Count;
        float max = -1000f;
        float min = 1000f;
        float sumH = 0, sumE =0;
        float PH = 0f, PE = 0f, PM = 0f, PFinal = 0f;
        string ans = "";

        foreach (ArrayList item in actions)
        {
            if ((float)item[1] >= max) max = (float)item[1];
            if ((float)item[1] <= min) min = (float)item[1];
        }

        foreach (ArrayList item in actions)
        {
            sumE += (max + epsilon) - (float)item[1];
            sumH += (float)item[1] - (min - epsilon);
        }

        foreach (ArrayList item in actions)
        {
           PE = ( (max + epsilon) - (float)item[1]) / sumE;
           PM = 1 / n;
           PH = ( (float)item[1] - (min - epsilon) ) / sumH;
           item.Insert(3, PE);
           item.Insert(4, PM);
           item.Insert(5, PH);
        }
        float skillFactorUp = 0f , skillFactorDown = 0f, skillFactor;
        for(int i=0; i<5; i++)
        {
            skillFactorUp += SF[i, 0] * SF[i, 1];
            skillFactorDown += SF[i, 1];
        }
        skillFactor = skillFactorUp / skillFactorDown;
        float muA = degreeOfMemership("A", skillFactor);
     //   float muM = degreeOfMemership("M", skillFactor);
        float muP = degreeOfMemership("P", skillFactor);

        foreach (ArrayList item in actions)
        {
            PFinal = Mathf.Lerp(Mathf.Lerp( (float)item[3], (float)item[4], (1-muA) ), (float)item[5], muP);
            item.Insert(6, PFinal);
        }
        float pmax = -1000f;
        foreach (ArrayList item in actions)
        {
            if( (float)item[6] >= pmax)
            {
                pmax = (float)item[6];
                ans = (string)item[0];
            }
        }
       // Debug.Log(ans);
        return ans;
    }
    float degreeOfMemership(string s, float sf)
    {
        float ans = 0f;
        if(s == "A")
        {
            if (sf <= 0.25) ans = 1;
            else if (sf >= 0.75) ans = 0;
            else
            {
                ans = 1 + ( (float)(sf - 0.25) * (float)(-1 / 0.5) );
            }
            
        }else if(s == "M")
        {
            if (sf <= 0.25) ans = 0;
            else if (sf >= 0.75) ans = 0;
            else if (sf == 0.5) ans = 1;
            else
            {
                if (sf < 0.5 && sf > 0.25)
                {
                    ans =  ((float)(sf - 0.25) * (float)(1 / 0.25));
                }
                else if( sf > 0.5 && sf < 0.75)
                {
                    ans = 1 + ((float)(sf - 0.5) * (float)(-1 / 0.25));
                }
            }
        }
        else if(s == "P")
        {
            if (sf <= 0.25) ans = 0;
            else if (sf >= 0.75) ans = 1;
            else
            {
                ans = ((float)(sf - 0.25) * (float)(1 / 0.5));
            }
        }
        return ans;
    }
 
   int calculateReward()
    {
        reward = correctAns - lastcorrectCounts;
        return reward;
    }

    float getQValue(State state, string action)
    {
        float qValue = 0f;
        ArrayList actions = state.GetActions();
        foreach (ArrayList item in actions)
        {
            if ((string)item[0] == action)
            {
                qValue = (float)item[1];
                break;
            }
        }
        return qValue;
    }

    State findNextState(State state , string action)
    {
        int nextStateNumber = states.Length;
        ArrayList actions = state.GetActions();
        foreach (ArrayList item in actions)
        {
            if ((string)item[0] == action)
            {
                nextStateNumber = (int)item[2];
                break;
            }
        }
        return states[nextStateNumber - 1];
    }
    void doAction(string action)
    {
        switch (action)
        {
            case "DoNothing":
                break;
            case "AddColor":
                AddColor();
                break;
            case "RemoveColor":
                RemoveColor();
                break;
            case "AddRotation":
                AddRotation();
                break;
            case "RemoveRotation":
                RemoveRotation();
                break;
            case "CandyShapes":
                CandyShapes();
                break;
            case "GeometryShapes":
                GeometryShapes();
                break;
            default:
                break;
        }

    }

    void AddColor()
    {
        hasColor = true;
    }
    void RemoveColor()
    {
        hasColor = false;
    }

    void AddRotation()
    {
        hasrotation = true;
    }
    void RemoveRotation()
    {
        hasrotation = false;
    }
    void CandyShapes()
    {
        iscandyShaped = true;
    }
    void GeometryShapes()
    {
        iscandyShaped = false;
    }
    void setQValue(State state, string action, float QValue)
    {
        ArrayList actions = state.GetActions();
        foreach (ArrayList item in actions)
        {
            if ((string)item[0] == action)
            {
                item[1] = QValue;
                break;
            }
        }
    }


    public State getCurrentState()
    {
        return currentState;
    }
 }
