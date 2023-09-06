//import 'dart:js';


//import 'dart:js';

import 'dart:convert';
//import 'dart:js';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'dart:typed_data';
import 'package:flutter/services.dart';
import 'activeIngredients.dart';
import 'brands.dart';
import 'categories.dart';
import 'drug.dart';
import 'forms.dart';
import 'home.dart';
import 'registerPage.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/Screens/welcomeScreen.dart';
import 'package:http/http.dart' as http;

import 'activeIngredientDrugs.dart';
import 'brandDrugs.dart';
import 'categoryDrugs.dart';
import 'drugsMainScreen.dart';
import 'examQuestions.dart';
import 'exams.dart';
import 'formDrugs.dart';
import 'loginPage.dart';
//import 'package:overflow_view/overflow_view.dart';
//import 'package:image/image.dart';

void main() {
  runApp(MaterialApp(
    debugShowCheckedModeBanner: false,
    //home: PharmacyApp(),
    initialRoute: '/welcomeScreenPage',
    routes: {
      '/welcomeScreenPage':(context)=> PharmacyApp(),
      '/home':(context){
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
        return homePage(
            bearerToken: token,
            brands:args['brands'],

        );
      },
      '/login':(context)=>loginPage(),
      '/register':(context)=>registerPage(),
      '/brands':(context){
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
        return brandsPage(
          bearerToken: token,
        );
      },
      '/categories': (context){
        return categoriesPage(
          bearerToken: token,
        );
      },
      '/exams':(context){
        return examsPage(
            bearerToken:token
        );
        },
      '/exam-questions': (context) {
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;

        return examQuestionsPage(
          id: args['id'],
          name: args['name'],
          bearerToken: token,
        );
      },
      '/activeIngredients':(context){
        return activeIngredientsPage(
          bearerToken: token,
        );

      },
      '/activeIngredients-drugs': (context) {
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;

        return activeIngredientDrugsPage(
          id: args['id'],
          name: args['name'],
          bearerToken: token,
        );
      },
      '/brand-drugs': (context) {
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;

        return brandDrugsPage(
          id: args['id'],
          name: args['name'],
          image: args['image'],
          bearerToken:token
        );
      },
      '/category-drugs': (context) {
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;

        return categoryDrugsPage(
          id: args['id'],
          name: args['name'],
          image: args['image'],
          bearerToken: token,
        );
      },
      '/forms':(context) {
        return formsPage(
            bearerToken: token
        );
      },
      '/form-drugs': (context) {
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;

        return formDrugsPage(
          id: args['id'],
          name: args['name'],
          image: args['image'],
          bearerToken: token,
        );
      },
      '/drugsMainScreen':(context) {
        return drugsMainPage(
            bearerToken: token
        );

      },
      '/drug': (context) {
        final Map<String, dynamic> args =
        ModalRoute.of(context)!.settings.arguments as Map<String, dynamic>;
        return drugPage(
          id: args['id'],
          brandId:args['brandId'],
          categoryId:args['categoryId'],
          englishName: args['englishName'],
          arabicName:args['arabicName'],
          description:args['description'],
          sideEffects:args['sideEffects'],
          image: args['image'],
          brandName: args['brandName'],
          categoryName: args['categoryName'],
          bearerToken: token,
          );
        },
    },
  ));
}
String urll='http://10.0.2.2:5191';
String token='';
Image img = Image.asset('images/logo.png');
Image flask =Image.asset('images/flask.jpg');
Image notFoundImage=Image.asset('images/notFound.png');
Image primaryImage=Image.asset('images/primary.png');
Image timeOutImage=Image.asset('images/pharmacist.jpg');
class PharmacyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          toolbarHeight: 70,
          //backgroundColor: Color(0xFFABB0C7),
         // backgroundColor: Color(0xFFBBBFD4),//truuuue
           backgroundColor: Color(0xFFA0CED5),
          centerTitle: true,
          leading: Container(
            margin: const EdgeInsets.only(left: 10.0, top: 3.0, bottom: 3.0),
            decoration: BoxDecoration(
                color: Colors.white,
                image: DecorationImage(image: img.image),
                border: Border.all(color: Colors.white, width: 1.0),
                shape: BoxShape.circle),
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
          decoration: const BoxDecoration(
            gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
              colors:[
                //truuue
               /* Color(0xFFABB0CC),
                Color(0xFFDCD7E4),
                Color(0xFFE6DADF),
                Color(0xFFD5CED1),*/
               // Color(0xFFA0CED5),
               // Color(0xFFA1CADC),
                Color(0xFFA8C4DC),
                Color(0xFFC8D5DD),
                //Color(0xFFD5CAD7),
                Color(0xFFD0CCD7),
                Color(0xFFDCC9CF),
              ]
            )
          ),
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 20),
            child: Column(children: [
              Row(
                 children:[
                Container(
                   padding: const EdgeInsets.all(80),
                  margin: const EdgeInsets.only(top: 50,left: 115,right: 115,bottom: 50),
                   /*decoration: BoxDecoration(
                     image: DecorationImage(
                         image: img1.image),
               ),*/

              )
                 ,]
              ),
              const Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                Text(
                  "Online Pharmacy Training App",
                  style: TextStyle(
                    color: Colors.black87,
                    fontSize: 25,
                    fontWeight: FontWeight.bold,
                    fontFamily: 'Roboto Condensed',
                    wordSpacing: 5,
                  ),
                  textAlign: TextAlign.center,
                ),
              ]),
              const Row(
                mainAxisAlignment: MainAxisAlignment.center,
                textDirection: TextDirection.ltr,
                verticalDirection: VerticalDirection.down,
                children: [
                  Flexible(
                      child: Text(
                    "Pharmacy Training App helps you training on all"
                    " medicines online, and pass exams which give you "
                    "certificates that assure you take the training plan  ",
                    style: TextStyle(
                      color: Colors.black54,
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      fontFamily: 'Roboto Condensed',
                      wordSpacing: 5,
                    ),
                    textAlign: TextAlign.center,
                  ))
                ],
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Container(
                    margin: const EdgeInsets.only(top:40),
                  child: TextButton(
                    style: TextButton.styleFrom(
                      backgroundColor: Colors.teal[700],
                      alignment: Alignment.center,
                      padding: const EdgeInsets.only(left: 100, right: 100,top:10,bottom: 10),
                    ),
                    onPressed: () {
                      //loginPage(context);
                      Navigator.pushNamed(context, '/login');
                    },
                    child: const Text(
                      "Login",
                      style: TextStyle(
                          fontWeight: FontWeight.bold,
                          fontSize: 20,
                          color: Colors.white70),
                    ),
                  ),
                  ),
                ]
              ),
            ]),
        )
    );
  }
}



