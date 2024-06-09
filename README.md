**Welcome to NoPro, our Project Management Application!**

NoPro was developed as the main project for an extensive .NET course. This application was built entirely from the ground up, starting with the data layer API, progressing through coding the application's complex logic, and culminating in the development of an extensive GUI from scratch using WPF. No external libraries (such as material-design) were used – everything was crafted by hand!

Creating an application from the ground up is a significant challenge, and our application features functionalities that go above and beyond the course requirements. Some of these include:

- Managing a system clock in real-time with GUI updates: This feature requires careful multithreading configurations, as the WPF GUI operates in a Single-Threaded Apartment (STA) model.
- Uploading PNG profile pictures: Users can upload profile pictures, which are converted to bitmaps for storage in the database.
- Dynamic, custom-made loading spinner: The application includes a loading spinner that was entirely custom-built.
- Handcrafted GUI: The GUI was custom made entirely by hand, with no external libraries used whatsoever.
- And much more!
-------------------------------------------------------------------------------------------------------

**Startup instructions:**
- Clone the repository: Make sure to set PL as the 'startup project' in order for the program to boot.
- Demo Login: You can always log in to the system with a pre-made user for demonstration purposes:
        Username: test
        Password: 1234
- Using the Application: After a successful login, feel free to review the use instructions on the application's home page (click the 'How to use' rectangle). This basic 
  guide does not covers all of the features but it will give you an understand of the application's primary use cases and functionalities.
-------------------------------------------------------------------------------------------------------

**About the Program: A Brief Overview of the Application's Structure**
- Three-Layer Architecture: The program was created from the ground up using a three-layer architecture model.
- Design Patterns: Incorporation of design patterns such as Singleton, Observer, and Factory.
- Industry-Level Best Practices: Utilization of industry-level best design practices.
- Data Layer API: The data layer API allows for any database implementation for the application. Currently, the application uses a local XML database (data is stored in XML 
  files on the client). Adding more database implementations, such as SQL, is planned for the future.
- And more!
