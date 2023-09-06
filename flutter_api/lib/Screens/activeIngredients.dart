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
import '../services/ActiveIngredient.dart';
import 'main.dart';

//import 'package:overflow_view/overflow_view.dart';
//import 'package:image/image.dart';

Image img = Image.asset('images/pharmacyLogo.jpg');
Image img1 = Image.asset('images/snake.jpg');
List<Image> images = [];

class activeIngredientsPage extends StatefulWidget {
  String bearerToken;
  activeIngredientsPage({super.key, required this.bearerToken});
  _activeIngredientsState createState() => _activeIngredientsState();
}

class _activeIngredientsState extends State<activeIngredientsPage> {
  List<ActiveIngredient> activeIngredients = [];
  List<ActiveIngredient> displayedList = [];

  void getData() async {
    var url = Uri.parse('$urll/ActiveIngredients');
    Response response = await get(url,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonList = json.decode(response.body);
    activeIngredients = jsonList.map((json) => toActiveIngredient(json)).toList();
    for (int i = 0; i < activeIngredients.length; i++) {
      displayedList.add(activeIngredients[i]);
    }
    setState(() {});
  }

  ActiveIngredient toActiveIngredient(Map<String, dynamic> map) {
    ActiveIngredient activeIngredient = ActiveIngredient(map['id'],map['name']);
    return activeIngredient;
  }

  @override
  void initState() {
    getData();
    super.initState();
  }

  void updateList(String value) {
    setState(() {
      displayedList = activeIngredients
          .where((element) =>
          element.name.toLowerCase().contains(value.toLowerCase()))
          .toList();
    });
  }
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: const Text("All Active Ingredients"),
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
              child: ListView(
                children:[ Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    Row(children: [
                      displayedList.isNotEmpty?
                      Container(
                        width: 387,
                        margin: const EdgeInsets.symmetric(horizontal: 5),
                        child: TextField(
                          onChanged: (value) => updateList(value),
                          decoration: InputDecoration(
                              prefixIcon: const Icon(
                                Icons.search,
                                size: 25,
                              ),
                              prefixIconColor: Colors.teal[700],
                              hintText: 'Search by active ingredient\'s name',
                              hintStyle: TextStyle(
                                  color: Colors.teal[700],
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold),
                              filled: true,
                              fillColor: Colors.white54,
                              border: OutlineInputBorder(
                                  borderRadius: BorderRadius.circular(50.0),
                                  borderSide: BorderSide.none)),
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
                                  image: img1.image,
                                ),
                              ),
                            ),
                      Container(
                          margin: const EdgeInsets.symmetric(vertical: 10),
                          child:Text("There are no active ingredients yet",
                            style: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 25,
                            ),
                            textAlign: TextAlign.center,
                          )
                      )])]),
                    Column(
                      children: [
                       for (int i = 0; i < displayedList.length; i++)

                      Row(children: [
                        Container(
                          height: 80,
                          width: 365,
                          margin: const EdgeInsets.symmetric(
                              vertical: 20, horizontal: 20),
                          padding: const EdgeInsets.symmetric(
                              vertical: 0, horizontal: 10),
                          decoration: BoxDecoration(
                            color: Colors.white60,
                            borderRadius: BorderRadius.circular(35),
                          ),
                          child: Row(children: [
                            /*Row(
                              children: [
                                Container(
                                  width: 100,
                                  height: 80,
                                  padding: const EdgeInsets.all(10),
                                  child: Container(
                                      margin: const EdgeInsets.only(
                                          left: 10.0, right: 10.0),
                                      decoration: BoxDecoration(
                                          color: Colors.white,
                                          border: Border.all(
                                              color: Colors.black87, width: 2.0),
                                          shape: BoxShape.circle)),
                                )
                              ],
                            ),*/
                            TextButton(
                              onPressed: () {
                                ActiveIngredient activeIngredient =ActiveIngredient(
                                    displayedList[i].id, displayedList[i].name);
                                activeIngredient.selectActiveIngredient(context);
                              },
                              child: Text(
                                displayedList[i].name,
                                style: const TextStyle(
                                    fontWeight: FontWeight.bold,
                                    fontSize: 30.0,
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
                      ]),
             ] )));
  }

  @override
  State<StatefulWidget> createState() {
// TODO: implement createState
    throw UnimplementedError();
  }
}
