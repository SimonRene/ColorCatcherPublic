using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEdge : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        if(colorPicker == null || colorPicker.Count == 0)
        {

            colorPicker = new Dictionary<EdgeColor, Color>();

            colorPicker.Add(EdgeColor.RED, Color.red);
            colorPicker.Add(EdgeColor.BLUE, Color.blue);
            colorPicker.Add(EdgeColor.GREEN, Color.green);
            colorPicker.Add(EdgeColor.YELLOW, Color.yellow);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EdgeColor EdgeColor
    {
        get
        {
            return m_edgeColor;
        }
        set
        {
            m_edgeColor = value;

            if(!colorPicker.ContainsKey(value))
            {
                Debug.LogError("PlayerEdge: EdgeColor (" + value + ") is not in the dictionary!");
                return;
            }

            if(m_spriteRenderer == null)
            {
                m_spriteRenderer = GetComponent<SpriteRenderer>();
            }

            // update the color of the sprite renderer to match the new edge color, using the "colorPicker" dictionary
            m_spriteRenderer.color = colorPicker[m_edgeColor];
        }
    }

    // dictionary that stores the correct color for each "EdgeColor" enum value
    public static Dictionary<EdgeColor, Color> colorPicker;

    private EdgeColor m_edgeColor;

    private SpriteRenderer m_spriteRenderer;
}
