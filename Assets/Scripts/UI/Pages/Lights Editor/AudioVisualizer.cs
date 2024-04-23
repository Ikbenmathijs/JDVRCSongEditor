using UnityEngine;


// Original code from JDVRC, very little modifications made
// I wrote this a loooong time ago (and with limitations of U# at the time in mind, which I don't really have here) and it's very messy, apologies
// some parts are copied from a tutorial, and I basically built on top of that, which is why there's also very inconsistent naming conventions... yaaaaay
// keep in mind I was like 15 when I wrote this lol
public class AudioVisualiser : MonoBehaviour
{
    private Material[] panels = new Material[64];
    public Color defaultColor;
    //public Transform[] bars = new Transform[16];
    public AudioSource audioInput;
    public FFTWindow fftWindow;
    public int smoothness = 10;
    public Color[] colorList;
    private float[] SpectrumData = new float[64];
    private float[] lerpValues = new float[64];
    private Color[] panelColors = new Color[64];
    private float[] decaySpeed = new float[64];
    private float[] lastSeconds = new float[60];
    
    private float waitTime;
    public int panelUpdateRate = 5;
    private float timer = 0f;
    private int atIndex = 0;
    public int BeatLoops = 4;
    public float defaultDecaySpeed = 5f;

    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;

    private float m_previousAudioValue;
    private float m_audioValue;
    private float m_timer;

    public bool m_isBeat;
    public bool disableLights = false;
    private bool clearedLights = false;
    public float volumeMultiplier = 1f;

    public bool beatSolidColors = false;
    public bool intervalSolidColors = false;
    public Color fillColor;
    public bool filled = false;
    public bool disableInterval = false;
    public bool disableBeat = false;
    
    public static AudioVisualiser instance;
    
    public AudioVisualiser()
    {
        instance = this;
    }

