using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;

public class TriviaQuestions : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class question
    {
        public string Actual_Question;
        public string Correct_Answer;
        public string Wrong_Answer1;
        public string Wrong_Answer2;
        public string Wrong_Answer3;
    }

    [System.Serializable]
    public class QuestionsList
    {
        public question[] Questions;
    }

    public long highscore = 0;
    public string highscoreString = "";


    private string highscoreFolder; //= "Assets/UI/TextFiles/HighScore.txt";
    private string QuestionFolder;

    public QuestionsList TriviaList = new QuestionsList();
    // Start is called before the first frame update
    void Start()
    {
        highscoreFolder = Application.persistentDataPath + "/HighScore.txt";
        Debug.Log(highscoreFolder);
        QuestionFolder = Application.persistentDataPath + "/Questions.txt";

        try
        {
            WebClient wc = new WebClient();
            var json = wc.DownloadString("https://mo-amin.github.io/hostJsonPrac/Questions.txt");
            Debug.Log(json);
            TriviaList = JsonUtility.FromJson<QuestionsList>(json);
            Debug.Log(Application.persistentDataPath);

            WriteQuestionTextFile(json.ToString());
        }
        catch
        {
            TriviaList = JsonUtility.FromJson<QuestionsList>(File.ReadAllText(QuestionFolder));
            //TriviaList = JsonUtility.FromJson<QuestionsList>(textJSON.text);
        }
        
        Debug.Log(TriviaList.Questions[TriviaList.Questions.Length - 1].Actual_Question);

        readTextFile();
        //WriteTextFile("128", highscoreFolder);
        readTextFile();

    }

    public void readTextFile()
    {
        StreamReader read = null;
        try
        {
            read = new StreamReader(highscoreFolder);
        }
        catch
        {
            WriteHighScoreTextFile("0");
            return;
        }
        while (!read.EndOfStream)
        {
            highscoreString = read.ReadLine();
            highscore = long.Parse(highscoreString);
            Debug.Log(highscoreString);
        }

        read.Close();

    }

    public void WriteHighScoreTextFile(string information)
    {
        File.WriteAllText(highscoreFolder, information);
        /*
        StreamWriter writetext = new StreamWriter(filepath);
        writetext.WriteLine(information);
        */
    }

    public void WriteQuestionTextFile(string information)
    {
        File.WriteAllText(QuestionFolder, information);
        /*
        StreamWriter writetext = new StreamWriter(filepath);
        writetext.WriteLine(information);
        */
    }
}
