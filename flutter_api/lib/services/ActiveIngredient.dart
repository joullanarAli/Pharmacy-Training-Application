import 'dart:convert';
import 'dart:io';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';

class ActiveIngredient{
  int id;
  String name;
  ActiveIngredient(this.id,this.name);
  void selectActiveIngredient(BuildContext context) {
    Navigator.of(context).pushNamed('/activeIngredients-drugs', arguments: {
      'id': id,
      'name': name,
    });
  }

}