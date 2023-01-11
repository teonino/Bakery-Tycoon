using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorieButton : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Color selectedColor;

    private bool selected = false;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = background.color;
    }

    public void OnSelection() {
        if (selected)
            background.color = originalColor;
        else
            background.color = selectedColor;

        selected = !selected;
    }
}
