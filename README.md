Octopus
==========

Octopus is a web application to manage operations inside Santex Company

This use the following technologies

  - ASP.NET WEB API 2
  - AngularJS for the Front End
  - SQL Server 2008


Version
----

1.0


Installation
--------------

##### Front End Compilation

Install NodeJS (http://nodejs.org/)

- nmp install = Install all the packages needed for Grunt, karma, etc.
- bower install = Install all the javascript dependencies, e.g. Angular, jQuery, etc.
- grunt [build, karma, compile] = build the front end, run the unittest, compile the front end.

```sh
git clone git@git.santexgroup.com:net/octopus.git octopus
cd octopus
cd Octopus.FrontEnd
npm -g install grunt-cli karma bower
npm install
bower install
grunt build
```

##### BackEnd API Compilation

All the projects must have the version 4.5.1 of the .Net Framework

Run the folling from the Package Manager Console

Running the WebApi project will create the database and will run all the fixtures scripts, please be sure the change the web.config according your connection string.


Good Practices
----------------


* Add Summary documentation to the methods and classes.
* Add the corresponding unittests

Done Criteria
-------------
- Two +1 from other devs in the Code Review.
- Jenkins Build / Deployment
- Precence of UnitTest in both sides BackEnd and FrontEnd.
- +1 from QA
- Description and testing suggestions in the Code Review description.