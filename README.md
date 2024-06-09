Wellcome to NoPro, our Project-management application!
This programm was created as the main project for an extensive .NET course

This application was built completely from the ground up. starting with the data layer API all the way through coding
the application's complex logic, and crafting the extensive GUI from scrach using WPF - NO external libraries(like material-design) were used! all built by hand!

Building an application from the ground up is a serious chalenge and this project especialy features plenty of
options and features which were above and byond the course requirements. for example: 
- Manging a system clock in real time including GUI updates (requires carefule multithreading configurations as WPF GUI works in a Single-thread - STA)
- Uploading PNG profile pictures feature for users (requires convertion to bit maps for storage in the data base)
- Dynamic, costum-made loading spinner
- The GUI was custom made entrely by hand. NO external libraries were used what so ever
- Much more
-----------------------------------------------------------------------------

Startup instructions:
1) Upon clonening the repository, make sure to set PL as the 'start up project' in order for the programm to boot
2) You can alwasy log in to the system with a pre-made user made for demonstration purposes:
    User name: test password:1234
3) After successful log-in, feel free to go over the use instruction in the application's home page (click the rectangle 'How to use').
   it is a very basic guide and does not cover all of the features, but it should help you understand the application's use cases
-----------------------------------------------------------------------------
About the programm:
- The programm was created from the ground up with the 3 layer model as the underlying architechture
- Incorperation of design patterns such as singleton, observer and factory
- Utilization of indastry level best design practices
- Data layer API, which allow for any data-base implementation for the application
  currently, there is a local XML data base for the application (the data will be stored in XML files at the client)
  adding more data-bases implementation like SQL is on my plan
