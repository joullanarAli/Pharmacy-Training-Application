//import 'dart:js_interop';

import '../Screens/main.dart';

class ImageModel{
  int id;
  int questionId;
  String image;
  ImageModel(this.id,this.questionId,this.image);
  String getImage(String imageName){
    if(imageName!=null) {
      return '$urll/Images/Quiz Images/$imageName';
    } else {
      return '$urll/Images/Quiz Images/imageNotFound.jpg';
    }
  }
}