//import 'dart:js';

//import 'dart:html';

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Category.dart';
import 'package:flutter_api/services/DrugActiveIngredients.dart';
import 'package:flutter_api/services/DrugForms.dart';
import 'package:flutter_api/services/Form.dart';
import 'package:http/http.dart';

import 'main.dart';
import '../services/Drug.dart';
class drugsMainPage extends StatefulWidget {
  String bearerToken;
  drugsMainPage({super.key, required this.bearerToken});
  String getImage(String imageName){
    if(imageName!=null) {
      return '$urll/Images/Drugs/$imageName';
    } else {
      return '$urll/Images/Drugs/DrugNotFound.jpg';
    }
  }
  @override
  _drugsMainState createState() => _drugsMainState();
}

class _drugsMainState extends State<drugsMainPage> {
  List<Drug> drugs = [];
  List<DrugForms> drugForms = [];
  List<DrugActiveIngredients> drugActiveIngredients = [];
  List<FormModel> forms = [];
  List<Brand> brands = [];
  List<CategoryModel> categories = [];

  List<Drug> displayedList = [];


  void getData() async {

    var urlBrand = Uri.parse('$urll/Brands');
    Response responseBrand = await get(urlBrand,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListBrand= json.decode(responseBrand.body);
    brands = jsonListBrand.map((json) => toBrand(json)).toList();
    var urlCategory = Uri.parse('$urll/Categories');
    Response responseCategory = await get(urlCategory,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListCategory= json.decode(responseCategory.body);
    categories = jsonListCategory.map((json) => toCategory(json)).toList();
    var urlDrugs = Uri.parse('$urll/Drugs',);
    Response responseDrugs = await get(urlDrugs,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListDrugs = json.decode(responseDrugs.body);
    drugs = jsonListDrugs.map((json) => toDrug(json)).toList();
    for (int k = 0; k < drugs.length; k++) {
      displayedList.add(drugs[k]);
    }
    for (int j = 0; j < drugs.length; j++) {
      setState(() {});
    }
  }
  String getBrandName(int brandId){
    Brand brand=brands.firstWhere((element) => element.id==brandId);
    return brand.name;
  }
  String getCategoryName(int categoryId){
    CategoryModel category=categories.firstWhere((element) => element.id==categoryId);
    return category.name;
  }
  void updateList(String value) {
    setState(() {
      displayedList = drugs.where((element) =>
          element.englishName.toLowerCase().contains(value.toLowerCase())||element.arabicName.contains(value)
          || element.brandName.toLowerCase().contains(value.toLowerCase())
          ||element.categoryName.toLowerCase().contains(value.toLowerCase())).toList();
    });
  }
  Brand toBrand(Map<String, dynamic> map) {
    Brand brand = Brand(map['id'],map['name'],map['image']);
    return brand;
  }
  CategoryModel toCategory(Map<String, dynamic> map) {
    CategoryModel category = CategoryModel(map['id'],map['name'],map['image']);
    return category;
  }
  DrugActiveIngredients toDrugActiveIngredient(Map<String, dynamic> map) {
    DrugActiveIngredients drugActiveIngredients = DrugActiveIngredients(
        map['activeIngredientId']);
    return drugActiveIngredients;
  }

  DrugForms toDrugForm(Map<String, dynamic> map) {
    DrugForms drugForms = DrugForms(map['drugId'],map['formId'], map['volume'], map['dose']);
    return drugForms;
  }

  FormModel toForm(Map<String, dynamic> map) {
    FormModel form = FormModel(map['id'], map['name'], map['image']);
    return form;
  }

  Drug toDrug(Map<String, dynamic> map) {
    String brandName=getBrandName(map['brandId']);
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
        brandName,
        categoryName);
    return drug;
  }

  @override
  void initState() {
    getData();
    super.initState();
    setState(() {

    });
  }

  @override
  Widget build(BuildContext context) {
    setState(() {});
    try {} catch (err) {
      null;
    }


    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          backgroundColor: const Color(0xFFA0CED5),
          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: const Text('Drugs'),
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
                                hintText: 'Search by drug/category/brand',
                                hintStyle: TextStyle(
                                    color: Colors.teal[700],
                                    fontSize: 18,
                                    fontWeight: FontWeight.bold
                                ),
                                filled: true,
                                fillColor: Colors.white54,
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8.0),
                                    borderSide: BorderSide.none

                                )
                            ),
                            style:  TextStyle(
                              color: Colors.teal[700],
                              fontSize: 22,
                              fontWeight: FontWeight.bold,
                              fontStyle: FontStyle.normal
                            ),
                          ),
                        )
                            : Column(
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
                              )]),

                      ]),
                  GridView.count(
                    physics: ScrollPhysics(),
                    scrollDirection: Axis.vertical,
                    crossAxisCount: 2,
                    mainAxisSpacing: 10,
                    shrinkWrap: true,
                    children:[
                      for (int i = 0; i < displayedList.length; i++)

                        Column(children: [
                          Expanded(child: Container(
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
                              child: Container(
                                  margin: const EdgeInsets.only(left: 0, right: 0.0),
                                decoration: BoxDecoration(
                                  color: Colors.white,
                                  borderRadius: const BorderRadius.only(
                                      topLeft: Radius.circular(39),
                                      topRight: Radius.circular(39)
                                  ),
                                  image: DecorationImage(
                                      image: NetworkImage(displayedList[i].getImage(displayedList[i].image)),
                                      fit: BoxFit.contain,
                                      opacity: 1
                                  ),
                                )
                              ),),
                              Container(
                                margin: const EdgeInsets.only(right: 0,top:10),
                                child:TextButton(
                                  onPressed: () {
                                    /*CategoryModel drugCategory=categories.firstWhere((element) => element.id==drugs[i].categoryId);
                                    String categoryName=drugCategory.name;*/
                                   /* Brand drugBrands=brands.firstWhere((element) => element.id==drugs[i].brandId);
                                    String brandName=drugBrands.name;*/

                                    Drug drug = Drug(
                                        drugs[i].id,drugs[i].brandId,drugs[i].categoryId,
                                        drugs[i].englishName,drugs[i].arabicName,
                                        drugs[i].description,drugs[i].sideEffects,drugs[i].image,
                                      drugs[i].brandName,drugs[i].categoryName);
                                    drug.selectDrug(context,drugs[i].brandName,drugs[i].categoryName);
                                  },
                                  child: Text(
                                    displayedList[i].englishName,
                                    style: const TextStyle(
                                        fontWeight: FontWeight.bold,
                                        fontSize: 20.0,
                                        color: Color(0xFF242424),
                                        fontFamily: 'Roboto Condensed'),
                                    textAlign: TextAlign.left,
                                  ),
                                ),),

                                       ])))],
                            ),


                          ])
                            ]) ,
        ) );
  }


}
