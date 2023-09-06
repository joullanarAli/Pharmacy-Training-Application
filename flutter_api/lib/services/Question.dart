class Question{
  int id;
  int courseId;
  String question;
  int correctAnswerMark;
  int noAnswerMark;
  int wrongAnswerMark;
  bool hasImage=false;
  Question(this.id,this.courseId,this.question,this.correctAnswerMark,this.noAnswerMark,this.wrongAnswerMark);
}