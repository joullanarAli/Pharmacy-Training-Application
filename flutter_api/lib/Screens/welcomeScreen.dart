import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class welcomeScreenPage extends StatefulWidget {
  //welcomeScreenPage({required String bearerToken,required this.brands});
  @override
  _welcomeScreenState createState() => _welcomeScreenState();
}

class _welcomeScreenState extends State<welcomeScreenPage> {

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
          title: const Text("Pharmacy Training Application"),
          titleTextStyle:  TextStyle(
          fontWeight: FontWeight.bold,
          fontSize: 22.0,
          color: Colors.teal[700],
          fontFamily: 'Roboto Condensed'),
          ),
        body: Container(
          height: double.maxFinite,
          decoration:  const BoxDecoration(
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
              ]
            )
          ),
        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
        )
    );
}}