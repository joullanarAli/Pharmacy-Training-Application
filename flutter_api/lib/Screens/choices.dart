import 'package:flutter/material.dart';

class choicesPage extends StatelessWidget {
  final String choiceText;
  final MaterialColor? choiceColor;
  final VoidCallback choiceTap;

  choicesPage({required this.choiceText, required this.choiceColor,required this.choiceTap});

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: choiceTap,
      child: Container(
        padding: const EdgeInsets.all(15.0),
        margin: const EdgeInsets.symmetric(vertical: 5.0, horizontal: 30.0),
        width: double.infinity,
        decoration: BoxDecoration(
          color: choiceColor,
          border: Border.all(color: Colors.blue),
          borderRadius: BorderRadius.circular(20.0),
        ),
        child:RawMaterialButton(
        onPressed: () {
          choiceTap();
        },
        child: Text(
          choiceText,
          style: const TextStyle(
            fontSize: 15.0,
          ),
        ),
      ),)
    );
  }
}