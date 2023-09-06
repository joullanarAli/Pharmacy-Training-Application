import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_api/services/Register.dart';
import 'package:http/http.dart' as http;

import 'main.dart';
class registerPage extends StatefulWidget {
  const registerPage({super.key});

  _registerState createState() => _registerState();
}
class _registerState extends  State<registerPage>{
  String? emailAddress;
  String? password;
  String? confirmPassword;
  bool _obscureText=true;
  String? errorMessage;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  @override
  Widget build(BuildContext context){
  return Scaffold(
      //context: context,
     // builder: (BuildContext buildContext) {
     //   return AlertDialog(
       //   scrollable: true,
         // backgroundColor: Colors.white,
   //       shadowColor: Colors.teal,
         appBar:AppBar(title: const Text(
            "Register",
            textAlign: TextAlign.center,
          ),
           iconTheme: const IconThemeData(
             color: Colors.black,
           ),
          titleTextStyle: TextStyle(
            color: Colors.teal[700],
            fontWeight: FontWeight.bold,
            fontSize: 30,
          ),
           centerTitle: true,
           backgroundColor: const Color(0xFFA0CED5),
         ),
          body: Container(
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
            child:Padding(
            padding: const EdgeInsets.all(5.0),
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
                  margin: const EdgeInsets.only(bottom: 30),
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
                  Container(
                      margin: const EdgeInsets.only(bottom: 30),
                  child: TextFormField(
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
                  )),
                  Container(
                      margin: const EdgeInsets.only(bottom: 5),
                      child: TextFormField(
                        onSaved: (String? value){
                          confirmPassword=value;
                        },
                        obscureText: _obscureText,
                        decoration: InputDecoration(
                          labelText: 'Confirm Password',
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
                    )
                  ),
                  Container(
                    margin: const EdgeInsets.only(top: 5,bottom: 0),
                    padding: const EdgeInsets.only(top: 20),
                    child: const Text("Already have an account?",
                        style: TextStyle(
                            color: Colors.black87,
                            fontWeight: FontWeight.bold,
                            fontSize: 25)),
                  ),
                  Container(
                    margin: const EdgeInsets.only(bottom: 5),
                  child:TextButton(
                    style: TextButton.styleFrom(
                        textStyle: TextStyle(
                            color: Colors.teal[700],
                            fontSize: 15,
                            decoration: TextDecoration.underline)),
                    onPressed: () {
                      //Navigator.pop(context);
                     // loginPage(context);
                      Navigator.pushNamed(context, '/login');
                    },
                    child: Text(
                      "Login",
                      style: TextStyle(
                        color: Colors.teal[700],
                        fontWeight: FontWeight.bold,
                        fontSize: 20,
                      ),
                    ),
                  )),
                  Row(
                      children:[Container(
                        width: 150,
                        margin: EdgeInsets.only(left:50),
                        child:TextButton(
                        style: TextButton.styleFrom(
                          backgroundColor: Colors.grey[300],
                        ),
                        onPressed: () {
                        //  Navigator.pop(context);
                          Navigator.pushNamed(context, '/login');
                        },
                        child: const Text(
                          "Close",
                          style: TextStyle(
                              fontSize: 15,
                              color: Colors.black87,
                              fontWeight: FontWeight.bold),
                        ),
                      )),
                        Container(
                          width: 150,
                            margin: const EdgeInsets.only(right: 30,left:10),
                            child: TextButton(
                              style: TextButton.styleFrom(
                                backgroundColor: Colors.teal[700],
                              ),
                              onPressed: () async {
                                if (_formKey.currentState!.validate()) {
                                _formKey.currentState!.save();
                                }
                                var url= Uri.parse("$urll/Auth/Register");
                                Register register = Register(emailAddress, password, confirmPassword);
                                String body=jsonEncode(register);

                                final dynamic jsonString=await post(url, body);
                                if (jsonString is List || jsonString['errors'] is List) {
                                  List<dynamic> errors = jsonString is List ? jsonString : jsonString['errors'];
                                // Handle response as a list
                                  setState(() {
                                    errorMessage = errors.join('\n'); // Join errors into a string
                                  });
                                } else if (jsonString is Map<String, dynamic>) {
                                // Handle response as a map
                                if (jsonString['isSuccess'] == false || jsonString['status']==400) {
                                  setState(() {
                                    var errors=jsonString['errors'];
                                    if(errors!=null) {
                                      for (int i = 0; i < errors.length; i++) {
                                        if(errors is List) {
                                          errorMessage = jsonString['errors'][i];
                                        }else  {
                                             String jsonString2=jsonEncode(jsonString);
                                             var emailErrors=jsonDecode(jsonString2)['errors']['Email'];
                                             var passwordErrors=jsonDecode(jsonString2)['errors']['Password'];
                                             if(emailErrors is List || passwordErrors is List) {
                                               if(emailErrors!=null){
                                               for(int i=0;i<emailErrors.length;i++) {
                                                 errorMessage=emailErrors[i];
                                               }}
                                               if(passwordErrors!=null) {
                                                 for (int i = 0; i < passwordErrors.length; i++) {
                                                   errorMessage = passwordErrors[i];
                                                 }
                                               }

                                             }
                                          }
                                        }}
                                    else{
                                      errorMessage = jsonString['message'];
                                    }
                                  //  errorMessage = jsonString['errors'];
                                  });
                                } else {
                                  setState(() {
                                    errorMessage = null;
                                    Navigator.pushNamed(context, "/login");
                                  });
                                }
                                /*if(await jsonString['isSuccess']==false){
                                  setState(() {
                                  errorMessage=jsonString['errors'];
                                });
                                }else{
                                  setState(() {
                                  errorMessage=null;
                                });
                              }*/};
                                },
                              child: const Text(
                                "Create Account",
                                style: TextStyle(
                                    fontSize: 15,
                                    color: Colors.white70,
                                    fontWeight: FontWeight.bold),
                              ),)),])
                ],
              ),
            ),
            // )
          ]),

        )));
  }
}
Future<dynamic> post(var url,String body)async{
  return await http
      .post(url, body: body, headers: {"Content-Type":"application/json"})
      .then((http.Response response) {
   // print(response.body);
    final int statusCode = response.statusCode;
    if (statusCode < 200 || statusCode > 400 || json == null) {
      null;
    }
    return json.decode(response.body);
  });
}