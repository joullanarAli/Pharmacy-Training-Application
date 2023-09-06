//import 'dart:html';

class Login{
  final String? emailAddress;
  final String? password;
  Login(this.emailAddress,this.password);
  Login.fromJson(Map<String, dynamic> json)
      : emailAddress = json['email'],
        password = json['password'];

  Map<String, dynamic> toJson() {
    return {
      "email": emailAddress.toString(),
      "password": password.toString(),
    };
  }
}