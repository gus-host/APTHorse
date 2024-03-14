using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField _staminaInput;
    [SerializeField] public Button _submitandPlay;
    [SerializeField] public TextMeshProUGUI _message;

    private void Awake()
    {
        _submitandPlay.onClick.AddListener(PlayGame);
    }

    private void PlayGame()
    {
        string input = _staminaInput.text;
        if (string.IsNullOrEmpty(input))
        {
            StartCoroutine(ShowPromt("Please enter in digits"));
            return;
        }
        int result;
        if (HasAlphabetsOrCharacters(input))
        {
            StartCoroutine(ShowPromt("Enter in digits"));

            return;
        }
        if (int.TryParse(input, out result))
        {
            Debug.Log("Converted value: " + result);
        }
        else
        {
            Debug.Log("Failed to convert the input to an integer");
        }

        if (result <= 100)
        {
            LoadGame(result);
        }
        else
        {
            StartCoroutine(ShowPromt("Enter value less than 100"));
        }
    }
    private bool HasAlphabetsOrCharacters(string input)
    {
        foreach (char c in input)
        {
            if (char.IsLetter(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c))
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator ShowPromt(string message)
    {
        _message.gameObject.SetActive(true);
        _message.text = message;
        yield return new WaitForSeconds(3f);
        _message.gameObject.SetActive(false);

    }

    private static void LoadGame(int result)
    {
        PlayerPrefs.SetInt("Stamina", result);
        SceneManager.instance.LoadScene("Week-1");
    }
}
