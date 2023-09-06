//import 'dart:js';

//import 'dart:html';

import 'dart:convert';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'dart:typed_data';
import 'package:flutter/services.dart';
import 'brands.dart';
import 'package:flutter_api/services/ActiveIngredient.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Category.dart';
import 'package:flutter_api/services/Drug.dart';
import 'package:flutter_api/services/Form.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';

import 'main.dart';

//import 'package:overflow_view/overflow_view.dart';
//import 'package:image/image.dart';
/*void main() {
  runApp(MaterialApp(
    home: homePage(),
  ));
}*/



class homePage extends StatefulWidget {
  String? bearerToken;
  List<Brand> brands;
  homePage({required String bearerToken,required this.brands});
  @override
  _homeState createState() => _homeState();
}

class _homeState extends State<homePage> {
  Image img = Image.asset('images/activeIngredients.jpg');
  Image img1 = Image.asset('images/notFound.png');
  Image exam = Image.asset('images/pharmacist.jpg');
  List<Drug> drugs = [];
  List<Brand> brands = [];
  List<CategoryModel> categories = [];
  List<Drug> displayedDrugsList = [];
  List<CategoryModel> displayedCategoriesList = [];
  List<Brand> displayedBrandsList = [];
  List<FormModel> forms = [];
  List<FormModel> displayedFormsList = [];
  List<ActiveIngredient> activeIngredients = [];
  List<ActiveIngredient> displayedActiveIngredientsList = [];

