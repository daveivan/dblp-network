# Collaborative network from DBLP

This application allows parsing DBLP xml file and creating Collaborative network.

## Usage

Run program with parameters:

* -y = start year (required)
* -t = end year (optional)
* -f = path to dblp xml file (optional, default is ./dblp.xml

Created network will contain authors published between start and end year. Edges between authors indicates co-authors.

Example:

```
dblp.exe -y 2000 -t 2002 -f "path/to/dblp.xml"
```

## Output

Output contains two files:

* graph.csv - key:value list of authors (ID:name)
* nodes.csv - edge list with ID of authors where edge indicates co-authors

## DBLP data

This service provides open bibliographic information on major computer science journals and proceedings.

You can download XML file from http://dblp.dagstuhl.de/xml/release/. With xml file there must be downloaded .dtd file too.

Note that unzip file is about 2GB big. Parsing will take few minutes.