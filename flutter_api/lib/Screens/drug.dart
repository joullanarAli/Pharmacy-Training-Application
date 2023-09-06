//import 'dart:js';

//import 'dart:html';

import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_api/services/ActiveIngredient.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Category.dart';
import 'package:flutter_api/services/DrugActiveIngredients.dart';
import 'package:flutter_api/services/DrugForms.dart';
import 'package:flutter_api/services/Form.dart';
import 'package:http/http.dart';

import 'main.dart';
import '../services/Drug.dart';
class drugPage extends StatefulWidget {
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
  String bearerToken;
  drugPage({super.key, required this.id,required this.brandId,required this.categoryId,
    required this.englishName,required this.arabicName,required this.description,
    required this.sideEffects,required this.image,required this.brandName,
    required this.categoryName,required this.bearerToken});
  String getImage(String imageName){
    if(imageName!=null) {
      return '$urll/Images/Drugs/$imageName';
    } else {
      return '$urll/Images/Drugs/DrugNotFound.jpg';
    }
  }
  _drugState createState() => _drugState();
}

class _drugState extends State<drugPage> {
  List<DrugForms> drugForms = [];
  List<DrugActiveIngredients> drugActiveIngredients=[];
  List<FormModel> forms = [];
  List<ActiveIngredient> activeIngredients=[];

  void getData() async {
    var urlDrugForms = Uri.parse('$urll/Drugs/GetDrugForms?drugId=${widget.id}');
    Response responseDrugForms = await get(urlDrugForms,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListDrugForms= json.decode(responseDrugForms.body);
    drugForms = jsonListDrugForms.map((json) => toDrugForm(json)).toList();
    var urlDrugActiveIngredients = Uri.parse('$urll/Drugs/GetDrugActiveIngredients?drugId=${widget.id}');
    Response responseDrugActiveIngredients = await get(urlDrugActiveIngredients,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListDrugActiveIngredients= json.decode(responseDrugActiveIngredients.body);
    drugActiveIngredients = jsonListDrugActiveIngredients.map((json) => toDrugActiveIngredient(json)).toList();
    var urlForms = Uri.parse('$urll/Forms');
    Response responseForms = await get(urlForms,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListForms= json.decode(responseForms.body);
    forms = jsonListForms.map((json) => toForm(json)).toList();
    var urlActiveIngredients = Uri.parse('$urll/ActiveIngredients');
    Response responseActiveIngredients = await get(urlActiveIngredients,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListActiveIngredients= json.decode(responseActiveIngredients.body);
    activeIngredients = jsonListActiveIngredients.map((json) => toActiveIngredient(json)).toList();
    setState(() {
    });
  }
  DrugActiveIngredients toDrugActiveIngredient(Map<String, dynamic> map) {
    DrugActiveIngredients drugActiveIngredients= DrugActiveIngredients(map['activeIngredientId']);
    return drugActiveIngredients;
  }
  DrugForms toDrugForm(Map<String, dynamic> map) {
    DrugForms drugForms= DrugForms(map['drugId'],map['formId'], map['volume'], map['dose']);
    return drugForms;
  }
  FormModel toForm(Map<String, dynamic> map) {
    FormModel form= FormModel (map['id'], map['name'], map['image']);
    return form;
  }
  ActiveIngredient toActiveIngredient(Map<String, dynamic> map) {
    ActiveIngredient activeIngredient= ActiveIngredient (map['id'], map['name']);
    return activeIngredient;
  }
  String getFormName(int formId){
    FormModel form =forms.firstWhere((element) => element.id==formId);
    String formName=form.name;
    return formName;
  }
  String getActiveIngredientName(int activeIngredientId){
    ActiveIngredient activeIngredient =activeIngredients.firstWhere((element) => element.id==activeIngredientId);
    String activeIngredientName=activeIngredient.name;
    return activeIngredientName;
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



    setState(() {
    });
    try {
    }catch(err){
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
          title: Text(widget.englishName),
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
                              image: NetworkImage(widget.getImage(widget.image)),
                              fit: BoxFit.fill),
                        ),
                      )
                    ],
                  ),
                  Card(
                    margin: const EdgeInsets.symmetric(horizontal: 15),
                    shadowColor: Colors.black87,
                    color: const Color.fromRGBO(215, 240, 231, 0.7),
                    child:
                    Container(
                      padding: const EdgeInsets.symmetric(vertical: 10,horizontal: 20),
                    child:
                    Text(
                      'Name: ${widget.englishName} ${widget.arabicName}\n\nBrand: ${widget.brandName}'
                          '\nCategory: ${widget.categoryName}\nDescription: ${widget.description}\nSide Effects: ${widget.sideEffects}',
                      style:  const TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 15,
                        color: Colors.black87,
                      ),)

                    ),
                  ),
                  Container(
                    margin: const EdgeInsets.only(top: 50,bottom: 10,left:20, right:20),
                    child:  const Text('Forms:',
                      style: TextStyle(
                        fontSize: 30,
                        fontWeight: FontWeight.bold,
                        color: Colors.black87
                      ),
                    ),
                  ),
                  GridView.count(
                    physics: const ScrollPhysics(),
                    scrollDirection: Axis.vertical,
                    crossAxisCount: 2,
                    mainAxisSpacing:10,
                    shrinkWrap: true,
                    children: [
                      for (int i=0;i<drugForms.length;i++)
                        Column(
                          children: [
                             Container(
                              height:110,
                              width: 150,
                              margin: const EdgeInsets.only(left: 10,right: 10,top:10,bottom: 0),
                              padding: const EdgeInsets.symmetric(
                              vertical: 20, horizontal: 20),
                              decoration: BoxDecoration(
                              color: Colors.white60,
                              borderRadius: BorderRadius.circular(35),
                            ),
                              child: Text(
                                  'Form: ${getFormName(drugForms[i].formId)}\n'
                                  'Dose: ${drugForms[i].dose}\n'
                                  'Volume: ${drugForms[i].volume}',
                              style:  const TextStyle(
                                fontSize: 15,
                                color:Colors.black87,
                                fontWeight: FontWeight.bold
                              ),),
                            ),]),]),
                      if(drugForms.isEmpty)
                           Column(
                          children:[
                            Container(
                              margin: const EdgeInsets.symmetric(horizontal: 120,vertical: 0),
                              width: 200,
                              height: 200,
                              decoration: BoxDecoration(
                                image: DecorationImage(
                                  image: notFoundImage.image,
                                ),
                              ),
                            ),
                            Container(
                                margin: const EdgeInsets.symmetric(vertical: 0,horizontal: 10),
                                child:Text("There are no forms for this drug yet",
                                  style: TextStyle(
                                    color: Colors.teal[700],
                                    fontSize: 20,
                                  ),
                                  textAlign: TextAlign.center,
                                )
                            )]),