  Future<void> getData() async {
    var urlBrand = Uri.parse('$urll/Brands');
    Response responseBrand = await get(urlBrand, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListBrand = json.decode(responseBrand.body);
    brands = jsonListBrand.map((json) => toBrand(json)).toList();
    var urlCategory = Uri.parse('$urll/Categories');
    Response responseCategory = await get(urlCategory, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListCategory = json.decode(responseCategory.body);
    categories = jsonListCategory.map((json) => toCategory(json)).toList();
    var urlDrugs = Uri.parse('$urll/Drugs',);
    Response responseDrugs = await get(urlDrugs, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListDrugs = json.decode(responseDrugs.body);
    drugs = jsonListDrugs.map((json) => toDrug(json)).toList();
    var urlForms = Uri.parse('$urll/Forms');
    Response responseForm = await get(urlForms, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonList = json.decode(responseForm.body);
    forms = jsonList.map((json) => toForm(json)).toList();
    var urlActiveIngredients = Uri.parse('$urll/ActiveIngredients');
    Response response = await get(urlActiveIngredients, headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListActiveIngredients = json.decode(response.body);
    activeIngredients =
        jsonListActiveIngredients.map((json) => toActiveIngredient(json))
            .toList();
    displayCategories();
    displayBrands();
    displayDrugs();
    displayForms();
    displayActiveIngredients();
    setState(() {});
/*    for (int j = 0; j < drugs.length; j++) {
      setState(() {});
    }*/
  }

  void displayCategories() {
    if (categories.length >= 6) {
      for (int k = 0; k < 6; k++) {
        displayedCategoriesList.add(categories[k]);
      }
    } else if (categories.length >= 3) {
      for (int k = 0; k < 3; k++) {
        displayedCategoriesList.add(categories[k]);
      }
    } else {
      for (int k = 0; k < categories.length; k++) {
        displayedCategoriesList.add(categories[k]);
      }
    }
  }

  void displayDrugs() {
    if (drugs.length >= 6) {
      for (int k = 0; k < 6; k++) {
        displayedDrugsList.add(drugs[k]);
      }
    } else if (drugs.length >= 3) {
      for (int k = 0; k < 3; k++) {
        displayedDrugsList.add(drugs[k]);
      }
    } else {
      for (int k = 0; k < drugs.length; k++) {
        displayedDrugsList.add(drugs[k]);
      }
    }
  }

  void displayBrands() {
    if (widget.brands.length >= 2) {
      for (int k = 0; k < 2; k++) {
        displayedBrandsList.add(widget.brands[k]);
      }
    } else {
      for (int k = 0; k < widget.brands.length; k++) {
        displayedBrandsList.add(widget.brands[k]);
      }
    }
  }

  void displayActiveIngredients() {
    if (activeIngredients.length >= 2) {
      for (int k = 0; k < 2; k++) {
        displayedActiveIngredientsList.add(activeIngredients[k]);
      }
    } else {
      for (int k = 0; k < activeIngredients.length; k++) {
        displayedActiveIngredientsList.add(activeIngredients[k]);
      }
    }
  }

  void displayForms() {
    if (forms.length >= 2) {
      for (int k = 0; k < 2; k++) {
        displayedFormsList.add(forms[k]);
      }
    } else {
      for (int k = 0; k < forms.length; k++) {
        displayedFormsList.add(forms[k]);
      }
    }
  }

  Brand toBrand(Map<String, dynamic> map) {
    Brand brand = Brand(map['id'], map['name'], map['image']);
    return brand;
  }

  CategoryModel toCategory(Map<String, dynamic> map) {
    CategoryModel category = CategoryModel(
        map['id'], map['name'], map['image']);
    return category;
  }

  FormModel toForm(Map<String, dynamic> map) {
    FormModel form = FormModel(map['id'], map['name'], map['image']);
    return form;
  }

  ActiveIngredient toActiveIngredient(Map<String, dynamic> map) {
    ActiveIngredient activeIngredient = ActiveIngredient(
        map['id'], map['name']);
    return activeIngredient;
  }

  String getBrandName(int brandId) {
    Brand brand = widget.brands.firstWhere((element) => element.id == brandId);
    return brand.name;
  }

  String getCategoryName(int categoryId) {
    CategoryModel category = categories.firstWhere((element) =>
    element.id == categoryId);
    return category.name;
  }

  Drug toDrug(Map<String, dynamic> map) {
    String brandName = getBrandName(map['brandId']);
    String categoryName = getCategoryName(map['categoryId']);

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

  void updateList(String value) {
    setState(() {
      List<Drug> _drugs = drugs.where((element) =>
      element.englishName.toLowerCase().contains(value.toLowerCase())
          || element.brandName.toLowerCase().contains(value.toLowerCase())
          || element.categoryName.toLowerCase().contains(value.toLowerCase())
      )
          .toList();
      if(_drugs.isNotEmpty) {
        displayedDrugsList.clear();
        if(_drugs.length>=3) {
          displayedDrugsList.clear();
          for (int i = 0; i < 3; i++) {
            displayedDrugsList.add(_drugs[i]);
          }
        }else{
          displayedDrugsList.clear();
          for(int i=0;i<_drugs.length;i++){
            displayedDrugsList.add(drugs[i]);
          }
        }
      }else {
        for(int i=0;i<3;i++){
          displayedDrugsList.add(drugs[i]);
        }
      }
    });
  }

  void initState() {
    getData();
    super.initState();
    setState(() {

    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          //backgroundColor: Color(0xFFBBBFD4),
          backgroundColor: const Color(0xFFA0CED5),

          centerTitle: true,
          iconTheme: const IconThemeData(
            color: Colors.black,
          ),
          title: const Text("Pharmacy Training Application"),
          titleTextStyle: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 22.0,
              color: Color(0xFF242424),
              fontFamily: 'Roboto Condensed'),
        ),
       // drawer: CustomSidebar(),
        body: Container(
          height: double.maxFinite,
          decoration: const BoxDecoration(
              gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  //   stops: [0.9, 0.1],
                  colors: [
                    Color(0xFFA0CED5),
                    Color(0xFFA1CADC),
                    Color(0xFFA8C4DC),
                    Color(0xFFC8D5DD),
                    Color(0xFFD5CAD7),
                    Color(0xFFD0CCD7),
                    Color(0xFFDCC9CF),
                    /*Color(0xFFD3E1E0),
              Color(0xFFD3E5E8),
              Color(0xFFD4E4EA),
              Color(0xFFD5E1EA),
              Color(0xFFDADBE4),
              Color(0xFFE1DDE9),
              Color(0xFFE3D7E4),
              Color(0xFFEBD8DE),*/
                    //  Color(0xFFB2CBB3),
                    /* Color(0xFFA0CED5),
              Color(0xFFA1CADC),
              Color(0xFFA8C4DC),
              Color(0xFFC8D5DD),
              Color(0xFFD5CAD7),
              Color(0xFFD0CCD7),
              Color(0xFFDCC9CF),*/
                    /*Color(0xFF84C2BB),
              Color(0xFF8BB9CE),
              //Color(0xFF8AAFCC),
              Color(0xFFA2C4D4),
              Color(0xFFC4C7D8),
              Color(0xFFD7D3DD),
              Color(0xFFD1CCDA),*/
                    /* Color(0xFFBACCCF),
              Color(0xFFB1CDE4),
              Color(0xFFBAC5CD),
              Color(0xFFB9BCD4),
              Color(0xFFD2CDD8),
              Color(0xFFD6CCD8),
              Color(0xFFD9CFDB),
              //  Color(0xFFB4B9D3),
            //  Color(0xFFE3CDD5),
              Color(0xFFE4D4D9),*/
                    /*Color(0xFFABB0CC),
              Color(0xFFDCD7E4),
             // Color(0xFFCDB4D1),
              Color(0xFFE6DADF),
              Color(0xFFD5CED1)*/


                  ]
              )
          ),
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
          child: ListView(
              children: [
               /* Container(
                  width: 387,
                  height: 50,
                  margin: const EdgeInsets.only(
                      left: 10, right: 10, top: 10, bottom: 20),
                  child: TextField(
                    onChanged: (value) => updateList(value),
                    decoration: InputDecoration(
                        prefixIcon: const Icon(Icons.search, size: 25,),
                        prefixIconColor: Colors.teal[700],
                        hintText: 'Filter drugs by name,category,brand',
                        hintStyle: TextStyle(
                            color: Colors.teal[700],
                            fontSize: 18,
                            fontWeight: FontWeight.bold
                        ),
                        filled: true,
                        fillColor: Colors.white54,
                        border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(50.0),
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
                ),*/
                Row(
                  children: [
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 10, bottom: 10, left: 25),
                          child: const Text(
                            'Drugs',
                            style: TextStyle(
                              fontSize: 25,
                              // fontWeight: FontWeight.bold,
                              color: Color(0xFF242424),
                            ),
                          ),
                        ),
                      ],
                    ),
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 10, bottom: 10, left: 190),
                          child: TextButton(
                            onPressed: () {
                              Navigator.of(context).pushNamed(
                                  '/drugsMainScreen', arguments: {
                                'bearerToken': token,
                              });
                            },
                            child: Text(
                              'View All >',
                              style: TextStyle(
                                  fontSize: 20,
                                  // fontWeight: FontWeight.bold,
                                  color: Colors.teal[700]
                              ),
                            ),
                          ),
                        )
                      ],
                    )
                  ],
                ),
                displayedDrugsList.isNotEmpty ?
                GridView.count(
                    physics: const ScrollPhysics(),
                    scrollDirection: Axis.vertical,
                    crossAxisCount: 3,
                    mainAxisSpacing: 0,
                    crossAxisSpacing: 0,
                    shrinkWrap: true,
                    children: [
                      for (int i = 0; i < displayedDrugsList.length; i++)
                        Column(children: [
                          Container(
                              height: 120,
                              width: 200,
                              margin: const EdgeInsets.symmetric(
                                  vertical: 0, horizontal: 10),
                              padding: const EdgeInsets.only(
                                  top: 0, left: 0),
                              decoration: BoxDecoration(
                                color: Colors.white60,
                                borderRadius: BorderRadius.circular(35),
                              ),
                              child: Column(children: [
                                //Column(children: [
                                Flexible(
                                    child:
                                    Container(
                                        width: 200,
                                        height: 250,
                                        margin: const EdgeInsets.only(
                                            left: 0, top: 0, bottom: 0),
                                        padding: const EdgeInsets.all(0),
                                        child:
                                        Container(
                                            margin: const EdgeInsets.only(
                                                right: 0, bottom: 0, left: 0),
                                            decoration: BoxDecoration(
                                              color: Colors.white,
                                              borderRadius: const BorderRadius
                                                  .only(
                                                  topRight: Radius.circular(30),
                                                  topLeft: Radius.circular(30)),
                                              image: DecorationImage(
                                                //  colorFilter:const  ColorFilter.mode(Colors.white, BlendMode.lighten),
                                                  image: NetworkImage(
                                                      displayedDrugsList[i]
                                                          .getImage(
                                                          displayedDrugsList[i]
                                                              .image)),
                                                  fit: BoxFit.contain,
                                                  opacity: 1
                                              ),)))),
                                Container(
                                  //  width:50,
                                  margin: const EdgeInsets.only(
                                      left: 0, top: 0, right: 0),
                                  child: TextButton(
                                    onPressed: () {
                                      Drug drug = Drug(
                                          displayedDrugsList[i].id,
                                          displayedDrugsList[i].brandId,
                                          displayedDrugsList[i].categoryId,
                                          displayedDrugsList[i].englishName,
                                          displayedDrugsList[i].arabicName,
                                          displayedDrugsList[i].description,
                                          displayedDrugsList[i].sideEffects,
                                          displayedDrugsList[i].image,
                                          displayedDrugsList[i].brandName,
                                          displayedDrugsList[i].categoryName);
                                      drug.selectDrug(context,
                                          displayedDrugsList[i].brandName,
                                          displayedDrugsList[i].categoryName);
                                    },
                                    child: Text(
                                      displayedDrugsList[i].englishName,
                                      style: const TextStyle(
                                          fontWeight: FontWeight.bold,
                                          fontSize: 15.0,
                                          color: Color(0xFF242424),
                                          fontFamily: 'Roboto Condensed'),
                                      textAlign: TextAlign.left,
                                    ),
                                  ),),

                              ]))
                        ])
                    ])
                    : Column(
                    children: [
                      Container(
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
                          child: Text("There are no drugs yet",
                            style: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 25,
                            ),
                            textAlign: TextAlign.center,
                          )
                      )
                    ]),

