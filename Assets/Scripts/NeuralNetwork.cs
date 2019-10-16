using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]

public class NeuralNetwork 
{

    public int[] layers;
    public float[][] neurons;
    public float[][][] axons;

    int x;
    int y;
    int z;

    public NeuralNetwork()
    {

    }


    public NeuralNetwork(int[]  _layers)
    {
        layers = new int[_layers.Length];

        for (x = 0; x < _layers.Length; x++)
        {
            layers[x] = _layers[x];
        }

        InitNeurons();
        InitAxons();

    }

    void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();

        for (x = 0; x < layers.Length; x++)
        {
           
            neuronsList.Add(new float[layers[x]]);
        }

        neurons = neuronsList.ToArray();

    }

    void InitAxons()
    {
        //on crée une liste à deux dimensions
        List<float[][]> axonsList = new List<float[][]>();

        //on parcourt tout les neurons dans le layer
        for (x = 1; x < layers.Length; x++)
        {
            List<float[]> layeAxonsList = new List<float[]>();

            int neuronsInPreviousLayer = layers[x - 1];

            for (y = 0; y < layers[x]; y++)
            {
                float[] neuronAxons = new float[neuronsInPreviousLayer];

                //on parcourt tout les axons dans le neuron
                for (int z = 0; z < neuronsInPreviousLayer; z++)
                {
                    neuronAxons[z] = UnityEngine.Random.Range(-1, 1f);
                }

                layeAxonsList.Add(neuronAxons);
            }

            axonsList.Add(layeAxonsList.ToArray());
        }

        axons = axonsList.ToArray();
    }





    public void CopyNet(NeuralNetwork netToCopy)
    {

        for (x = 0; x < netToCopy.axons.Length; x++)
        {
            for (y  = 0; y  < netToCopy.axons[x].Length; y++)
            {
                for (z = 0; z < netToCopy.axons[x][y].Length; z++)
                {
                    axons[x][y][z] = netToCopy.axons[x][y][z];
                }
            }
        }

    }





    float value;

    public void FeedForward(float[] inputs)
    {
        neurons[0] = inputs;

        for (x = 1; x <layers.Length; x++)
        {
            for (y = 0;y < layers[x]; y++)
            {
                value = 0;

                for (z= 0; z < layers[x - 1]; z++)
                {
                    value += neurons[x - 1][z] * axons[x - 1][y][z];
                }

                neurons[x][y] = (float)Math.Tanh(value);
            }
        }
    }

    float RandomNumber;

    public void Mutate (float probability)
    {
        for (x = 0; x < axons.Length; x++)
        {
            for (y = 0; y < axons[x].Length; y++)
            {
                for (z = 0; z < axons[x][y].Length; z++)
                {
                    value = axons[x][y][z];

                    RandomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (RandomNumber < 0.06f * probability)
                    {
                        value = UnityEngine.Random.Range(-1f, 1f);
                    }
                    else if (RandomNumber < 0.07f * probability)
                    {
                        value *= -1f;
                    }
                    else if (RandomNumber < 0.5f * probability)
                    {
                        value += 0.1f * UnityEngine.Random.Range(-1f, 1f);
                    }
                    else if (RandomNumber < 0.75f * probability)
                    {
                        value *= 1f + UnityEngine.Random.Range(0f, 1f);
                    }
                    else if (RandomNumber < 1f * probability)
                    {
                        value *= UnityEngine.Random.Range(-1, 1);
                    }



                    axons[x][y][z] = value;

                }
            }
        }
    }





    /*float ProbaTry = 0;

    public void Mutation(float proba)
    {
        for (int x = 0; x < axons.Length - 1; x++)
        {
            for (int y = 0; y < axons[x].Length; y++)
            {
                for (z = 0; z < axons[x][y].Length; z++)
                {
                    if (UnityEngine.Random.Range(0, 1) <= proba)
                    {
                        axons[x][y][z] = UnityEngine.Random.Range(-1, 1);
                    }
                }

            }
        }
    }*/



}
