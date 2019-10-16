using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public int populationSize = 100;
    public float trainingDuration = 25;

    public GameObject agentPrefab;
    public Transform agentGroup;

    public float mutationRate = 8;

    public int[] layer;

    List<Agent> agents = new List<Agent>();

    Agent agent;

    private void Start()
    {
        StartCoroutine(InitCouroutine());
    }

    IEnumerator InitCouroutine()
    {

        NewGeneration();
        //Init(NeuralNetWorkViewer);
        Load();
        Focus();


        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());

    }

    IEnumerator Loop()
    {
        NewGeneration();

        Focus();

        yield return new WaitForSeconds(trainingDuration);
        StartCoroutine(Loop());
    }

    void NewGeneration()
    {

        AddRemoveAgent();
        agents.Sort();

        Mutate();

        ResetAgent();
        SetColor();
    }

    private void SetColor()
    {
        agents[0].SetFirstColor();

        for (int i = 1; i < populationSize/2; i++)
        {
            agents[i].SetDefaultColor();
        }
        for (int i = populationSize/2; i < populationSize; i++)
        {
            agents[i].SetMutatedColor();
        }

    }

    void AddRemoveAgent()
    {
        if(agents.Count != populationSize)
        {
            int dif = populationSize - agents.Count;

            if(dif > 0)
            {
                for (int i = 0; i < dif; i++)
                {
                    AddAgent();
                }
            }
            else
            {
                for (int i = 0; i < -dif; i++)
                {
                    RemoveAgent();
                }
            }

        }
    }

    void AddAgent()
    {
        agent = (Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup)).GetComponent<Agent>();

        agent.net = new NeuralNetwork(layer);

        Debug.Log(agent.net.layers.Length);
        Debug.Log(agent.net.layers[0]);

        agents.Add(agent);
    }
    void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].gameObject);

        agents.RemoveAt(agents.Count - 1);
    }

     void Focus()
    {
        //Focus neural viewer
        //NeuralNetworkViewer.instance.agent = agents[0];
        //NeuralNetworkViewer.instance.RefreshAxon();

        CameraController.instance.target = agents[0].transform;
    }

    void Mutate()
    {
        for (int i = agents.Count/2; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i - agents.Count/2].net);
            agents[i].net.Mutate(mutationRate);
        }
    }

    void ResetAgent()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].ResetAgent();
        }
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if(data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }
        }

        StopAllCoroutines();
        StartCoroutine(Loop());
    }


    [ContextMenu("Save")]
    public void Save()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }

        DataManager.instance.Save(nets);
    }

    public void Refocus()
    {
        agents.Sort();
        Focus();
    }

    public void End()
    {
        StopAllCoroutines();
        StartCoroutine(Loop());
    }

    public void ResetCrlh()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new NeuralNetwork(agent.net.layers);
        }

        End();
    }
}
