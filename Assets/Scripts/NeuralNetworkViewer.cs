using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkViewer : MonoBehaviour
{
    public static NeuralNetworkViewer instance;

    public Gradient colorGradient;

    const float decalX = 100;
    const float decalY = 20;

    public Transform viewerGroup;

    public GameObject neuronPrefab;
    public GameObject axonPrefab;

    public  GameObject fitnessPrefab;
    private GameObject fitnesTransform;

    public Agent agent;

    private Image[][]   neurons;
    private Text[][]    neuronsValue;
    private Image[][][] axons;

    private GameObject neuron;
    private GameObject axon;
    private Text       fitness;

    private int   i;
    private int   x;
    private int   y;
    private int   z;
    private float posY;
    private float posZ;
    private float yAdd;
    private float zAdd;

    private void Awake()
    {
        instance = this;
    }

    public void Init(Agent _agent)
    {
        agent = _agent;
        Init(agent.net);
    }

    void Init(NeuralNetwork net)
    {
        for (i = viewerGroup.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(viewerGroup.GetChild(i).gameObject);
        }

        axons = new Image[net.axons.Length][][];

        for (x = 0; x < net.axons.Length; x++)
        {
            axons[x] = new Image[net.axons[x].Length][];

            for (y = 0; y < net.axons[x].Length; y++)
            {
                axons[x][y] = new Image[net.axons[x][y].Length];

                for (z = 0; z < net.axons[x][y].Length; z++)
                {
                    if ((net.axons[x].Length) % 2 == 0)
                    {
                        yAdd = 1.0f;
                    }
                    else
                    {
                        yAdd = 0;
                    }

                    if ((net.axons[x][y].Length) % 2 == 0)
                    {
                        zAdd = 1.0f;
                    }
                    else
                    {
                        zAdd = 0;
                    }

                    if (y % 2 == 0)
                    {
                        posY = y + yAdd;
                    }
                    else
                    {
                        posY = -y - 1 + yAdd;
                    }

                    if (z % 2 == 0)
                    {
                        posZ = z + zAdd;
                    }
                    else
                    {
                        posZ = -z - 1 + zAdd;
                    }

                    float midPosX = decalX                 * (x + .5f);
                    float midPosY = (posY + posZ) * decalY * .5f;

                    float zAngle = Mathf.Atan2((posY - posZ) * decalY, decalX) * Mathf.Rad2Deg;


                    axon = Instantiate(axonPrefab, transform.position, Quaternion.identity, viewerGroup);

                    axon.GetComponent<RectTransform>().anchoredPosition = new Vector2(midPosX, (midPosY));
                    axon.GetComponent<RectTransform>().eulerAngles      = new Vector3(0, 0, zAngle);

                    axon.GetComponent<RectTransform>().sizeDelta =
                        new Vector2(new Vector2(decalX, (posY - posZ) * decalY).magnitude * 1 - 35, 2);

                    axons[x][y][z] = axon.GetComponent<Image>();
                }
            }
        }

        neurons      = new Image[net.neurons.Length][];
        neuronsValue = new Text[net.neurons.Length][];

        for (x = 0; x < net.neurons.Length; x++)
        {
            neurons[x]      = new Image[net.neurons[x].Length];
            neuronsValue[x] = new Text[net.neurons[x].Length];

            for (y = 0; y < net.neurons[x].Length; y++)
            {
                if (net.neurons[x].Length % 2 == 0)
                {
                    yAdd = 1.0f;
                }
                else
                {
                    yAdd = 0;
                }

                if (y % 2 == 0)
                {
                    posY = y + yAdd;
                }
                else
                {
                    posY = -y - 1 + yAdd;
                }

                neuron = Instantiate(neuronPrefab, transform.position, Quaternion.identity, viewerGroup);

                neuron.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * decalX, posY * decalY);
                neurons[x][y]                                         = neuron.GetComponent<Image>();

                neuronsValue[x][y] = neuron.transform.GetChild(0).GetComponent<Text>();
            }
        }

        fitnesTransform = (Instantiate(fitnessPrefab, transform.position, Quaternion.identity, viewerGroup));

        fitnesTransform.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(decalX * net.neurons.Length * .5f + 300, 300);

        fitness = fitnesTransform.GetComponent<Text>();

        RefreshAxon();
    }

    public void Update()
    {
        for (x = 0; x < agent.net.neurons.Length; x++)
        {
            for (y = 0; y < agent.net.neurons[x].Length; y++)
            {
                neurons[x][y].color     = colorGradient.Evaluate((agent.net.neurons[x][y] + 1) * .5f);
                neuronsValue[x][y].text = agent.net.neurons[x][y].ToString("F2");
            }
        }

        fitness.text = agent.fitness.ToString("F1");
    }

    public void RefreshAxon()
    {
        for (x = 0; x < agent.net.axons.Length; x++)
        {
            for (y = 0; y < agent.net.axons[x].Length; y++)
            {
                for (z = 0; z < agent.net.axons[x][y].Length; z++)
                {
                    axons[x][y][z].color = colorGradient.Evaluate((agent.net.axons[x][y][z] + 1) * .5f);
                }
            }
        }
    }
}