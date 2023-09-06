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
import 'package:http/http.dart';

import '../services/Brand.dart';
import 'main.dart';

Image img = Image.asset('images/BrandNotFound.jpg');
Image img1 = Image.asset('images/snake.jpg');

class brandsPage extends StatefulWidget {
  String bearerToken;
  brandsPage({super.key, required this.bearerToken});
  @override
  _brandsState createState() => _brandsState();
}

class _brandsState extends State<brandsPage> {
  List<Brand> brands = [];
  List<Brand> displayedList = [];

  void getData() async {
    var url = Uri.parse('$urll/Brands');
    Response response = await get(url,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonList = json.decode(response.body);

    brands = jsonList.map((json) => toBrand(json)).toList();
    for (int i = 0; i < brands.length; i++) {
      displayedList.add(brands[i]);
    }
    setState(() {});
  }

  Brand toBrand(Map<String, dynamic> map) {
    Brand brand = Brand(map['id'], map['name'], map['image']);
    return brand;
  }

  @override
  void initState() {
    getData();
    super.initState();
  }

  void updateList(String value) {
    setState(() {
      displayedList = brands
          .where((element) =>
              element.name.toLowerCase().contains(value.toLowerCase()))
          .toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        //resizeToAvoidBottomInset: false,
        appBar: AppBar(
          toolbarHeight: 70,
//backgroundColor: Color(0xFFABB0C7),
//backgroundColor: Color(0xFFBBBFD4),//truuuue
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: const Text("All Brands"),
          titleTextStyle: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 22.0,
              color: Color(0xFF242424),
              fontFamily: 'Roboto Condensed'),
        ),
        body:
             Container(
          // height: 600,
          padding: const EdgeInsets.only(bottom: 30, top: 20),
          decoration: const BoxDecoration(
              gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [
//truuue
/* Color(0xFFABB0CC),
Color(0xFFDCD7E4),
Color(0xFFE6DADF),
Color(0xFFD5CED1),*/
                Color(0xFFA0CED5),
                Color(0xFFA1CADC),
                Color(0xFFA8C4DC),
                Color(0xFFC8D5DD),
                Color(0xFFD5CAD7),
                Color(0xFFD0CCD7),
                Color(0xFFDCC9CF),
              ])),
          child: Center(
            child: ListView(
              children: [
                displayedList.isNotEmpty?
                Row(children: [
                  Container(
                    width: 387,
                    margin: const EdgeInsets.symmetric(horizontal: 10),
                    child: TextField(
                      onChanged: (value) => updateList(value),
                      decoration: InputDecoration(
                          prefixIcon: const Icon(
                            Icons.search,
                            size: 25,
                          ),
                          prefixIconColor: Colors.teal[700],
                          hintText: 'Search by brand\'s name',
                          hintStyle: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 20,
                              fontWeight: FontWeight.bold),
                          //icon: Icon(Icons.search),
                          filled: true,
                          fillColor: Colors.white54,
                          border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(8.0),
                              borderSide: BorderSide.none)),
                      style:  TextStyle(
                          color: Colors.teal[700],
                          fontSize: 22,
                          fontWeight: FontWeight.bold,
                          fontStyle: FontStyle.normal
                      ),
                    ),
                  ),
                ])
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
                        child:Text("There are no brands yet",
                          style: TextStyle(
                            color: Colors.teal[700],
                            fontSize: 20,
                          ),
                          textAlign: TextAlign.center,
                        )
                    )]),
                for (int i = 0; i < displayedList.length; i++)
                  Row(children: [
                    Container(
                      height: 80,
                      width: 365,
                      margin: const EdgeInsets.symmetric(
                          vertical: 20, horizontal: 20),
                      padding: const EdgeInsets.only(right: 10),
                      decoration: BoxDecoration(
                        color: Colors.white60,
                        borderRadius: BorderRadius.circular(35),
                      ),
                      child: Row(children: [
                        Row(
                          children: [
                            Container(
                                width: 100,
                                height: 500,
                                padding: const EdgeInsets.only(right: 10),
                                child: Container(
                                    margin: const EdgeInsets.only(
                                        left: 0, right: 10.0),
                                    decoration: BoxDecoration(
                                      color: Colors.white,
                                      borderRadius: const BorderRadius.only(
                                          topLeft: Radius.circular(42),
                                          bottomLeft: Radius.circular(42)),
                                      /*border: Border.all(
                                          color: Colors.black87, width: 2.0),
                                      shape: BoxShape.circle),*/
                                      image: DecorationImage(
                                          image: NetworkImage(displayedList[i]
                                              .getImage(
                                                  displayedList[i].image)),
                                          fit: BoxFit.contain,
                                          opacity: 1),
                                    )))
                          ],
                        ),
                        TextButton(
                          onPressed: () {
                            Brand item = Brand(
                                brands[i].id, brands[i].name, brands[i].image);
                            item.selectBrand(context);
                           // Navigator.pushNamed(context, MaterialPageRoute(builder: (context)=>));

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
          ),
        ));
  }

  @override
  State<StatefulWidget> createState() {
// TODO: implement createState
    throw UnimplementedError();
  }
}
