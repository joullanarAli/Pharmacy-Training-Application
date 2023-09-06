//import 'dart:js';

//import 'dart:html';

import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Category.dart';
import 'package:http/http.dart';

import '../services/Drug.dart';
import 'main.dart';
class brandDrugsPage extends StatefulWidget {
  int id;
  String name;
  String image;
  String bearerToken;
  brandDrugsPage({ required this.id,required this.name,required this.image, super.key, required this.bearerToken});
  @override
  _brandDrugsState createState() => _brandDrugsState();
}

class _brandDrugsState extends State<brandDrugsPage> {
List<Drug> brandDrugs = [];
List<CategoryModel> categories = [];
List<Drug> _brandDrugs=[];
List<Drug> displayedList = [];

void getData() async {
  var urlCategory = Uri.parse('$urll/Categories');
  Response responseCategory = await get(urlCategory,headers:{
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Authorization': 'Bearer $token'});
  final List<dynamic> jsonListCategory= json.decode(responseCategory.body);
  categories = jsonListCategory.map((json) => toCategory(json)).toList();
  var url = Uri.parse('$urll/Drugs');
  Response response = await get(url,headers:{
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Authorization': 'Bearer $token'});
  final List<dynamic> jsonList = json.decode(response.body);
  brandDrugs = jsonList.map((json) => toBrandDrug(json)).toList();
  setState(() {
    _brandDrugs=brandDrugs.where((element) =>
    element.brandId==widget.id
    ).toList();
    for(int i =0;i<_brandDrugs.length;i++) {
      displayedList.add(_brandDrugs[i]);
    }
  });
}
String getCategoryName(int categoryId){
  CategoryModel category=categories.firstWhere((element) => element.id==categoryId);
  return category.name;
}
Drug toBrandDrug(Map<String, dynamic> map) {
  String categoryName=getCategoryName(map['categoryId']);

  Drug drug = Drug(
      map['id'],
      map['brandId'],
      map['categoryId'],
      map['englishName'],
      map['arabicName'],
      map['description'],
      map['sideEffects'],
      map['image'],
      widget.name,
      categoryName);
  return drug;
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
  displayedList = brandDrugs
      .where((element) => element.englishName.toLowerCase().contains(value.toLowerCase())
        || element.categoryName.toLowerCase().contains(value.toLowerCase())).toList();
  setState(() {

  });
}
  @override
  Widget build(BuildContext context) {

    int brandId = widget.id;
    String brandName = widget.name;
    String brandImage = widget.image;
    final route = ModalRoute.of(context);
    if (route != null) {
      final routesArgs = route.settings.arguments as Map<String, dynamic>;
      brandId = routesArgs['id'];
      brandName = routesArgs['name'];
      brandImage = routesArgs['image'];
    }

    Brand brand = Brand(brandId, brandName, brandImage);

    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: Text(brandName),
          titleTextStyle: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 22.0,
              color: Color(0xFF242424),
              fontFamily: 'Roboto Condensed'),
        ),
        body: Container(
          padding: const EdgeInsets.only(bottom: 30, top: 2),
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
              children: [
                /*Row(children: [
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
                      hintText: 'Search by drug\'s name',
                      hintStyle: TextStyle(
                          color: Colors.teal[700],
                          fontSize: 20,
                          fontWeight: FontWeight.bold),
                      filled: true,
                      fillColor: Colors.white54,
                      border: OutlineInputBorder(
                          borderRadius: BorderRadius.circular(8.0),
                          borderSide: BorderSide.none)),
                  style: const TextStyle(
                    color: Colors.black38,
                  ),
                ),
              ),
                ]),*/

                Row(
              children: [
                Container(
                  margin: const EdgeInsets.only(bottom: 10),
                  height: 200,
                  width: 409,
                  decoration: BoxDecoration(
                    image: DecorationImage(
                      //alignment: Alignment.center,
                        image: NetworkImage(brand.getImage(widget.image)),
                        fit: BoxFit.fill),
                  ),
                )
              ],
                ),
                Row(
                    children: [
                      displayedList.isNotEmpty?
                      Container(
                        width: 387,
                        margin:const  EdgeInsets.only(left: 10,right: 10,top:10,bottom:20),
                        child: TextField(
                          onChanged: (value) => updateList(value),
                          decoration: InputDecoration(
                              prefixIcon: const Icon(Icons.search, size: 25,),
                              prefixIconColor: Colors.teal[700],
                              hintText: 'Search by drug/category',
                              hintStyle: TextStyle(
                                  color: Colors.teal[700],
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold
                              ),
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
                      ): Column(
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
                                child:Text("There are no drugs \n"
                                    "that made by this brands yet",
                                  style: TextStyle(
                                    color: Colors.teal[700],
                                    fontSize: 20,
                                  ),
                                  textAlign: TextAlign.center,
                                )
                            )]),                    ]),
      GridView.count(
        physics: const ScrollPhysics(),
        scrollDirection: Axis.vertical,
        crossAxisCount: 2,
        mainAxisSpacing: 10,
        shrinkWrap: true,
        children:[
                for (int i = 0; i < displayedList.length; i++)
              Column(children: [
                Expanded(child:
                Container(
                  height: 210,
                  width: 200,
                  margin: const EdgeInsets.symmetric(
                      vertical: 0, horizontal: 10),
                  padding: const EdgeInsets.symmetric(
                      vertical: 0, horizontal: 0),
                  decoration: BoxDecoration(
                    color: Colors.white60,
                    borderRadius: BorderRadius.circular(35),
                  ),
                  child: Column(children: [
                        Container(
                            width: 200,
                            height: 130,
                            padding: const EdgeInsets.all(0),
                            child:
                            Container(
                                margin: const EdgeInsets.only(
                                    left: 0, right: 0.0),
                                decoration: BoxDecoration(
                                  color: Colors.white,
                                  borderRadius: const BorderRadius.only(
                                    topLeft: Radius.circular(39),
                                    topRight: Radius.circular(39)
                                  ),
                                  image: DecorationImage(
                                      image: NetworkImage(displayedList[i]
                                          .getImage(displayedList[i].image)),
                                      fit: BoxFit.contain,
                                      opacity: 1),
                                )       ))
                        ,
                    Container(
                      margin: const EdgeInsets.only(right: 0,top:10),
                        child: TextButton(
                      onPressed: () {
                        Drug drug = Drug(
                            displayedList[i].id, displayedList[i].brandId,displayedList[i].categoryId,
                            displayedList[i].englishName,displayedList[i].arabicName,displayedList[i].description,
                            displayedList[i].sideEffects, displayedList[i].image,widget.name,displayedList[i].categoryName);
                        drug.selectDrug(context,widget.name,displayedList[i].categoryName);
                      },
                      child: Text(
                        displayedList[i].englishName,
                        style: const TextStyle(
                            fontWeight: FontWeight.bold,
                            fontSize: 20.0,
                            color: Color(0xFF242424),
                            fontFamily: 'Roboto Condensed'),
                       // textAlign: TextAlign.left
                      ),
                    )
                    )]),

    ))]),
              ],
            ),
    ]) ));
  }

  @override
  State<StatefulWidget> createState() {
// TODO: implement createState
    throw UnimplementedError();
  }
}



