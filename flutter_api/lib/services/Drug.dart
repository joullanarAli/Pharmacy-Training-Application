import 'dart:convert';
import 'dart:io';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';

import '../Screens/main.dart';
import 'DrugForms.dart';

class Drug{
  int id;
  String englishName;
  String arabicName;
  String description;
  String sideEffects;
  int brandId;
  int categoryId;
  String image;
  String brandName;
  String categoryName;
  Drug(this.id,this.brandId,this.categoryId,this.englishName,this.arabicName,this.description,
      this.sideEffects,this.image,this.brandName,this.categoryName);

  String getImage(String imageName){
    if(imageName!=null) {
      return '$urll/Images/Drugs/$imageName';
    } else {
      return '$urll/Images/Drugs/DrugNotFound.jpg';
    }
  }
  void selectDrug(BuildContext context,String brandName,String categoryName){
    Navigator.of(context).pushNamed('/drug',arguments: {
      'id': id,
      'englishName':englishName,
      'arabicName':arabicName,
      'description':description,
      'sideEffects':sideEffects,
      'brandId':brandId,
      'categoryId':categoryId,
      'image':image,
      'brandName': brandName,
      'categoryName':categoryName,
    });
  }
}