    private void Start()
    {
        int child = 0;
        for (int i = 0; i < panels.Length; i++)
        {
            if (i % 8 == 0 && i != 0)
            {
                child++;
            }
            panels[i] = GetComponent<Transform>().GetChild(child).GetChild(i % 8).GetComponent<MeshRenderer>().materials[0];
        }
        for (int i = 0; i < panelColors.Length; i++)
        {
            panelColors[i] = defaultColor;
        }
        for (int i = 0; i < lastSeconds.Length; i++)
        {
            lastSeconds[i] = 0f;
        }
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].color = defaultColor;
            panels[i].SetColor("_EmissionColor", defaultColor);
        }
        waitTime = 1f / panelUpdateRate;
    }
    private void Update()
    {
        if (!disableLights)
        {
            if (!filled)
            {
                clearedLights = false;
                audioInput.GetSpectrumData(SpectrumData, 0, fftWindow);
                SpectrumData[0] = SpectrumData[0] * volumeMultiplier;
                if (SpectrumData[0] > 1f)
                {
                    SpectrumData[0] = 1f;
                }
                lastSeconds[atIndex] = SpectrumData[0];
                if (atIndex < lastSeconds.Length - 1)
                {
                    atIndex++;
                }
                else
                {
                    atIndex = 0;
                }
                /*
                for (int i = 0; i < 16; i += 1)
                {
                    int m = 10;
                    if (i == 0)
                    {
                        m = 5;
                    }
                    bars[i].localScale = Vector3.Lerp(bars[i].localScale, new Vector3(bars[i].localScale.x, m * SpectrumData[i] * m, bars[i].localScale.z), Time.deltaTime * smoothness);
                }
                */
                // fade effect
                for (int i = 0; i < panels.Length; i++)
                {
                    if (panels[i].color != defaultColor)
                    {
                        lerpValues[i] = lerpValues[i] - decaySpeed[i] * Time.deltaTime;
                        if (lerpValues[i] < 0f || lerpValues[i] > 1f)
                        {
                            lerpValues[i] = 0f;
                        }
                        Color color = Color.Lerp(defaultColor, panelColors[i], lerpValues[i]);
                        panels[i].color = color;
                        panels[i].SetColor("_EmissionColor", color);
                    }
                }
                // scan thingy effect
                timer += Time.deltaTime;
                if (!disableInterval && timer >= waitTime)
                {
                    timer = 0f;
                    int[] chosenPanels = new int[8];
                    for (int i = 0; i < chosenPanels.Length; i++)
                    {
                        chosenPanels[i] = Random.Range(0, panels.Length);
                    }
                    Color color;
                    if (intervalSolidColors)
                    {
                        color = Color.Lerp(defaultColor, getColor(), 1);
                    }
                    else
                    {
                        color = Color.Lerp(defaultColor, getColor(), getHighest(lastSeconds));
                    }
                    for (int i = 0; i < chosenPanels.Length; i++)
                    {
                        int chosenPanel = chosenPanels[i];
                        if (!intervalSolidColors && ColorDistance(color, defaultColor) > ColorDistance(panels[chosenPanel].color, defaultColor) || intervalSolidColors && panels[chosenPanel].color == defaultColor && getHighest(lastSeconds) != 0)
                        {
                            panelColors[chosenPanel] = color;
                            decaySpeed[chosenPanel] = defaultDecaySpeed;
                            lerpValues[chosenPanel] = 1f;
                            panels[chosenPanel].color = color;
                            panels[chosenPanel].SetColor("_EmissionColor", color);
                        }
                    }
                }


                // totally not stolen™️
                // update audio value
                m_previousAudioValue = m_audioValue;
                m_audioValue = SpectrumData[0] * 100;

                // if audio value went below the bias during this frame
                if (m_previousAudioValue > bias &&
                    m_audioValue <= bias)
                {
                    // if minimum beat interval is reached
                    if (m_timer > timeStep)
                        OnBeat();
                }

                // if audio value went above the bias during this frame
                if (m_previousAudioValue <= bias &&
                    m_audioValue > bias)
                {
                    // if minimum beat interval is reached
                    if (m_timer > timeStep)
                        OnBeat();
                }

                m_timer += Time.deltaTime;
            } else
            {
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].color = fillColor;
                    panels[i].SetColor("_EmissionColor", fillColor);
                }
            }
        } else
        {
            if (!clearedLights)
            {
                clearedLights = true;
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].color = defaultColor;
                    panels[i].SetColor("_EmissionColor", defaultColor);
                }
            }
        }

    }
    // beat effect
    private void OnBeat()
    {
        if (!disableBeat)
        {
            m_isBeat = true;
            for (int i = 0; i < BeatLoops; i++)
            {
                int chosenPanel = Random.Range(0, panelColors.Length);
                Color color;
                if (beatSolidColors)
                {
                    color = Color.Lerp(defaultColor, getColor(), 1);
                }
                else
                {
                    color = Color.Lerp(defaultColor, getColor(), SpectrumData[0]);
                }
                if (!beatSolidColors && (ColorDistance(color, defaultColor) > ColorDistance(panels[chosenPanel].color, defaultColor)) || beatSolidColors && panels[chosenPanel].color == defaultColor)
                {
                    panelColors[chosenPanel] = color;
                    decaySpeed[chosenPanel] = defaultDecaySpeed;
                    lerpValues[chosenPanel] = 1f;
                    panels[chosenPanel].color = color;
                    panels[chosenPanel].SetColor("_EmissionColor", color);
                }
            }
        }
    }
    private float average(float[] array)
    {
        float total = 0f;
        for (int i = 0; i < array.Length; i++)
        {
            total += array[i];
        }
        return total / array.Length;
    }
    private float getHighest(float[] array)
    {
        float highest = 0f;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > highest)
            {
                highest = array[i];
            }
        }
        return highest;
    }
    
    // the original formula has a square root, but since it's only used for checking if one's closer or not it's not needed here so is left out for performance
    // not that the rest of the code is optimized :')
    int ColorDistance(Color a, Color z)
    {
        
        int r = (int)(a.r*256) - (int)(z.r*256),
            g = (int)(a.g * 256) - (int)(z.g * 256),
            b = (int)(a.b * 256 )- (int)(z.b * 256);
        return (r * r + g * g + b * b);
    }
    Color getColor()
    {
        return colorList[Random.Range(0, colorList.Length)];
    }
}