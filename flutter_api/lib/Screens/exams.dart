//import 'dart:js';

//import 'dart:html';

import 'dart:collection';
import 'dart:convert';
import 'dart:io';

//import 'dart:html';
//import 'dart:js_util';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_api/services/Exam.dart';
import 'dart:typed_data';
import 'package:http/http.dart';

import 'main.dart';
import '../services/Brand.dart';

class examsPage extends StatefulWidget {
  String bearerToken;
  examsPage({super.key, required this.bearerToken});
  _examsState createState() => _examsState();
}

class _examsState extends State<examsPage> {
  List<Exam> exams = [];
  List<Exam> displayedList = [];

  void getData() async {
    var url = Uri.parse('$urll/Exams');
    Response response = await get(url,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonList = json.decode(response.body);

    exams = jsonList.map((json) => toExam(json)).toList();
    for (int i = 0; i < exams.length; i++) {
      displayedList.add(exams[i]);
    }
    setState(() {});
  }

  Exam toExam(Map<String, dynamic> map) {
    Exam exam = Exam(map['id'], map['name']);
    return exam;
  }

  @override
  void initState() {
    getData();
    super.initState();
  }

  void updateList(String value) {
    setState(() {
      displayedList = exams
          .where((element) =>
          element.name.toLowerCase().contains(value.toLowerCase()))
          .toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          toolbarHeight: 70,
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          /*leading: BackButton(
            color: Colors.black,
            onPressed: () {
              Navigator.pushNamed(context, '/home');
            },
          ),*/
          title: const Text("All Exams"),
          titleTextStyle: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 22.0,
              color: Color(0xFF242424),
              fontFamily: 'Roboto Condensed'),
        ),
        body:
        Container(
          // height: 600,
          padding: const EdgeInsets.only(bottom: 30, top: 20),
          decoration: const BoxDecoration(
              gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [
//truuue
/* Color(0xFFABB0CC),
Color(0xFFDCD7E4),
Color(0xFFE6DADF),
Color(0xFFD5CED1),*/
                    Color(0xFFA0CED5),
                    Color(0xFFA1CADC),
                    Color(0xFFA8C4DC),
                    Color(0xFFC8D5DD),
                    Color(0xFFD5CAD7),
                    Color(0xFFD0CCD7),
                    Color(0xFFDCC9CF),
                  ])),
          child: Center(
            child: ListView(
              children: [
                Row(children: [
                  displayedList.isNotEmpty?
                  Container(
                    width: 387,
                    margin: const EdgeInsets.symmetric(horizontal: 10),
                    child: TextField(
                      onChanged: (value) => updateList(value),
                      decoration: InputDecoration(
                          prefixIcon: const Icon(
                            Icons.search,
                            size: 25,
                          ),
                          prefixIconColor: Colors.teal[700],
                          hintText: 'Search by exam\'s name',
                          hintStyle: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 20,
                              fontWeight: FontWeight.bold),
                          //icon: Icon(Icons.search),
                          filled: true,
                          fillColor: Colors.white54,
                          border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(8.0),
                              borderSide: BorderSide.none)),
                      style:  TextStyle(
                          color: Colors.teal[700],
                          fontSize: 22,
                          fontWeight: FontWeight.bold,
                          fontStyle: FontStyle.normal
                      ),
                    ),
                  )
                      :Column(
                      children:[
                        Container(
                          margin: const EdgeInsets.symmetric(horizontal: 120,vertical: 20),
                          width: 150,
                          height: 150,
                          decoration: BoxDecoration(
                            image: DecorationImage(
                              image: notFoundImage.image,
                            ),
                          ),
                        ),
                        Container(
                            margin: const EdgeInsets.symmetric(vertical: 10),
                            child:Text('There are no exams yet',
                              style: TextStyle(
                                color: Colors.teal[700],
                                fontSize: 20,
                              ),
                              textAlign: TextAlign.center,
                            )
                        )]),]),
                for (int i = 0; i < displayedList.length; i++)
                  Row(children: [
                    Container(
                      alignment: Alignment.center,
                      height: 80,
                      width: 365,
                      margin: const EdgeInsets.symmetric(
                          vertical: 20, horizontal: 20),
                      padding: const EdgeInsets.symmetric(horizontal: 70),
                      decoration: BoxDecoration(
                        color: Colors.white60,
                        borderRadius: BorderRadius.circular(35),
                      ),
                      child: Row(children: [
                        TextButton(
                          onPressed: () {
                            Exam exam = Exam(
                                exams[i].id, exams[i].name);
                            exam.selectExam(context);
                            // Navigator.pushNamed(context, MaterialPageRoute(builder: (context)=>));

                          },
                          child: Text(
                            '${displayedList[i].name}',
                            style: const TextStyle(
                                fontWeight: FontWeight.bold,
                                fontSize: 17.0,
                                color: Color(0xFF242424),
                                fontFamily: 'Roboto Condensed'),
                            textAlign: TextAlign.center,
                          ),
                        )
                      ]),
                    )
                  ]),
              ],
            ),
          ),
        ));
  }

  @override
  State<StatefulWidget> createState() {
// TODO: implement createState
    throw UnimplementedError();
  }
}
