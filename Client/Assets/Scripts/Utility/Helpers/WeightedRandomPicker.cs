using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickerElement
{
    public string name;
    public double weigted;

    public RandomPickerElement(string name, double weigted)
    {
        this.name = name;
        this.weigted = weigted;
    }
}

public class WeightedRandomPicker : MonoBehaviour
{
    public static string GetRandomPicker(List<RandomPickerElement> list)
    {
        System.Random randomInstance = new System.Random();

        Dictionary<string, double> itemWeightDict = new Dictionary<string, double>();
        double sumOfWeights = 0; // ��� ����ġ ��

        foreach (var item in list)
        {
            if (itemWeightDict.ContainsKey(item.name))
            {
                Debug.LogError("�ߺ� ������ ����");
                continue;
            }

            sumOfWeights += item.weigted;
            itemWeightDict.Add(item.name, item.weigted);
        }

        double randomValue = randomInstance.NextDouble(); // 0.0 ~ 1.0 ���� �� ����
        randomValue *= sumOfWeights;

        double current = 0.0;
        foreach(var pair in itemWeightDict)
        {
            current += pair.Value;

            if (randomValue <= current)
                return pair.Key;
        }

        Debug.LogWarning("���� �̱⿡ ����");
        return null;
    }
}
