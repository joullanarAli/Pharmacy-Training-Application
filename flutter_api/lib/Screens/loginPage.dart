import 'dart:convert';
import 'dart:ffi';
//import 'dart:html';
//import 'dart:html';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_api/services/Brand.dart';
import 'package:flutter_api/services/Login.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';

import 'main.dart';

class loginPage extends StatefulWidget {
  const loginPage({super.key});

  _loginState createState() => _loginState();
}
class _loginState extends State <loginPage>{
  String? emailAddress;
  String? password;
  bool _obscureText=true;
  String? errorMessage;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  List<Brand> brands=[];
  Future<void> getBrand() async {
    var urlBrand = Uri.parse('$urll/Brands');
    Response responseBrand = await get(urlBrand,headers:{
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'});
    final List<dynamic> jsonListBrand= json.decode(responseBrand.body);
    brands = jsonListBrand.map((json) => toBrand(json)).toList();
  }
  Brand toBrand(Map<String, dynamic> map) {
    Brand brand = Brand(map['id'],map['name'],map['image']);
    return brand;
  }
  @override
  void initState() {
    super.initState();
  }
  @override
Widget build(BuildContext context) {

        return Scaffold(
            appBar: AppBar(
            title: const Text(
              "Login",
              textAlign: TextAlign.center,
            ),
              iconTheme: const IconThemeData(
                color: Colors.black,
              ),
              centerTitle: true,
              titleTextStyle: TextStyle(
              color: Colors.teal[700],
              fontWeight: FontWeight.bold,
              fontSize: 30,
            ),
              backgroundColor: const Color(0xFFA0CED5),
            ),
            body:Container(
            // height: 600,
              padding: const EdgeInsets.only(bottom: 30, top: 10),
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
            child:Padding(
              padding: const EdgeInsets.all(10.0),
                child: ListView(
                    children:[
                          Container(
                              width: 200,
                              height: 150,
                              margin: const EdgeInsets.only(right: 40),
                              padding: const EdgeInsets.all(0),
                              child:
                              Container(
                                  decoration: BoxDecoration(
                                    //  color: Colors.white60,
                                    borderRadius: const BorderRadius.only(bottomLeft: Radius.circular(30),topLeft: Radius.circular(30) ),
                                    image: DecorationImage(
                                        image: primaryImage.image
                                    ),))),
               if(errorMessage!=null)
               SizedBox(
                 width: 300,
               //  height: 50,
                 child:Container(
                   margin: const EdgeInsets.only(left:20,top: 15,bottom: 15),
                   child: Text('$errorMessage',
                    style: const TextStyle(
                      fontSize: 20,
                      color: Colors.redAccent,
                      fontWeight: FontWeight.bold
                    ),
                 ),
               )),
               Form(
                 key: _formKey,
                child: Column(
                    children: <Widget>[
                      Container(
                        margin: const EdgeInsets.only(bottom: 40),
                      child:TextFormField(
                        onSaved: (String? value){emailAddress=value;},
                        decoration: InputDecoration(
                          labelText: 'Email',
                          iconColor: Colors.teal[700],
                          labelStyle: TextStyle(
                              color: Colors.teal[700],
                              fontWeight: FontWeight.bold),
                          icon: const Icon(Icons.email),
                        ),
                      )),
                      TextFormField(
                        onSaved: (String? value){password=value;},
                        obscureText: _obscureText,
                        decoration: InputDecoration(
                          labelText: 'Password',
                          iconColor: Colors.teal[700],
                          labelStyle: TextStyle(
                              color: Colors.teal[700],
                              fontWeight: FontWeight.bold),
                          icon: const Icon(Icons.lock),
                          suffixIcon: IconButton(
                            onPressed: () {
                              setState(() {
                                _obscureText = !_obscureText; // Toggle the visibility
                              });
                            },
                            icon: Icon(
                              _obscureText ? Icons.visibility_off : Icons.visibility,
                              color: Colors.teal[700],
                            ),
                          ),
                        ),
                      ),
                      Container(
                        margin: const EdgeInsets.only(top: 40,bottom: 10),
                        padding: const EdgeInsets.only(top: 20),
                        child: const Text("Don't have an account?",
                            style: TextStyle(
                                color: Colors.black87,
                                fontWeight: FontWeight.bold,
                                fontSize: 25)),
                      ),
                      Container(
                        margin: const EdgeInsets.only(bottom: 20),
                        child:TextButton(
                        style: TextButton.styleFrom(
                            textStyle: TextStyle(
                                color: Colors.teal[700],
                                fontSize: 15,
                                decoration: TextDecoration.underline)),
                        onPressed: () {
                          Navigator.pushNamed(context, '/register');
                        },
                        child: Text(
                          "Register",
                          style: TextStyle(
                            color: Colors.teal[700],
                            fontWeight: FontWeight.bold,
                            fontSize: 20,
                          ),
                        ),
                      )),
                      Row(
                        children:[
                        Container(
                          width: 300,
                            margin: const EdgeInsets.only(right: 42,left:42),
                            child: TextButton(
                              style: TextButton.styleFrom(
                                backgroundColor: Colors.teal[700],
                              ),
                              onPressed: () async {
                                if (_formKey.currentState!.validate()) {
                                  _formKey.currentState!.save();
                                }
                                var url = Uri.parse("$urll/Auth/Login");
                                Login login = Login(emailAddress, password);
                                String body = jsonEncode(login);
                               // await post(url, body);
                                final dynamic jsonString=await post(url,body);
                                if(await jsonString['isSuccess']==false){
                                  setState(() {
                                    errorMessage=jsonString['message'];
                                  });
                                }else{
                                  setState(() {
                                    errorMessage='';
                                  });
                                await getBrand();
                                Navigator.of(context).pushNamed(
                                    '/home', arguments: {
                                  'bearerToken': token,
                                  'brands':brands
                                });}
                              },
                              child: const Text(
                                "Login",
                                style: TextStyle(
                                    fontSize: 15,
                                    color: Colors.white70,
                                    fontWeight: FontWeight.bold),
                              ),)),
                      ],
                      ),
                    ]),
              ),
            ]))));
      }
}
Future<dynamic> post(var url,String body)async{
  return await http
      .post(url, body: body, headers: {"Content-Type":"application/json"})
      .then((http.Response response) {
    final int statusCode = response.statusCode;
    if (statusCode < 200 || statusCode > 400 || json == null) {
      throw Exception("Error while fetching data");
    }
    final  jsonString = json.decode(response.body);
    if(jsonString['isSuccess']==true) {
      token = jsonString['message'];
    }else{
      token='';
    }
   // return json.decode(response.body);
    return jsonString;
  });
}