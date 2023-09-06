//import 'dart:js';

//import 'dart:html';

import 'dart:collection';
import 'dart:convert';
import 'dart:io';

//import 'dart:html';
//import 'dart:js_util';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'dart:typed_data';
import 'package:flutter/services.dart';
import 'package:http/http.dart';

import 'package:http/http.dart' as http;
import 'package:http_parser/http_parser.dart';
import 'main.dart';
import '../services/Category.dart';


Image img = Image.asset('images/pharmacyLogo.jpg');
Image img1 = Image.asset('images/snake.jpg');
List<Image> images = [];
class categoriesPage extends StatefulWidget {
  String bearerToken;
  categoriesPage({super.key, required this.bearerToken});
  @override
  _categoriesState createState() => _categoriesState();
}

class _categoriesState extends State<categoriesPage> {
  List<CategoryModel> categories = [];
  List<CategoryModel> displayedList = [];


  void getData() async {
    var url = Uri.parse(urll+'/Categories');
    Response response = await get(url,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonList = json.decode(response.body);
    categories = jsonList.map((json) => toCategory(json)).toList();
    for (int i = 0; i < categories.length; i++) {
      displayedList.add(categories[i]);
    }
    for (int j = 0; j < categories.length; j++) {
      setState(() {});
    }
  }

    CategoryModel toCategory(Map<String, dynamic> map) {
      CategoryModel category = CategoryModel(map['id'],map['name'],map['image']);
      return category;
    }

    @override
    void initState() {
      getData();
      super.initState();
    }
    void updateList(String value) {
      setState(() {
        displayedList = categories.where((element) =>
            element.name.toLowerCase().contains(value.toLowerCase())).toList();
      });
    }
    @override
    Widget build(BuildContext context) {
      return Scaffold(
        resizeToAvoidBottomInset: false,
          appBar: AppBar(
            toolbarHeight: 70,
            backgroundColor: const Color(0xFFA0CED5),
            centerTitle: true,
            iconTheme: const IconThemeData(
              color: Colors.black,
            ),
            title: const Text("All Categories"),
            titleTextStyle: const TextStyle(
                fontWeight: FontWeight.bold,
                fontSize: 22.0,
                color: Color(0xFF242424),
                fontFamily: 'Roboto Condensed'),
          ),
          body: Container(

                padding: const EdgeInsets.only(bottom: 30, top: 20),
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

                  child:
                      ListView(
               //   child: Column(
                    children: [

                      Row(
                          children: [
                            displayedList.isNotEmpty?
                            Container(
                            width: 387,
                            margin:const  EdgeInsets.only(left: 10,right: 10,top:10,bottom:20),
                            child: TextField(
                              onChanged: (value) => updateList(value),
                              decoration: InputDecoration(
                                  prefixIcon: Icon(Icons.search, size: 25,),
                                  prefixIconColor: Colors.teal[700],
                                  hintText: 'Search by category\'s name',
                                  hintStyle: TextStyle(
                                      color: Colors.teal[700],
                                      fontSize: 20,
                                      fontWeight: FontWeight.bold
                                  ),
                                  //icon: Icon(Icons.search),
                                  filled: true,
                                  fillColor: Colors.white54,
                                  border: OutlineInputBorder(
                                      borderRadius: BorderRadius.circular(8.0),
                                      borderSide: BorderSide.none

                                  )
                              ),
                              style: TextStyle(
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
                                      child:Text("There are no categories yet",
                                        style: TextStyle(
                                          color: Colors.teal[700],
                                          fontSize: 20,
                                        ),
                                        textAlign: TextAlign.center,
                                      )
                                  )]),                             ]),
                        GridView.count(
                          physics: ScrollPhysics(),
                          scrollDirection: Axis.vertical,
                              crossAxisCount: 2,
                              mainAxisSpacing: 10,
                        shrinkWrap: true,
                        children:[
                          for (int i = 0; i < displayedList.length; i++)

                         Column(children: [
                          Container(
                            height: 200,
                            width: 200,
                            margin: const EdgeInsets.symmetric(
                                vertical: 0, horizontal: 10),
                            padding: const EdgeInsets.only(
                                top: 20, left: 5),
                            decoration: BoxDecoration(
                              color: Colors.white60,
                              borderRadius: BorderRadius.circular(35),
                            ),
                            child: Column(children: [
                              //Column(children: [
                                Container(
                                //  width:50,
                                  margin: const EdgeInsets.only(left: 0, top: 0,right:0),
                                  child:TextButton(
                                    onPressed: () {
                                      CategoryModel category = CategoryModel(
                                          categories[i].id, categories[i].name, categories[i].image);
                                      category.selectCategory(context);
                                    },
                                    child: Text(
                                      displayedList[i].name,
                                      style: const TextStyle(
                                          fontWeight: FontWeight.bold,
                                          fontSize: 30.0,
                                          color: Color(0xFF242424),
                                          fontFamily: 'Roboto Condensed'),
                                      textAlign: TextAlign.left,
                                    ),
                                  ),),
                                Flexible(
                                  // flex: 1,
                                    child:
                                    Container(
                                        width: 100,
                                        height: 400,
                                        margin: const EdgeInsets.only(left: 80, top:36,bottom: 0),
                                        padding: const EdgeInsets.all(0),
                                        child:
                                        Container(
                                            margin: const EdgeInsets.only(right: 0,bottom: 0,left:0),
                                            decoration: BoxDecoration(
                                              //  color: Colors.white60,
                                              borderRadius: const BorderRadius.only(bottomRight: Radius.circular(30)),
                                              image: DecorationImage(
                                                //  colorFilter:const  ColorFilter.mode(Colors.white, BlendMode.lighten),
                                                  image: NetworkImage(displayedList[i].getImage(displayedList[i].image)),
                                                  fit: BoxFit.contain,
                                                  opacity: 1
                                              ),),


                                          //for image (foreground)


                                          /*  child:ShaderMask(
                                              shaderCallback: (Rect bounds) {
                                                return  const LinearGradient(
                                                  colors: [Colors.white54, Colors.transparent],
                                                  stops: [
                                                    0.0,
                                                    1,
                                                  ],
                                                ).createShader(bounds);
                                              },
                                              blendMode: BlendMode.srcATop,
                                              child: Image.network(displayedList[i].getImage(displayedList[i].image)),
                                            )*/

                                        ) ))],
                              ),


                          )
                        ]),
                          ],
                  ),

                ]),
              ));
    }

    @override
    State<StatefulWidget> createState() {
// TODO: implement createState
      throw UnimplementedError();
    }
  }

  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    throw UnimplementedError();
  }