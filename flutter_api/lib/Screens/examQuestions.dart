//import 'dart:js';

//import 'dart:html';

import 'dart:convert';
import 'dart:math';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_api/Screens/choices.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Category.dart';
import 'package:flutter_api/services/Choice.dart';
import 'package:flutter_api/services/DrugForms.dart';
import 'package:flutter_api/services/Exam.dart';
import 'package:flutter_api/services/Form.dart';
import 'package:flutter_api/services/Image.dart';
import 'package:flutter_api/services/Question.dart';
import 'package:http/http.dart';
import 'dart:async';

import 'main.dart';
import '../services/Drug.dart';

class examQuestionsPage extends StatefulWidget {
  int id;
  String name;
  String bearerToken;

  examQuestionsPage({ required this.id,required this.name,required this.bearerToken,super.key});
  @override
  _examQuestionsState createState() => _examQuestionsState();
}

class _examQuestionsState extends State<examQuestionsPage> {
  List<Question> questions = [];
  List<Choice> choices = [];
  List<Question> displayedList = [];
  bool choiceWasSelected = false;
  bool endOfQuiz = false;
  bool correctChoiceSelected = false;
  List<Icon> _scoreTracker = [];
  int _totalScore = 0;
  int questionIndex=0;
 // int questionCounter=1;
  List<Choice> displayedChoices=[];
  List<Brand> brands=[];
  List<ImageModel> images=[];
  int remainingTime = 60; // in seconds
  Timer? timer;
  void startTimer() {
    timer = Timer.periodic(Duration(seconds: 1), (Timer timer) {
      if (remainingTime > 0) {
        setState(() {
          remainingTime--;
        });
      } else {
        timer.cancel(); // Cancel the timer when time is up
        // You can add code here to handle what happens when the time is up
      }
    });
  }
  Future<void> getBrand() async {
    var urlBrand = Uri.parse('$urll/Brands');
    Response responseBrand = await get(urlBrand,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    print(responseBrand.body);
    final List<dynamic> jsonListBrand= json.decode(responseBrand.body);
    brands = jsonListBrand.map((json) => toBrand(json)).toList();
  }
  Brand toBrand(Map<String, dynamic> map) {
    Brand brand = Brand(map['id'],map['name'],map['image']);
    return brand;
  }
  void getData() async {
    var urlQuestionExams = Uri.parse('$urll/Questions/GetQuestionsByExam?examId=${widget.id}');
    Response responseQuestionExams = await get(urlQuestionExams,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListQuestionExams= json.decode(responseQuestionExams.body);
    questions = jsonListQuestionExams.map((json) => toQuestion(json)).toList();
    var urlChoices = Uri.parse('$urll/Choices');
    Response responseChoices = await get(urlChoices,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListChoices= json.decode(responseChoices.body);
    choices = jsonListChoices.map((json) => toChoice(json)).toList();
    var urlImages = Uri.parse('$urll/Images');
    Response responseImages = await get(urlImages,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListImages= json.decode(responseImages.body);
    images = jsonListImages.map((json) => toImage(json)).toList();
    questions=randomizeQuestions(questions);
    remainingTime=questions.length*15;
    displayedChoices= choices.where((element) => element.questionId==questions[questionIndex].id).toList();
    setState(() {
      for(int i =0;i<questions.length;i++) {
        for(int j=0;j<images.length;j++){
          if(images[j].questionId==questions[i].id){
            questions[i].hasImage=true;
          }
        }
        displayedList.add(questions[i]);
      }
    });
  }
  List<Question> randomizeQuestions(List<Question> questions){
    List<Question> shuffledQuestions = List.from(questions);

    // Shuffle the list using the Fisher-Yates algorithm
    final random = Random();
    for (int i = shuffledQuestions.length - 1; i > 0; i--) {
      int j = random.nextInt(i + 1);
      Question temp = shuffledQuestions[i];
      shuffledQuestions[i] = shuffledQuestions[j];
      shuffledQuestions[j] = temp;
    }
    return shuffledQuestions;
  }
  int getExamMark(){
    int examMark=0;
    for(int i=0;i<questions.length;i++){
      //  List<Choice> Choices= choices.where((element) => element.questionId==questions[i].id).toList();
      for(int j=0;j<displayedChoices.length;j++){
        if(choices[j].score){
          examMark+=questions[i].correctAnswerMark;
        }
      }
    }
    return examMark;
  }
  List<ImageModel> getQuestionImages(int questionId){
    List<ImageModel> questionImages=[];
    for(int i=0;i<images.length;i++){
      if(images[i].questionId==questionId){
        questionImages.add(images[i]);
      }
    }
    return questionImages;
  }
  Question toQuestion(Map<String, dynamic> map) {
    Question question =Question(map['id'],map['courseId'],map['questionText'],map['correctAnswerMark'],
        map['noAnswerMark'],map['wrongAnswerMark']);
    return question;
  }
  ImageModel toImage(Map<String, dynamic> map){
    ImageModel image=ImageModel(map['id'],map['questionId'],map['path']);
    return image;
  }
  Choice toChoice(Map<String, dynamic> map) {
    MaterialColor? choiceColor =choiceWasSelected ? map['score'] ?
    Colors.green
        : Colors.red
        : null;
    Choice choice = Choice(map['id'],map['questionId'],map['choiceText'],map['score'],choiceColor,);
    return choice;
  }
  void _questionAnswered(bool choiceScore,int questionCounter) {
    setState(() {
      //to prevent user from answer the same question many times
      choiceWasSelected = true;
      if (choiceScore) {
        _totalScore+=questions[questionCounter].correctAnswerMark;
        correctChoiceSelected = true;
      }else{
        _totalScore+=questions[questionIndex].wrongAnswerMark;
      }
      _scoreTracker.add(
        choiceScore
            ? const Icon(
          Icons.check_circle,
          color: Colors.green,
        )
            : const Icon(
          Icons.clear,
          color: Colors.red,
        ),
      );
      //when the quiz ends
      if (questionCounter + 1 == questions.length) {
        endOfQuiz = true;
      }else{
       // questionIndex++;
      }
     // _nextQuestion();}
    });
  }
  void _nextQuestion() {
    setState(() {
      questionIndex++;
      if(questionIndex<questions.length) {
        displayedChoices = choices.where((element) => element.questionId ==
            questions[questionIndex].id).toList();
      }
      if(questionIndex==questions.length-1){
        endOfQuiz=true;
      }
      choiceWasSelected = false;
      correctChoiceSelected = false;
    });
    if (questionIndex >= questions.length) {
      _resetQuiz();
    }
  }
  void _resetQuiz() {
    setState(() {
      questionIndex = 0;
      choiceWasSelected = false;
      correctChoiceSelected = false;
      displayedChoices.clear();
      questions=randomizeQuestions(questions);
      displayedChoices= choices.where((element) => element.questionId==questions[questionIndex].id).toList();
      _totalScore = 0;
      _scoreTracker = [];
      endOfQuiz = false;
      remainingTime=questions.length*15;
    });
  }
  @override
  void initState() {
    getData();
    startTimer();
    super.initState();
  }
/*  void updateList(String value) {
    displayedList =questions
        .where((element) => element.ques.toLowerCase().contains(value.toLowerCase())
        || element.categoryName.toLowerCase().contains(value.toLowerCase())
        || element.brandName.toLowerCase().contains(value.toLowerCase())).toList();
    setState(() {

    });
  }*/
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: Text(widget.name),
          titleTextStyle: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 22.0,
              color: Color(0xFF242424),
              fontFamily: 'Roboto Condensed'),
        ),
        body: remainingTime>0?
        Container(
            padding: const EdgeInsets.only(bottom: 30, top: 2),
            decoration: const BoxDecoration(
                gradient: LinearGradient(
                    begin: Alignment.topCenter,
                    end: Alignment.bottomCenter,
                    colors: [
                      Color(0xFFA0CED5),
                      Color(0xFFA1CADC),
                      Color(0xFFA8C4DC),
                      Color(0xFFC8D5DD),
                      Color(0xFFD5CAD7),
                      Color(0xFFD0CCD7),
                      Color(0xFFDCC9CF),
                    ])),
            child: ListView(
                children:<Widget> [
                  Row(
                    children: [
                      if (_scoreTracker.isEmpty)
                        const SizedBox(
                          height: 25.0,
                        ),
                      if (_scoreTracker.isNotEmpty) ..._scoreTracker
                    ],
                  ),
                  remainingTime>0?
                  Container(
                      child: Text('Time left: $remainingTime seconds',
                          style: const TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                            color: Colors.teal
                          ) ,
                    )
                  )
                  : Container(
                      child: const Text('Time\'s out',
                        style: TextStyle(
                            fontSize: 20,
                            fontWeight: FontWeight.bold,
                            color: Colors.redAccent
                        ) ,
                      )
                  ),
             //     for(int i=0;i<questions.length;i++)
                  questions.isNotEmpty && remainingTime>0  && getQuestionImages(questions[questionIndex].id).isNotEmpty ?
                  Column(
                  children:[
                    Container(
                    width: double.infinity,
                    height: 130.0,
                    margin: const EdgeInsets.only(bottom: 10.0, left: 30.0, right: 30.0),
                    padding: const EdgeInsets.symmetric(horizontal: 50.0, vertical: 20.0),
                    decoration: BoxDecoration(
                      color: Colors.white60,
                      borderRadius: BorderRadius.circular(10.0),
                      ),
                    child: Center(
                      child: Text(questions.isNotEmpty? questions[questionIndex].question:'',
                        textAlign: TextAlign.center,
                        style: const TextStyle(
                          fontSize: 20.0,
                          color: Colors.black87,
                          fontWeight: FontWeight.bold,
                        ),

                      ),
                    ),
                  ),
                    for(int i=0;i<getQuestionImages(questions[questionIndex].id).length;i++)
                    Container(
                      width: double.infinity,
                      height: 130.0,
                      margin: const EdgeInsets.only(bottom: 10.0, left: 30.0, right: 30.0),
                      padding: const EdgeInsets.symmetric(horizontal: 50.0, vertical: 20.0),
                      decoration: BoxDecoration(
                          color: Colors.white60,
                          borderRadius: BorderRadius.circular(10.0),
                          image: DecorationImage(
                            fit: BoxFit.fill,
                              image: NetworkImage(getQuestionImages(questions[questionIndex].id)[i].
                              getImage(getQuestionImages(questions[questionIndex].id)[i].image))
                          )
                      ),
                    )
                  ])
                  :Container(
                    width: double.infinity,
                    height: 130.0,
                    margin: const EdgeInsets.only(bottom: 10.0, left: 30.0, right: 30.0),
                    padding: const EdgeInsets.symmetric(horizontal: 50.0, vertical: 20.0),
                    decoration: BoxDecoration(
                      color: Colors.white60,
                      borderRadius: BorderRadius.circular(10.0),
                    ),
                    child: Center(
                      child: Text(questions.isNotEmpty? questions[questionIndex].question:'',
                        textAlign: TextAlign.center,
                        style: const TextStyle(
                          fontSize: 20.0,
                          color: Colors.black87,
                          fontWeight: FontWeight.bold,
                  ),
                ),
              ),
            ),
                  for(int j=0;j<displayedChoices.length;j++)
                    /* Container(
                      padding: const EdgeInsets.all(0),
                      margin: const EdgeInsets.symmetric(vertical: 5.0, horizontal: 30.0),
                      width: double.infinity,
                      decoration: BoxDecoration(
                        color: displayedChoices[j].choiceColor,
                        border: Border.all(color: Colors.blue),
                        borderRadius: BorderRadius.circular(20.0),
                      ),
                      child:*//*RawMaterialButton(
                    //    key: Key(displayedChoices[j].toString()),
                       // onPressed: (valur) =>onClick(int.parse());
                        *//*{
                          if (choiceWasSelected) {
                            return;
                          }
                          print(displayedChoices[j].choiceText);
                          print(displayedChoices[j].score);
                          _questionAnswered(,questionIndex);
                        }*//*
                        onPressed: () { null; },
                        child: Text(
                          displayedChoices[j].choiceText,
                          style: const TextStyle(
                            fontSize: 15.0,
                          ),
                        ),
                      ),),*/
                    choicesPage(choiceText: displayedChoices[j].choiceText,
                      choiceColor: displayedChoices[j].choiceColor,
                      choiceTap: () {
                        if (choiceWasSelected) {
                          return;
                        }
                        _questionAnswered(displayedChoices[j].score,questionIndex);
                      },),
                const SizedBox(height: 20.0),
                  ElevatedButton(
                    style: ElevatedButton.styleFrom(
                      minimumSize: const Size(double.infinity, 40.0),
                    ),
                    onPressed: () {
                      !endOfQuiz?
                      _nextQuestion()
                      :_resetQuiz();
                     // questionIndex++;
                    },
                    child: Text(endOfQuiz ? 'Restart Quiz' : 'Next Question'),
                  ),
                  Container(
                  //  margin: const EdgeInsets.symmetric(horizontal: 150),
                    padding: const EdgeInsets.all(20.0),
                    child: Text('Your mark is: '
                        '${_totalScore>0?_totalScore.toString():'0'}/${getExamMark()}',
                     // '${_totalScore.toString()}/${questions.length-1}',
                      style: const TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                    ),
                  ),
                  if (choiceWasSelected && !endOfQuiz)
                    Container(
                      height: 100,
                      width: double.infinity,
                      color: correctChoiceSelected ? Colors.green : Colors.red,
                      child: Center(
                        child: Text(
                          correctChoiceSelected
                              ? 'Well done, you got it right!'
                              : 'Wrong :/',
                          style: const TextStyle(
                            fontSize: 20.0,
                            fontWeight: FontWeight.bold,
                            color: Colors.white,
                          ),
                        ),
                      ),
                    ),
                  if (endOfQuiz && choiceWasSelected)
                    Column(children:[
                      Container(
                        height: 100,
                        width: double.infinity,
                        color: Colors.black,
                        child: Center(
                          child: Text(
                            _totalScore >= getExamMark()/2
                                ? 'Congratulations! Your final score is: $_totalScore'
                                :_totalScore>0
                                ? 'Your final score is:${_totalScore}. Better luck next time!'
                                : 'Your final score is: 0. Better luck next time!'
                            ,
                            style: TextStyle(
                              fontSize: 20.0,
                              fontWeight: FontWeight.bold,
                              color: _totalScore >= getExamMark()/2 ? Colors.green : Colors.red,
                            ),
                          ),
                        ),
                      ),
                      const SizedBox(height: 20.0),
                      ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          minimumSize: const Size(double.infinity, 40.0),
                        ),
                        onPressed: () async {
                          await getBrand();
                          Navigator.of(context).pushNamed(
                            '/home', arguments: {
                            'bearerToken': token,
                            'brands':brands
                          // questionIndex++;
                        });
                        },
                        child: const Text('Go back to home'),
                      ),
                    ]),
                          ]) )
            :Container(
              padding: const EdgeInsets.only(bottom: 30, top: 2,right: 30,left: 30),
              decoration: const BoxDecoration(
              gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [
              Color(0xFFA0CED5),
              Color(0xFFA1CADC),
              Color(0xFFA8C4DC),
              Color(0xFFC8D5DD),
              Color(0xFFD5CAD7),
              Color(0xFFD0CCD7),
              Color(0xFFDCC9CF),
              ])),
             child: Column(
                children: [
                      Container(
                      width: 250,
                      height: 250,
                      margin: const EdgeInsets.only(left: 70,right: 70,bottom: 10,top: 100),
                      decoration: BoxDecoration(
                        image: DecorationImage(
                          image: timeOutImage.image
                        )
                      )),
                      Container(
                          margin: const EdgeInsets.symmetric(horizontal: 50,vertical: 0),
                          child:const Text('Time\'s out',
                            style: TextStyle(
                              fontWeight: FontWeight.bold,
                              color: Colors.redAccent,
                              fontSize: 30
                      ),))
                  ])

        ));
  }

  @override
  State<StatefulWidget> createState() {
// TODO: implement createState
    throw UnimplementedError();
  }
  @override
  void dispose() {
    timer?.cancel(); // Cancel the timer when the screen is disposed
    super.dispose();
  }
  void onClick(int id) {
    for(int i=0;i<displayedChoices.length;i++){
      if(displayedChoices[i].id==id){
        _questionAnswered(displayedChoices[i].score,questionIndex);

      }
    }
  }
}