                  Container(
                    margin: const EdgeInsets.only(left: 20,right: 20,top:20,bottom: 0),
                    child: const Text('Active Ingredients:',
                      style: TextStyle(
                        fontSize: 30,
                        fontWeight: FontWeight.bold,
                        color: Colors.black87
                      ),
                    ),
                  ),
                  GridView.count(
                    physics: const ScrollPhysics(),
                    scrollDirection: Axis.vertical,
                    crossAxisCount: 2,
                    mainAxisSpacing:10,
                    shrinkWrap: true,
                    children: [
                    for (int i=0;i<drugActiveIngredients.length;i++)
                      Column(
                        children: [
                          Container(
                            height:110,
                            width: 200,
                            margin: const EdgeInsets.symmetric(vertical: 10, horizontal: 10),
                            padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 20),
                            decoration: BoxDecoration(
                              color: Colors.white60,
                              borderRadius: BorderRadius.circular(35),
                            ),
                            child: Text(
                              'Active Ingredient: ${getActiveIngredientName(drugActiveIngredients[i].activeIngredientId)}\n',
                              style: const TextStyle(
                                fontSize: 15,
                                color: Colors.black87,
                                fontWeight: FontWeight.bold
                              ),
                            ),
                          )
                       ],
                      )

                ]),
                  if(drugActiveIngredients.isEmpty)
                    Column(
                        children:[
                          Container(
                            margin: const EdgeInsets.symmetric(horizontal: 120,vertical: 0),
                            width: 200,
                            height: 200,
                            decoration: BoxDecoration(
                              image: DecorationImage(
                                image: notFoundImage.image,
                              ),
                            ),
                          ),
                          Container(
                              margin: const EdgeInsets.only(bottom: 5,left: 10,right: 10),
                              child:Text("There are no active ingredient\n"
                                  " for this drug yet",
                                style: TextStyle(
                                  color: Colors.teal[700],
                                  fontSize: 20,
                                ),
                                textAlign: TextAlign.center,
                              )
                          )]),
                ])));
  }

  @override
  State<StatefulWidget> createState() {
// TODO: implement createState
    throw UnimplementedError();
  }
}



