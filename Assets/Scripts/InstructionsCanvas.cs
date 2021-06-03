using UnityEngine;

public class InstructionsCanvas : MonoBehaviour
{
    // This class operates the functioning of the Instruction Canvas, switching the screens when necessary

    // The number of indexes
    private int siblingIndex = 6;
    
    // Counter for screen change
    private int siblingInt = 1;

    // Reference to MapSceneManager script
    private MapSceneManager mapSceneManagerScript;

    // References to RectTransforms of instruction screens
    public RectTransform instructionOne;
    public RectTransform instructionTwo;
    public RectTransform instructionThree;
    public RectTransform instructionFour;
    public RectTransform instructionFive;
    public RectTransform instructionSix;

    // Start is called before the first frame update
    void Start()
    {
        mapSceneManagerScript = GameObject.Find("MapSceneManager").GetComponent<MapSceneManager>();

        siblingInt = 1;

        instructionSix.SetSiblingIndex(1);
        instructionFive.SetSiblingIndex(2);
        instructionFour.SetSiblingIndex(3);
        instructionThree.SetSiblingIndex(4);
        instructionTwo.SetSiblingIndex(5);
        instructionOne.SetSiblingIndex(6);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            siblingInt++;
        }

        if (siblingInt == 1)
        {
            instructionOne.SetSiblingIndex(siblingIndex);
        }
        if (siblingInt == 2)
        {
            instructionTwo.SetSiblingIndex(siblingIndex);
        }
        if (siblingInt == 3)
        {
            instructionThree.SetSiblingIndex(siblingIndex);
        }
        if (siblingInt == 4)
        {
            instructionFour.SetSiblingIndex(siblingIndex);
        }
        if (siblingInt == 5)
        {
            instructionFive.SetSiblingIndex(siblingIndex);
        }
        if (siblingInt == 6)
        {
            instructionSix.SetSiblingIndex(siblingIndex);

        }
        if (siblingInt == 7)
        {
            mapSceneManagerScript.isFirstTime = false;

            gameObject.SetActive(false);
            instructionOne.SetSiblingIndex(5);
            instructionTwo.SetSiblingIndex(4);
            instructionThree.SetSiblingIndex(3);
            instructionTwo.SetSiblingIndex(2);
            instructionOne.SetSiblingIndex(1);
            siblingInt = 1;
        }
    }
}
