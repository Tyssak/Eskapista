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
    public TMP_Text Key_Deciphered;

    void Start()
    {
        Ciphered_Text = GameObject.FindWithTag("cipher_text").GetComponent<TextMeshPro>();
        Key_Deciphered = GameObject.FindWithTag("decipher_key").GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        // SprawdŸ, czy nast¹pi³o klikniêcie na kontrolerze VR
        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            int shiftAmount = 1;  // Iloœæ miejsc do przesuniêcia

            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText;

            //Debug.Log("Encrypted Text: " + encryptedText);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            int shiftAmount = -1;  // Iloœæ miejsc do przesuniêcia

            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText;

            //Debug.Log("Encrypted Text: " + encryptedText);
        }

        if (Ciphered_Text.text == "O cztery znaki w prawo")
        {
            Key_Deciphered.text = "Rozwi¹zano zagadkê - liczba: 4";
            Key_Deciphered.color = new UnityEngine.Color(1, 0, 0, 1);
        }
        else
        {
            Key_Deciphered.text = "Rozwi¹¿ zagadkê";
            Key_Deciphered.color = new UnityEngine.Color(0, 0, 0, 1);
        }
    }

    private string Encrypt(string text, int shift)
    {
        string result = "";

        foreach (char letter in text)
        {
            if (char.IsLetter(letter))
            {
                char baseChar = char.IsUpper(letter) ? 'A' : 'a';
                char encryptedChar = (char)(((letter - baseChar + shift) % 26 + 26) % 26 + baseChar);
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

