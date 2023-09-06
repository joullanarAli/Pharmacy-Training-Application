/*import 'dart:html';
import 'dart:convert';
import 'dart:io';*/
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_api/Screens/brands.dart';
import 'package:http/http.dart';

import '../Screens/brandDrugs.dart';
import '../Screens/main.dart';
import 'package:flutter/src/widgets/framework.dart';

class Exam{
  int id;
  String name;
  Exam(this.id,this.name);

  void selectExam(BuildContext context) {
    Navigator.of(context).pushNamed('/exam-questions',arguments: {
      'id': id,
      'name': name,
    });
  }
}