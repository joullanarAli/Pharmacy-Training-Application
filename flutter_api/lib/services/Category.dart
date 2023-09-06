import 'dart:convert';
import 'dart:io';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';

import '../Screens/main.dart';

class CategoryModel{
  int id;
  String name;
  String image;
  //String bearerToken;
  CategoryModel(this.id,this.name,this.image);
  String getImage(String imageName){
    if(imageName!=null)
      return urll+'/Images/Categories/'+imageName;
    else
      return urll+'/Images/Categories/CategoryNotFound.jpg';
  }
  void selectCategory(BuildContext context) {
    Navigator.of(context).pushNamed('/category-drugs', arguments: {
      'id': id,
      'name': name,
      'image': image,
    });
  }
}