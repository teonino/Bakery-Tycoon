using System.Collections.Generic;

public class Dialogue {
    public string npcSpeech;
    public List<Answer> answers; 

    public Dialogue() {
        this.answers = new List<Answer>();
    }   
}

public class Answer {
    public string answerText;
    public int relation;
    public Dialogue nextDialogue;

    public Answer(string answerText, int relation, Dialogue nextDialogue) {
        this.answerText = answerText;
        this.relation = relation;
        this.nextDialogue = nextDialogue;
    }
}