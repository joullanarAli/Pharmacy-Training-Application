import 'dart:ui';

import 'package:flutter/material.dart';

class Choice{
  int id;
  int questionId;
  String choiceText;
  MaterialColor? choiceColor;
  bool score;
  //VoidCallback choiceTap;
  Choice(this.id,this.questionId,this.choiceText,this.score,this.choiceColor);
}