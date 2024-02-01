using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;

public class Cipher_button : BaseButton
{
    public TMP_Text Ciphered_Text;
    public TMP_Text Key_Deciphered;
    public FiltersRiddleButtonType buttonType;

    public enum FiltersRiddleButtonType
    {
        Plus,
        Minus
    }

    // Start is called before the first frame update
    void Start()
    {
        Ciphered_Text = GameObject.FindWithTag("cipher_text").GetComponent<TextMeshPro>();
        Key_Deciphered = GameObject.FindWithTag("decipher_key").GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    protected override void ButtonAction()
    {
        if (buttonType == FiltersRiddleButtonType.Plus)
        {
            int shiftAmount = 1;  // Iloœæ miejsc do przesuniêcia

            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText;
        }
        else if (buttonType == FiltersRiddleButtonType.Minus)
        {
            int shiftAmount = -1;  // Iloœæ miejsc do przesuniêcia

            // Pobierz aktualny tekst z obiektu TextMesh
            string currentText = Ciphered_Text.text;

            // Zaszyfruj tekst i zaktualizuj obiekt TextMesh
            string encryptedText = Encrypt(currentText, shiftAmount);
            Ciphered_Text.text = encryptedText; // Set the object inactive to hide it
        }
        else
        {
            Debug.LogWarning("Unknown button!");
        }

        Ciphered_Text = GameObject.FindWithTag("cipher_text").GetComponent<TextMeshPro>();
        Key_Deciphered = GameObject.FindWithTag("decipher_key").GetComponent<TextMeshPro>();
        if (Ciphered_Text.text == "O cztery znaki w prawo")
        {
            Key_Deciphered.text = "Rozwi¹zano zagadkê - liczba: 432";
            Key_Deciphered.color = new UnityEngine.Color(1, 0, 0, 1);
            //_solved = true;
        }
        else
        {
            Key_Deciphered.text = "Rozwi¹¿ zagadkê";
            Key_Deciphered.color = new UnityEngine.Color(0, 0, 0, 1);
            //_solved = false;
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
