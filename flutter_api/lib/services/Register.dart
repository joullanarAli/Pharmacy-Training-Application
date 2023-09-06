//import 'dart:html';

class Register{
  final String? emailAddress;
  final String? password;
  final String? confirmPassword;
  Register(this.emailAddress,this.password,this.confirmPassword);
  Register.fromJson(Map<String, dynamic> json)
      : emailAddress = json['email'],
        password = json['password'],
        confirmPassword=json['confirmedPassword'];

  Map<String, dynamic> toJson() {
    return {
      "email": emailAddress.toString(),
      "password": password.toString(),
      "confirmedPassword": confirmPassword.toString()
    };
  }
}