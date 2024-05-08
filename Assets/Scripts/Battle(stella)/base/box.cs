using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Transform TL;
    [SerializeField] private Transform TM;
    [SerializeField] private Transform TR;
    [SerializeField] private Transform ML;
    [SerializeField] private Transform MR;
    [SerializeField] private Transform BL;
    [SerializeField] private Transform BM;
    [SerializeField] private Transform BR;
    [SerializeField] private Transform Background;
    public float height;
    public float width;
    public float speed;
    float currentHeight = 2;
    float currentWidth = 2;
    bool doneH = false;
    bool doneW = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWidth != width)
        {
            doneW = false;
        }
        if (currentHeight != height)
        {
            doneH = false;
        }

        if (!doneH || !doneW)
        {
            if (currentHeight > (height - 0.1) && currentHeight < (height + 0.1))
            {
                doneH = true;
                currentHeight = height;
            }
            else if (currentHeight > height)
            {
                currentHeight -= Time.deltaTime * speed;
            }
            else
            {
                currentHeight += Time.deltaTime * speed;
            }


            if (currentWidth > (width - 0.1) && currentWidth < (width + 0.1))
            {
                doneW = true;
                currentWidth = width;
            }
            else if (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * speed;
            }
            else
            {
                currentWidth += Time.deltaTime * speed;
            }

            TL.localPosition = new Vector3(-(currentWidth / 2), (currentHeight / 2), 0);

            TM.localPosition = new Vector3(0, (currentHeight / 2), 0);
            TM.localScale = new Vector3(currentWidth-1.16667f, 1, 1);

            TR.localPosition = new Vector3((currentWidth / 2), (currentHeight / 2), 0);

            ML.localPosition = new Vector3(-(currentWidth / 2), 0, 0);
            ML.localScale = new Vector3(1, currentHeight - 1.16667f, 1);

            MR.localPosition = new Vector3((currentWidth / 2), 0, 0);
            MR.localScale = new Vector3(1, currentHeight - 1.16667f, 1);

            BL.localPosition = new Vector3(-(currentWidth / 2), -(currentHeight / 2), 0);

            BM.localPosition = new Vector3(0, -(currentHeight / 2), 0);
            BM.localScale = new Vector3(currentWidth - 1.16667f, 1, 1);

            BR.localPosition = new Vector3((currentWidth / 2), -(currentHeight / 2), 0);

            Background.localScale = new Vector3(currentWidth, currentHeight, 1);
        }
    }
}
