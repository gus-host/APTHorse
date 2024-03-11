using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory_Script : MonoBehaviour
{
    private int tokenCount = 0;

    public void AddTokens(int amount)
    {
        tokenCount += amount;
    }

    public void RemoveTokens(int amount)
    {
        tokenCount -= amount;
        if (tokenCount < 0)
        {
            tokenCount = 0;
        }
    }

    public int GetTokenCount()
    {
        return tokenCount;
    }
}

