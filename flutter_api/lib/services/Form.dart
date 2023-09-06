
import 'package:flutter/cupertino.dart';

import '../Screens/main.dart';

class FormModel{
  int id;
  String name;
  String image;
  FormModel(this.id,this.name,this.image);

  String getImage(String imageName){
    if(imageName!=null) {
      return '$urll/Images/Forms/$imageName';
    } else {
      return '$urll/Images/Forms/FormNotFound.jpg';
    }
  }
  void selectForm(BuildContext context) {
    Navigator.of(context).pushNamed('/form-drugs', arguments: {
      'id': id,
      'name': name,
      'image': image,
    });
  }
}