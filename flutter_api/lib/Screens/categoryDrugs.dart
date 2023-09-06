//import 'dart:js';

//import 'dart:html';

import 'dart:convert';
//import 'dart:html';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Category.dart';
import 'package:http/http.dart';

import 'main.dart';
import '../services/Drug.dart';
class categoryDrugsPage extends StatefulWidget {
  int id;
  String name;
  String image;
  String bearerToken;
  categoryDrugsPage({ required this.id,required this.name,required this.image,required this.bearerToken,super.key});
  @override
  _categoryDrugsState createState() => _categoryDrugsState();
}

class _categoryDrugsState extends State<categoryDrugsPage> {
  List<Drug> categoryDrugs = [];
  List<Brand> brands = [];
  List<Drug> _categoryDrugs=[];

  List<Drug> displayedList = [];

  void getData() async {
    var urlBrand = Uri.parse('$urll/Brands');
    Response responseBrand = await get(urlBrand,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListBrand = json.decode(responseBrand.body);
    brands = jsonListBrand.map((json) => toBrand(json)).toList();
    var url = Uri.parse('$urll/Drugs');
    Response response = await get(url,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonList = json.decode(response.body);
    categoryDrugs = jsonList.map((json) => toCategoryDrug(json)).toList();
      setState(() {
        _categoryDrugs=categoryDrugs.where((element) =>
        element.categoryId==widget.id
        ).toList();
        for(int i =0;i<_categoryDrugs.length;i++) {
          displayedList.add(_categoryDrugs[i]);
        }
      });


  }
    String getBrandName(int brandId){
      Brand brand=brands.firstWhere((element) => element.id==brandId);
      return brand.name;
    }
  Drug toCategoryDrug(Map<String, dynamic> map) {
    String brandName=getBrandName(map['brandId']);
    Drug drug = Drug(
        map['id'],
        map['brandId'],
        map['categoryId'],
        map['englishName'],
        map['arabicName'],
        map['description'],
        map['sideEffects'],
        map['image'],brandName,widget.name);
    return drug;
  }
  Brand toBrand(Map<String, dynamic> map) {
    Brand brand = Brand(map['id'],map['name'],map['image']);
    return brand;
  }
  @override
  void initState() {
    getData();
    super.initState();
  }
  void updateList(String value) {
    displayedList = _categoryDrugs
        .where((element) => element.englishName.toLowerCase().contains(value.toLowerCase())
        || element.brandName.toLowerCase().contains(value.toLowerCase())).toList();
    setState(() {

    });
  }
  @override
  Widget build(BuildContext context) {

    int categoryId = widget.id;
    String categoryName = widget.name;
    String categoryImage = widget.image;
    final route = ModalRoute.of(context);
    if (route != null) {
      final routesArgs = route.settings.arguments as Map<String, dynamic>;
      categoryId = routesArgs['id'];
      categoryName = routesArgs['name'];
      categoryImage = routesArgs['image'];
    }

    // getData(brandId);

    /*setState(() {
      _categoryDrugs=categoryDrugs.where((element) =>
      element.categoryId==categoryId
      ).toList();
      for(int i =0;i<_categoryDrugs.length;i++) {
        displayedList.add(_categoryDrugs[i]);
      }
    });*/
    CategoryModel category = CategoryModel(categoryId, categoryName, categoryImage);

    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: Text(categoryName),
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
                          border: Border(
                            bottom: BorderSide(color: Colors.teal,width: 2,style: BorderStyle.solid),
                          ),
                          image: DecorationImage(
                            //alignment: Alignment.center,
                              image: NetworkImage(category.getImage(widget.image)),
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
                                hintText: 'Search by drug/brand',
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
                        )
                            :Column(
                            children:[
                              Container(
                                margin: const EdgeInsets.symmetric(horizontal: 130,vertical: 20),
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
                                  child:Text("There are no drugs\n"
                                      " that belong to this category yet",
                                    style: TextStyle(
                                      color: Colors.teal[700],
                                      fontSize: 20,
                                    ),
                                    textAlign: TextAlign.center,
                                  )
                              )]),                      ]),
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
                                                .getImage(
                                                displayedList[i].image)),
                                            fit: BoxFit.contain,
                                            opacity: 1),
                                      )       ))
                              ,
                              Container(
                                  margin: const EdgeInsets.only(right: 0,top:10),
                                  child: TextButton(
                                    onPressed: () {
                                      /*Brand drugBrands=brands.firstWhere((element) => element.id==categoryDrugs[i].brandId);
                                      String brandName=drugBrands.name;*/
                                      Drug drug = Drug(
                                          displayedList[i].id, displayedList[i].brandId,
                                          displayedList[i].categoryId,
                                          displayedList[i].englishName,displayedList[i].arabicName,
                                          displayedList[i].description,
                                          displayedList[i].sideEffects, displayedList[i].image,
                                          displayedList[i].brandName,
                                          widget.name);
                                      drug.selectDrug(context,displayedList[i].brandName,widget.name);
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



