# DataAnalysisSystem

DataAnalysisSystem is a comprehensive system providing mechanisms for data analysis. The application offers functionalities in the areas of data sets, data analysis, as well as visualization and sharing of their results. The distinguishing factor of DataAnalysisSystem is its well-thought-out architecture, which enables convenient extension of the functionalities available within the application, in particular the supported input formats for data sets and the set of available data analysis methods. The data analysis process itself has been designed in such a way that it can be carried out in a distributed manner.

The DataAnalysisSystem application is built using ASP.NET Core and Razor Pages. As a data store is used non-relational database MongoDB, which is located in the Mongo Atlas service. Access to the database is realized through the MongoDb Driver for C#. Authorisation and user authentication is built using ASP.NET Core Identity with an additional provider allowing to use the MongoDB database as a data store. The graphical interface is based on Materialize.css and Chart.js framework. The distributed processing system used in the data analysis process is the Actor-Model approach from the Akka.NET package.The data analysis methods used in the application come from the ML.NET and Math.NET Numerics libraries.The DataAnalysisSystem application is built using ASP.NET Core and Razor Pages. As a data store is used non-relational database MongoDB, which is located in the Mongo Atlas service. Access to the database is realized through the MongoDb Driver for C#. Authorisation and user authentication is built using ASP.NET Core Identity with an additional provider allowing to use the MongoDB database as a data store. The graphical interface is based on Materialize.css and Chart.js framework. The distributed processing system used in the data analysis process is the Actor-Model approach from the Akka.NET package.The data analysis methods used in the application come from the ML.NET and Math.NET Numerics libraries.

The application provides the following functionalities: 

1. Users and their accounts:
+ login
+ confirmation of email address
+ account registration
+ password reset
+ contact with the administrator
+ listing your personal data
+ editing personal data
+ change password
+ printing the contents of individual application panels

2. Data sets:
+ adding a new set of data
+ editing a data set
+ listing datasets uploaded by the user
+ exporting a dataset
+ displaying details of a dataset
+ displaying statistical data about a dataset
+ display of analyses performed on a dataset
+ deleting a dataset
+ listing datasets made available by the user
+ sharing a data set
+ cancelling the sharing process
+ downloading the access code to the shared data set
+ list of data sets not shared by the user
+ browsing through the data sets made available to the user
+ adding a new dataset made available to the user
+ displaying details of a data set made available to the user

3. Data analyses:
+ configuring the data analysis process
+ configuring methods used for data analysis
+ list of performed data analyses
+ displaying data analysis details
+ displaying received results for performed data analysis methods
+ display of configured parameters used for data analysis
+ deleting an executed data analysis
+ list of information about implemented data analysis methods
+ list of analyses made available by the user
+ opening a data analysis
+ cancelling the sharing of data analysis
+ getting the access code to the shared analysis
+ browsing through the data analyses made available to a user
+ adding a new data analysis made available to a user by using an access code
+ displaying details of a data analysis that is available to a user
+ list of analyses not shared by the user

Data sets saved in the following formats are supported: .json, .xls, .xlsx, .xml., .csv.

The following data analysis methods are available to the user:
+ Approximation
+ Basic Statistics
+ Deriverative
+ Histogram
+ K-Means Clustering
+ Regression

Access to selected functionalities is dependent on user status. A non-logged-in user has access only to informative functions and to the possibility of creating an account and using the password reset option. Users logged into their accounts have access to all areas of the designed system.

The following design patterns has been implemented in the application:
+ Model View Controller
+ Singleton
+ Dependency Injection
+ Repository
+ Chain of Responsibility
+ Command
+ Adapter
+ Facade
+ Strategy

Login form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_1.PNG "Login form")

Contact administrator form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_2.PNG "Contact administrator form")

Email received by administrator from system user:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_3.PNG "Question email")

Register user form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_4.PNG "Register user form")

Email confirmation message:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_5.PNG "Email confirmation message")

Main application menu:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_6.PNG "Main application menu")

Edit user data form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_7.PNG "Edit user form")

Change user password form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_8.PNG "Change user password")

Reset forgotten password - initial form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_9.PNG "Reset forgotten password - init form")

Reset forgotten password - confirmation email message:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_10.PNG "Reset forgotten password - confirmation email message")

Reset forgotten password - change form:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_11.PNG "Reset forgotten password - change form")

Add new dataset:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_12.PNG "Add new dataset")

Add new dataset - summary dataset content:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_13.PNG "Add new dataset - summary dataset content")

User datasets panel:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_14.PNG "User datasets panel")


Dataset details - overall information with control buttons:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_15.PNG "Dataset details - overall information")

Dataset details - dataset statistics section:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_16.PNG "Dataset details - dataset statistics section")

Dataset details - dataset content section:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_17.PNG "Dataset details - dataset content section")

Dataset details - print view:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_18.PNG "Dataset details - print view")

Dataset details - related analyses section:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_19.PNG "Dataset details - related analyses section")

Edit dataset panel:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_20.PNG "Edit dataset panel")

Delete dataset modal:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_21.PNG "Delete dataset modal")

Export dataset panel:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_22.PNG "Export dataset panel")

User shared datasets:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_23.PNG "User shared datasets")

User not shared datasets:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_24.PNG "User not shared datasets")

Shared dataset access info modal:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_25.PNG "Shared dataset access info modal")

Datasets shared to user browser:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_26.PNG "Datasets shared to user browser")

Available dataset analysis methods panel:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_27.PNG "Available dataset analysis methods panel")

Perform analysis panel:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_28.PNG "Perform analysis panel")

Perform analysis panel - configure methods parameters (Approximation and Basic Statistics Method):

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_29.PNG "Perform analysis panel - configure methods parameters for Approximation and Basic Statistics Method")

Perform analysis panel - configure methods parameters (Deriverative and Histogram Method):

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_30.PNG "Perform analysis panel - configure methods parameters for Deriverative and Histogram Method")


Perform analysis panel - configure methods parameters (K-Means Clustering and Regression Method):

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_31.PNG "Perform analysis panel - configure methods parameters for K-Means Clustering and Regression Method")

Perform analysis - class diagram:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_32.PNG "Perform analysis - class diagram")

User analyses panel:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_33.PNG "User analyses panel")

Analysis details panel - overall information and used methods:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_34.PNG "Analysis details panel - overall information and used methods")

Analysis details panel - Approximation Method parameters and obtained result:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_35.PNG "Analysis details panel - Approximation Method parameters and obtained result")

Analysis details panel - Basic Statistics Method parameters and obtained result:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_36.PNG "Analysis details panel - Basic Statistics Method parameters and obtained result")

Analysis details panel - Deriverative Method parameters and obtained result:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_37.PNG "Analysis details panel - Deriverative Method parameters and obtained result")

Analysis details panel - Histogram Method parameters and obtained result:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_38.PNG "Analysis details panel - Histogram Method parameters and obtained result")

Analysis details panel - K-Means Clustering Method parameters and obtained result:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_39.PNG "Analysis details panel - K-Means Clustering Method parameters and obtained result")

Analysis details panel - Regression Method parameters and obtained result:

![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_40.PNG "Analysis details panel - Regression Method parameters") <br/>
![alt text](https://github.com/Korag/DocumentationImages/blob/master/DataAnalysisSystem/DataAnalysisSystem_41.PNG "Analysis details panel - Regression Method obtained result")
