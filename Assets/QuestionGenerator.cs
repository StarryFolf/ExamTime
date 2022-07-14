using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestionGenerator
{
    internal QuestionBank bank = new QuestionBank();
    internal Question ChooseQuestion(Categories c, Difficulties d, QuestionBank bank)
    {
        return bank[new QuestionMetadata(c, d)].PickRandom();
    }

    public void EasyAlgebra()
    {
        QuestionMetadata metadata = new QuestionMetadata(Categories.Algebra, Difficulties.Easy);
        string question;
        int a, b, c, d, e;

        question = @"\begin{gathered}\text{Given&space;the&space;equation:}\\x^3&plus;x^2&plus;x&plus;1=0\\\text{Calculate&space;the&space;roots.}\end{gathered}";
        bank.Add(new Question(question),metadata);
    }
}
