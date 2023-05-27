using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LSystemGenerator : MonoBehaviour
{
    public Rule[] rules;
    public string rootSentence;
    [Range(0, 20)]
    public int iterationLimit = 1;

    public bool randomIgnoreRule = true;
    [Range(0, 1)]
    public float chanceToIgnorRule = 0.3f;

    private void Start()
    {
        Debug.Log(GenerateSentence());
    }
    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = rootSentence;
        }
        return GrowRecusive(word);
    }

    private string GrowRecusive(string word, int iterIndex = 0)
    {
        if (iterIndex >= iterationLimit)
        {
            return word;
        }

        StringBuilder newWord = new StringBuilder();

        foreach (var character in word)
        {
            newWord.Append(character);

            ProcessRules(newWord, character, iterIndex);

        }
        return newWord.ToString();
    }

    private void ProcessRules(StringBuilder newWord, char character, int iterIndex)
    {
        foreach (var rule in rules)
        {
            if (rule.letter == character.ToString())
            {
                if (randomIgnoreRule && iterIndex > 1)
                {
                    if(UnityEngine.Random.value < chanceToIgnorRule)
                    {
                        return;
                    }
                }
                newWord.Append(GrowRecusive(rule.GetResults(), iterIndex + 1));
            }
        }
    }

}
