using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using TMPro;

public class CipherController : MonoBehaviour
{
    public TMP_Text Ciphered_Text;
    public int shiftAmount = 1;  // Iloœæ miejsc do przesuniêcia

    void Start()
    {
        Ciphered_Text = GameObject.FindWithTag("cipher_text").GetComponent<TextMeshPro>();
    }

    private void Update()
    {

        // SprawdŸ, czy nast¹pi³o klikniêcie na kontrolerze VR
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText;

            Debug.Log("Encrypted Text: " + encryptedText);
        }
    }

    private string Encrypt(string text, int shift)
    {
        string result = "";
        foreach (char letter in text)
        {
            if (char.IsLetter(letter))
            {
                char encryptedChar = (char)(((letter - 'A' + shift) % 26) + 'A');
                result += encryptedChar;
            }
            else
            {
                result += letter;
            }
        }
        return result;
    }
}

