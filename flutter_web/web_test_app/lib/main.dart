import 'package:flutter/material.dart';
import 'dart:convert';
import 'package:http/http.dart' as http;

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Web Environment Variable',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(),
    );
  }
}

class MyHomePage extends StatefulWidget {
  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  String _envVariable = 'Loading...';

  @override
  void initState() {
    super.initState();
    _fetchEnvVariable();
  }

  Future<void> _fetchEnvVariable() async {
    final response =
        await http.get(Uri.parse('https://localhost:7265/get-env-variable'));
    if (response.statusCode == 200) {
      final data = json.decode(response.body);
      setState(() {
        _envVariable = data.toString();
      });
    } else {
      setState(() {
        _envVariable = 'Failed to load environment variable';
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Flutter Web Environment Variable'),
      ),
      body: Center(
        child: Text(
          _envVariable,
          style: const TextStyle(fontSize: 24),
        ),
      ),
    );
  }
}
