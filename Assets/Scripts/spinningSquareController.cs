using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinningSquareController : MonoBehaviour, IObstacle
{

    public spinningSquare spinningSquare2Prefab;

    public int numSquares;

    private GameObject[] squares;

    private int[] colorPosition;

    public bool activeColorChange;
    
    public float animationSpeed
    {
        get;
        set;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeColorChange = false;
        animationSpeed = 1f;

        squares = new GameObject[numSquares];
        colorPosition = new int[numSquares*4];
        for (int i = 0; i < (numSquares * 4); i++)
        {
            colorPosition[i] = i;
        }
        for (int x = 0; x < numSquares; x++)
        {
            

            var sqr = Instantiate<spinningSquare>(spinningSquare2Prefab);
            sqr.transform.parent = this.gameObject.transform;

            sqr.transform.localPosition = new Vector3(
                0f,
                0f,
                0.6f * x);

            squares[x] = sqr.gameObject;

            squares[x].transform.localEulerAngles = new Vector3(
                squares[x].transform.localEulerAngles.x,
                squares[x].transform.localEulerAngles.y,
                squares[x].transform.localEulerAngles.z +(x*3f));

        
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int x = 0; x < numSquares; x++)
        {
            
            squares[x].transform.localEulerAngles = new Vector3(
                squares[x].transform.localEulerAngles.x,
                squares[x].transform.localEulerAngles.y,
                squares[x].transform.localEulerAngles.z +1f);

                

            //Get the Renderer component from the new cube
            //var cubeRenderer = squares[x].gameObject.GetComponent<Renderer>();

            //Call SetColor using the shader property name "_Color" and setting the color to red
            //cubeRenderer.material.SetColor("_Color", Color.red);
        
        }
        if (activeColorChange) 
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            //foreach (var r in renderers)
            
            for (int x = 0; x < (numSquares*4); x++)
            {
                

                Renderer r = renderers[x];
                //Call SetColor using the shader property name "_Color" and setting the color to red
                //float lerp = Mathf.PingPong(Time.time, 1f) / 1f;
                
                //r.material.color = Color.HSVToRGB(colorPosition[x]/256f, 1f, 1f); 
                r.material.color = Color.HSVToRGB(0.5f, 0.5f,0.5f); // color blue color
                //r.material.color = getColorforIndex(colorPosition[x]);

                colorPosition[x] = (colorPosition[x] +1 ) % 255;
                //r.material.SetColor("_Color", Color.red);
                // Do something with the renderer here...
                //r.enabled = false; // like disable it for example. 
            }
        }
    }
    
    Color getColorforIndex(int index)
    {
        //HSBColor.ToColor(new HSBColor( Mathf.PingPong(Time.time * Speed, 1), 1, 1)))

        int cutoff = Mathf.FloorToInt(numSquares/7);

        if (index < cutoff)
        {
            return Color.Lerp(Color.red, new Color(255f,0f,255), index/(numSquares*4));
        } else if (index < cutoff*2) {
            return Color.Lerp(new Color(255f,0f,255),Color.blue, index/(numSquares*4));
        }else if (index < cutoff*3) {
            return Color.Lerp(Color.blue,new Color(0f,255f,255), index/(numSquares*4));
        }else if (index < cutoff*4) {
            return Color.Lerp(new Color(0f,255f,255),Color.green, index/(numSquares*4));
        }else if (index < cutoff*5) {
            return Color.Lerp(Color.green,Color.yellow, index/(numSquares*4));
        } else {
            return Color.Lerp(Color.yellow,Color.red, index/(numSquares*4));
        }
    } 
}
