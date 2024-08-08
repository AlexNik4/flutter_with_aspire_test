import 'dart:ui';

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
  String _otherEnvVariable = 'Loading...';
  String _apiHostname = '';
  String _anyUrl = '';
  String _anyResponse = '';

  @override
  void initState() {
    super.initState();
    _fetchEnvVariable();
  }

  Future<void> _fetchEnvVariable() async {
    try {
      var host =
          Uri.base.host; // This gives you the scheme, hostname, and port.
      if (host == "localhost") {
        host = "https://localhost:7265";
      } else {
        host = "https://$host";
      }
      final response = await http.get(Uri.parse('$host/get-env-variable'));
      if (response.statusCode == 200) {
        final data = json.decode(response.body);
        _apiHostname = data['realHostname'];
        setState(() {
          _envVariable = data.toString();
        });
      } else {
        setState(() {
          _envVariable = 'Failed to load environment variable';
        });
      }
    } catch (e) {
      setState(() {
        _envVariable = e.toString();
      });
    }
  }

  Future<void> _fetchOther() async {
    try {
      final response = await http
          .get(Uri.parse('https://$_apiHostname:7579/weatherforecast'));
      if (response.statusCode == 200) {
        final data = json.decode(response.body);

        setState(() {
          _otherEnvVariable = data.toString();
        });
      } else {
        setState(() {
          _otherEnvVariable = 'Failed to load api';
        });
      }
    } catch (e) {
      setState(() {
        _otherEnvVariable = e.toString();
      });
    }
  }

  Future<void> _fetchAny() async {
    try {
      final response = await http.get(Uri.parse(_anyUrl));
      if (response.statusCode == 200) {
        final data = json.decode(response.body);

        setState(() {
          _anyResponse = data.toString();
        });
      } else {
        setState(() {
          _anyResponse = 'Failed to load api';
        });
      }
    } catch (e) {
      setState(() {
        _anyResponse = e.toString();
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
        child: Column(
          children: [
            Text(
              _envVariable,
              style: const TextStyle(fontSize: 24),
            ),
            ElevatedButton(
                onPressed: () => {_fetchOther()},
                child: const Text('Use Different Container Api')),
            Text(
              _otherEnvVariable,
              style: const TextStyle(fontSize: 24),
            ),
            TextField(
              decoration: InputDecoration(
                hintText: 'Enter any api url',
              ),
              onChanged: (value) {
                _anyUrl = value;
              },
            ),
            ElevatedButton(
                onPressed: () => {_fetchAny()},
                child: const Text('Fetch Any Api')),
            Text(
              _anyResponse,
              style: const TextStyle(fontSize: 24),
            ),
          ],
        ),
      ),
    );
  }
}