                Row(
                  children: [
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 25),
                          child: const Text(
                            'Categories',
                            style: TextStyle(
                              fontSize: 25,
                              // fontWeight: FontWeight.bold,
                              color: Color(0xFF242424),
                            ),
                          ),
                        ),
                      ],
                    ),
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 130),
                          child: TextButton(
                            onPressed: () {
                              Navigator.of(context).pushNamed(
                                  '/categories', arguments: {
                                'bearerToken': token,
                              });
                            },
                            child: Text(
                              'View All >',
                              style: TextStyle(
                                  fontSize: 20,
                                  // fontWeight: FontWeight.bold,
                                  color: Colors.teal[700]
                              ),
                            ),
                          ),
                        )
                      ],
                    )
                  ],
                ),
                displayedCategoriesList.isNotEmpty ?
                GridView.count(
                    physics: const ScrollPhysics(),
                    scrollDirection: Axis.vertical,
                    crossAxisCount: 3,
                    mainAxisSpacing: 0,
                    crossAxisSpacing: 0,
                    shrinkWrap: true,
                    children: [
                      for (int i = 0; i < displayedCategoriesList.length; i++)
                        Column(children: [
                          Container(
                              height: 120,
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
                                  margin: const EdgeInsets.only(
                                      left: 0, top: 0, right: 0),
                                  child: TextButton(
                                    onPressed: () {
                                      CategoryModel category = CategoryModel(
                                          displayedCategoriesList[i].id,
                                          displayedCategoriesList[i].name,
                                          displayedCategoriesList[i].image);
                                      category.selectCategory(context);
                                    },
                                    child: Text(
                                      displayedCategoriesList[i].name,
                                      style: const TextStyle(
                                          fontWeight: FontWeight.bold,
                                          fontSize: 18.0,
                                          color: Color(0xFF242424),
                                          fontFamily: 'Roboto Condensed'),
                                      textAlign: TextAlign.left,
                                    ),
                                  ),),
                                Flexible(
                                    child:
                                    Container(
                                        width: 100,
                                        height: 400,
                                        margin: const EdgeInsets.only(
                                            left: 10, top: 0, bottom: 0),
                                        padding: const EdgeInsets.all(0),
                                        child:
                                        Container(
                                            margin: const EdgeInsets.only(
                                                right: 0, bottom: 0, left: 0),
                                            decoration: BoxDecoration(
                                              //  color: Colors.white60,
                                              borderRadius: const BorderRadius
                                                  .only(
                                                  bottomRight: Radius.circular(
                                                      30)),
                                              image: DecorationImage(
                                                //  colorFilter:const  ColorFilter.mode(Colors.white, BlendMode.lighten),
                                                  image: NetworkImage(
                                                      displayedCategoriesList[i]
                                                          .getImage(
                                                          displayedCategoriesList[i]
                                                              .image)),
                                                  fit: BoxFit.contain,
                                                  opacity: 1
                                              ),)))),
                              ]))
                        ])
                    ])
                    : Column(
                    children: [
                      Container(
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
                          child: Text("There are no categories yet",
                            style: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 25,
                            ),
                            textAlign: TextAlign.center,
                          )
                      )
                    ]),
                Row(
                  children: [
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 25),
                          child: const Text(
                            'Brands',
                            style: TextStyle(
                              fontSize: 25,
                              // fontWeight: FontWeight.bold,
                              color: Color(0xFF242424),
                            ),
                          ),
                        ),
                      ],
                    ),
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 180),
                          child: TextButton(
                            onPressed: () {
                              Navigator.of(context).pushNamed(
                                  '/brands', arguments: {
                                'bearerToken': token,
                              });
                            },
                            child: Text(
                              'View All >',
                              style: TextStyle(
                                  fontSize: 20,
                                  // fontWeight: FontWeight.bold,
                                  color: Colors.teal[700]
                              ),
                            ),
                          ),
                        )
                      ],
                    )
                  ],
                ),
                displayedBrandsList.isNotEmpty ?
                Row(
                    children: [
                      for (int i = 0; i < displayedBrandsList.length; i++)
                        Column(children: [
                          Container(
                              height: 70,
                              width: 185,
                              margin: const EdgeInsets.symmetric(
                                  vertical: 0, horizontal: 5),
                              decoration: BoxDecoration(
                                color: Colors.white60,
                                borderRadius: BorderRadius.circular(35),
                              ),
                              child: Row(children: [
                                //Column(children: [
                                Flexible(
                                    child:
                                    Container(
                                        width: 60,
                                        height: 70,
                                        margin: const EdgeInsets.only(
                                            left: 0, top: 0, bottom: 0),
                                        padding: const EdgeInsets.all(0),
                                        child:
                                        Container(
                                            decoration: BoxDecoration(
                                              //  color: Colors.white60,
                                              borderRadius: const BorderRadius
                                                  .only(
                                                  bottomLeft: Radius.circular(
                                                      30),
                                                  topLeft: Radius.circular(30)),
                                              image: DecorationImage(
                                                  image: NetworkImage(
                                                      displayedBrandsList[i]
                                                          .getImage(
                                                          displayedBrandsList[i]
                                                              .image)),
                                                  fit: BoxFit.cover,
                                                  opacity: 1
                                              ),)))),
                                TextButton(
                                  onPressed: () {
                                    Brand brand = Brand(
                                        displayedBrandsList[i].id,
                                        displayedBrandsList[i].name,
                                        displayedBrandsList[i].image);
                                    brand.selectBrand(context);
                                  },
                                  child: Text(
                                    displayedBrandsList[i].name,
                                    style: const TextStyle(
                                        fontWeight: FontWeight.bold,
                                        fontSize: 15.0,
                                        color: Color(0xFF242424),
                                        fontFamily: 'Roboto Condensed'),
                                    textAlign: TextAlign.left,
                                  ),
                                ),
                              ]))
                        ]),
                    ])
                    : Column(
                    children: [
                      Container(
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
                          child: Text("There are no brands yet",
                            style: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 25,
                            ),
                            textAlign: TextAlign.center,
                          )
                      )
                    ]),
                Row(
                  children: [
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 25),
                          child: const Text(
                            'Forms',
                            style: TextStyle(
                              fontSize: 25,
                              // fontWeight: FontWeight.bold,
                              color: Color(0xFF242424),
                            ),
                          ),
                        ),
                      ],
                    ),
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 180),
                          child: TextButton(
                            onPressed: () {
                              Navigator.of(context).pushNamed(
                                  '/forms', arguments: {
                                'bearerToken': token,
                              });
                            },
                            child: Text(
                              'View All >',
                              style: TextStyle(
                                  fontSize: 20,
                                  // fontWeight: FontWeight.bold,
                                  color: Colors.teal[700]
                              ),
                            ),
                          ),
                        )
                      ],
                    )
                  ],
                ),
                displayedFormsList.isNotEmpty ?
                Row(
                    children: [
                      for (int i = 0; i < displayedFormsList.length; i++)
                        Column(children: [
                          Container(
                              height: 70,
                              width: 185,
                              margin: const EdgeInsets.symmetric(
                                  vertical: 0, horizontal: 5),
                              decoration: BoxDecoration(
                                color: Colors.white60,
                                borderRadius: BorderRadius.circular(35),
                              ),
                              child: Row(children: [
                                //Column(children: [
                                Flexible(
                                    child:
                                    Container(
                                        width: 60,
                                        height: 70,
                                        margin: const EdgeInsets.only(
                                            left: 0, top: 0, bottom: 0),
                                        padding: const EdgeInsets.all(0),
                                        child:
                                        Container(
                                            decoration: BoxDecoration(
                                              //  color: Colors.white60,
                                              borderRadius: const BorderRadius
                                                  .only(
                                                  bottomLeft: Radius.circular(
                                                      30),
                                                  topLeft: Radius.circular(30)),
                                              image: DecorationImage(
                                                  image: NetworkImage(
                                                      displayedFormsList[i]
                                                          .getImage(
                                                          displayedFormsList[i]
                                                              .image)),
                                                  fit: BoxFit.cover,
                                                  opacity: 1
                                              ),)))),
                                TextButton(
                                  onPressed: () {
                                    FormModel form = FormModel(
                                        displayedFormsList[i].id,
                                        displayedFormsList[i].name,
                                        displayedFormsList[i].image);
                                    form.selectForm(context);
                                  },
                                  child: Text(
                                    displayedFormsList[i].name,
                                    style: const TextStyle(
                                        fontWeight: FontWeight.bold,
                                        fontSize: 15.0,
                                        color: Color(0xFF242424),
                                        fontFamily: 'Roboto Condensed'),
                                    textAlign: TextAlign.left,
                                  ),
                                ),
                              ]))
                        ]),
                    ])
                    : Column(
                    children: [
                      Container(
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
                          child: Text("There are no forms yet",
                            style: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 25,
                            ),
                            textAlign: TextAlign.center,
                          )
                      )
                    ]),
                Row(
                  children: [
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 25),
                          child: const Text(
                            'Active Ingredients',
                            style: TextStyle(
                              fontSize: 25,
                              color: Color(0xFF242424),
                            ),
                          ),
                        ),
                      ],
                    ),
                    Column(
                      children: [
                        Container(
                          margin: const EdgeInsets.only(
                              top: 40, bottom: 10, left: 50),
                          child: TextButton(
                            onPressed: () {
                              Navigator.of(context).pushNamed(
                                  '/activeIngredients', arguments: {
                                'bearerToken': token,
                              });
                            },
                            child: Text(
                              'View All >',
                              style: TextStyle(
                                  fontSize: 20,
                                  // fontWeight: FontWeight.bold,
                                  color: Colors.teal[700]
                              ),
                            ),
                          ),
                        )
                      ],
                    )
                  ],
                ),
                displayedActiveIngredientsList.isNotEmpty ?
                Row(
                    children: [
                      for (int i = 0; i <
                          displayedActiveIngredientsList.length; i++)
                        Column(children: [
                          Container(
                              height: 70,
                              width: 185,
                              margin: const EdgeInsets.symmetric(
                                  vertical: 0, horizontal: 5),
                              decoration: BoxDecoration(
                                color: Colors.white60,
                                borderRadius: BorderRadius.circular(35),
                              ),
                              child: Row(children: [
                                //Column(children: [
                                Flexible(
                                    child:
                                    Container(
                                        width: 60,
                                        height: 70,
                                        margin: const EdgeInsets.only(
                                            left: 0, top: 0, bottom: 0),
                                        padding: const EdgeInsets.all(0),
                                        child:
                                        Container(
                                            decoration: BoxDecoration(
                                              color: Colors.white60,
                                              borderRadius: const BorderRadius
                                                  .only(
                                                  bottomLeft: Radius.circular(
                                                      30),
                                                  topLeft: Radius.circular(30)),
                                              image: DecorationImage(
                                                  image: flask.image,
                                                  fit: BoxFit.contain,
                                                  opacity: 1
                                              ),)))),
                                TextButton(
                                  onPressed: () {
                                    ActiveIngredient activeIngredient = ActiveIngredient(
                                        displayedActiveIngredientsList[i].id,
                                        displayedActiveIngredientsList[i].name);
                                    activeIngredient.selectActiveIngredient(
                                        context);
                                  },
                                  child: Text(
                                    displayedActiveIngredientsList[i].name,
                                    style: const TextStyle(
                                        fontWeight: FontWeight.bold,
                                        fontSize: 15.0,
                                        color: Color(0xFF242424),
                                        fontFamily: 'Roboto Condensed'),
                                    textAlign: TextAlign.left,
                                  ),
                                ),
                              ]))
                        ]),
                    ])
                    : Column(
                    children: [
                      Container(
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
                          child: Text("There are no ActiveIngredients yet",
                            style: TextStyle(
                              color: Colors.teal[700],
                              fontSize: 25,
                            ),
                            textAlign: TextAlign.center,
                          )
                      )
                    ]),
                Row(
                    children: [
                      Column(children: [
                        Container(
                            height: 70,
                            width: 380,
                            margin: const EdgeInsets.symmetric(
                                vertical: 40, horizontal: 5),
                            decoration: BoxDecoration(
                              color: Colors.white60,
                              borderRadius: BorderRadius.circular(35),
                            ),
                            child: Row(children: [
                              //Column(children: [
                              Flexible(
                                  child:
                                  Container(
                                      width: 60,
                                      height: 70,
                                      margin: const EdgeInsets.only(right: 20,
                                          left: 0,
                                          top: 0,
                                          bottom: 0),
                                      padding: const EdgeInsets.all(0),
                                      child:
                                      Container(
                                          decoration: BoxDecoration(
                                            color: Colors.white60,
                                            borderRadius: const BorderRadius
                                                .only(
                                                bottomLeft: Radius.circular(30),
                                                topLeft: Radius.circular(30)),
                                            image: DecorationImage(
                                                image: exam.image,
                                                fit: BoxFit.cover,
                                                opacity: 1
                                            ),)))),
                              TextButton(
                                onPressed: () {
                                  Navigator.pushNamed(context, '/exams');
                                },
                                child: const Text(
                                  'Ready for Exam?',
                                  style: TextStyle(
                                      fontWeight: FontWeight.bold,
                                      fontSize: 20.0,
                                      color: Color(0xFF242424),
                                      fontFamily: 'Roboto Condensed'),
                                  textAlign: TextAlign.left,
                                ),
                              ),
                            ]))
                      ]),
                    ])
                /* Row(
           children: [
         Column(children: [
         Container(
         height: 70,
           width: 185,
           margin: const EdgeInsets.symmetric(
               vertical: 0, horizontal: 5),
           decoration: BoxDecoration(
             color: Colors.white60,
             borderRadius: BorderRadius.circular(35),
           ),

         )],)
          ]  ),*/
              ]),));
  }
}
  /*class CustomSidebar extends StatelessWidget {
    @override
    Widget build(BuildContext context) {
      return Drawer(
        Container(
            decoration: const BoxDecoration(
                gradient: LinearGradient(
                    begin: Alignment.topCenter,
                    end: Alignment.bottomCenter,
                    //   stops: [0.9, 0.1],
                    colors: [
                      Color(0xFFA0CED5),
                      Color(0xFFA1CADC),
                      Color(0xFFA8C4DC),
                      Color(0xFFC8D5DD),
                      Color(0xFFD5CAD7),
                      Color(0xFFD0CCD7),
                      Color(0xFFDCC9CF),
                    ]
                )
            ),
        child: ListView(
          padding: EdgeInsets.zero,
          children: <Widget>[


             const DrawerHeader(
              decoration: BoxDecoration(
            //  color:Color(0xFFA0CED5),
              ),
              child: Text(
              '',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 24,
                ),
              ),
            ),
            ListTile(
              title: Text('Item 1'),
              onTap: () {
              // Handle item 1 tap
              },
            ),
            ListTile(
              title: Text('Item 2'),
              onTap: () {
              // Handle item 2 tap
              },
            ),
          // Add more list tiles for additional menu items
      ],
        ),
        ));
    }
  }*/
