using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using KModkit;

public class ExamTimeScript : MonoBehaviour
{
    Texture Question;
    WWW QuestionGrabber;
    public TextMesh BufferText;
    Question questionText;
    public KMBombModule Module;
    QuestionGenerator Generator;
    public GameObject QuestionObject;
    public Material QuestionMat;
    public GameObject Shader;

    bool Activated = false, choosing = true;

    Difficulties Difficulty;
    Categories Category;

    private bool ModuleSolved = false;

    private static int _moduleIdCounter = 1;
    private int _moduleId;

    void Start()
    {
        Shader.SetActive(false);
        QuestionObject.SetActive(false);
        _moduleId = _moduleIdCounter++;
        Module.OnActivate += Activate;
    }


    void Activate()
    {
        Activated = true;
        StartCoroutine(Prep());
    }

    void Update()
    {

    }


    IEnumerator Prep()
    {
        BufferText.color = new Color(255f, 255f, 255f, a: 255f);
        float i = 0f;
        while (choosing)
        {
            Difficulty = (Difficulties)Random.Range(0, 3);
            Difficulties OldDiff = Difficulty;
            while (OldDiff == Difficulty) Difficulty = (Difficulties)Random.Range(0, 3);
            Category = (Categories)Random.Range(0, 4);
            Categories OldCat = Category;
            while (OldCat == Category) Category = (Categories)Random.Range(0, 4);
            BufferText.text = "Difficulty:\n" + Difficulty + "\n\n\nCategory:\n" + Category;
            yield return new WaitForSeconds(0.1f);
            i += 0.1f;
            if (i >= 3f) choosing = false;
        }
        yield return new WaitForSeconds(1f);
        BufferText.color = new Color(255f, 255f, 255f, a: 0f);
        Debug.LogFormat("Chosen category is {0}, chosen difficulty is {1}. Generating an appropriate question.", Category, Difficulty);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Buffer());
    }

    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(0.1f);
        BufferText.color = new Color(255f, 255f, 255f, a: 255f);
        BufferText.text = "Generating question";
        yield return new WaitForSeconds(2f);
        BufferText.color = new Color(255f, 255f, 255f, a: 0f);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(GenerateQuestion());
    }

    IEnumerator GenerateQuestion()
    {
        Generator = new QuestionGenerator();
        Generator.EasyAlgebra();
        questionText = Generator.ChooseQuestion(Categories.Algebra, Difficulties.Easy, Generator.bank);
        QuestionGrabber = new WWW(@"https://latex.codecogs.com/png.image?\inline&space;\huge&space;\dpi{750}" + questionText.Text);
        Debug.Log(QuestionGrabber.url);
        yield return QuestionGrabber;
        Question = QuestionGrabber.texture;
        if (Question != null)
        {
            QuestionObject.SetActive(true);
            QuestionMat.mainTexture = Question;
            QuestionObject.GetComponent<Transform>().localScale = new Vector3(Question.width * 0.00007f * QuestionObject.GetComponent<Transform>().localScale.x / 0.15f, 0.01f, Question.height * 0.00007f * QuestionObject.GetComponent<Transform>().localScale.z / 0.15f);
            Shader.SetActive(true);
            Shader.GetComponent<Transform>().localScale = new Vector3(QuestionObject.GetComponent<Transform>().localScale.x, 0.01f, QuestionObject.GetComponent<Transform>().localScale.z);
        }
    }
}