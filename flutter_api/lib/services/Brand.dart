import 'dart:convert';
import 'dart:io';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_api/Screens/brands.dart';
import 'package:http/http.dart';

import '../Screens/main.dart';
import '../Screens/brandDrugs.dart';

class Brand{
  int id;
  String name;
  String image;
  Brand(this.id,this.name,this.image);

  String getImage(String imageName){
    if(imageName!=null) {
      return '$urll/Images/Brands/$imageName';
    } else {
      return '$urll/Images/Brands/BrandNotFound.jpg';
    }
  }
  void selectBrand(BuildContext context){
    Navigator.of(context).pushNamed('/brand-drugs',arguments: {
      'id': id,
      'name': name,
      'image':image,
    });
  }
}