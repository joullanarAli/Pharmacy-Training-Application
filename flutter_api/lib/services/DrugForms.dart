import 'dart:convert';
import 'dart:ffi';
import 'dart:io';
//import 'dart:html';

import 'package:http/http.dart';

import '../Screens/main.dart';

class DrugForms{
  int drugId;
  int formId;
  double volume;
  double dose;
  DrugForms(this.drugId,this.formId,this.volume,this.dose);

}