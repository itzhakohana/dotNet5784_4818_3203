namespace DalTest;
using DalApi;
using DO;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.Intrinsics.X86;


/// <summary>
/// Initializing the Data lists with random values
/// </summary>
public static class Initialization
{
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    private static readonly int s_tasksAmount = 20; //amount of Tasks created randomly 
    private static readonly int s_dependenciesAmount = 40; //amount of Dependencies created randomly 
    private static readonly int s_engineersAmount = 10;  //amount of Engineers created randomly 
    private static readonly int MIN_ID = 200000000;
    private static readonly int MAX_ID = 400000000;

    /// <summary>
    /// Initializes predecided amount of uniqe Engineers
    /// </summary>
    private static void creatEngineers()
    {
        string[] privateNames = new string[]
        {
            // Male Names           
            "John", "Landon", "Christian", "Jaxon", "Julian", "Levi", "Christopher", "Jonathan", "Colton", "Elias",
            "Anthony", "Gavin", "Isaiah", "Charles", "Austin", "Dominic", "Adrian", "Thomas", "Lincoln", "Leo",
            "Sebastian", "Hudson", "Brayden", "Eli", "Asher", "Cooper", "Jeremiah", "Jordan", "Ezekiel", "Angel",
            "Aaron", "Jameson", "Ian", "Nicholas", "Easton", "Ezra", "Jaxson", "Adam", "Micah", "Jose",
            "Carson", "Kayden", "Silas", "Jason", "Parker", "Xavier", "Jace", "Miles", "Sawyer", "Declan",
            "Bryson", "Greyson", "Weston", "Kevin", "Luis", "Blake", "George", "Ashton", "Nolan", "Atticus",
            "Sherlock", "Holden", "Frodo", "Dorian", "Gatsby", "Aragorn", "Draco", "Hannibal", "Thor",

            // Female Names           
            "Emilia", "Everly", "Leah", "Aubrey", "Willow", "Addison", "Lucy", "Audrey", "Bella",
            "Caroline", "Eliana", "Anna", "Maya", "Valentina", "Ruby", "Kennedy", "Ivy", "Ariana", "Aaliyah",
            "Cora", "Madelyn", "Alice", "Kinsley", "Hailey", "Gabriella", "Allison", "Gianna", "Serenity",
            "Samantha", "Sarah", "Autumn", "Quinn", "Eva", "Piper", "Sophie", "Sadie", "Delilah", "Josephine",
            "Nevaeh", "Adeline", "Arya", "Emery", "Lydia", "Clara", "Vivian", "Madeline", "Julia", "Peyton",
            "Rylee", "Reagan", "Liliana", "Melanie", "Mackenzie", "Hadley", "Raelynn", "Kaylee", "Sienna",
            "Adalynn", "Alaina", "Jasmine", "Scout", "Hermione", "Arwen", "Daenerys", "Katniss", "Ophelia",
            "Eowyn", "Luna", "Leia", "Sansa",

            // New Unique and Interesting/Funny Reference Names
            "Yoda", "Morpheus", "Neo", "Trinity", "Gandalf", "Bilbo", "Legolas", "Elrond", "Galadriel", "Samwise",
            "Pippin", "Merry", "Boromir", "Eowyn", "Gimli", "Eomer", "Faramir", "Theoden", "Gollum", "Smeagol",
            "Saruman", "Celeborn", "Tom", "Goldberry", "Treebeard", "Shadowfax", "Balin", "Dwalin", "Kili",
            "Fili", "Dori", "Nori", "Ori", "Bifur", "Bofur", "Bombur", "Thorin", "Bard", "Thranduil", "Smaug",
            "Beorn", "Radagast", "Shelob", "Denethor", "Gothmog", "Erestor", "Glorfindel", "Cirdan", "Gil-galad",
            "Elendil", "Isildur", "Anarion", "Beren", "Luthien", "Feanor"
        };

        string[] surnames = new string[]
        {
            // Common Surnames
            "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
            "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
            "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts",
            "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", "Cruz", "Edwards", "Collins", "Reyes",
            "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers", "Gutierrez", "Ortiz", "Morgan", "Cooper",
            "Peterson", "Bailey", "Reed", "Kelly", "Howard", "Ramos", "Kim", "Cox", "Alexander", "Ward",
            "Richardson", "Watson", "Brooks", "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza", "Ruiz",
            "Hughes", "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers", "Long", "Ross", "Foster",            

            // Unique and Interesting/Funny Reference Surnames
            "Skywalker", "Stark", "Potter", "Dumbledore", "Holmes", "Wayne", "Bond", "Solo", "Baggins", "Lannister", "Targaryen",
            "Malfoy", "Grey", "Gatsby", "Vader", "Kenobi", "Granger", "Snape", "Thorin", "Spock", "Kirk", "Luthar", "Ninefingers",
            "Picard", "Sulu", "McFly", "Tannen", "Hannibal", "Rocky", "Balboa", "Munson", "Lebowski", "Biggs", "Oakenshield",
            "Strange", "Banner", "Riddle", "Weasley", "Dursley", "Durden", "Gump", "Wick", "Corleone", "Ripley",
            "Soprano", "Banks", "Draper", "Bourne", "Rambo", "McClane", "Thatcher", "Fisher", "Travolta", "Schrute"
        };

        int emailFiller = 1000;

        for (int i = 0; i < s_engineersAmount; i++) //randomizing 10 Engineers
        {

            //randomizing ID number (range: 200000000 to 400000000)
            int randId;
            do
            {
                randId = s_rand.Next(MIN_ID, MAX_ID); 
            } while (s_dal!.Engineer.Read(randId) != null);

            //Randomly assembles a name from the two arrays
            int firstNameIndex, surNameIndex;
            string randName;
            do
            {
                firstNameIndex = s_rand.Next(privateNames.Length);
                surNameIndex = s_rand.Next(surnames.Length);
                randName = (privateNames[firstNameIndex] + " " + surnames[surNameIndex]); 
            } while (s_dal!.Engineer.Read(e => e.Name == randName) != null);

            //setting email address based on the randomized name
            string randEmail = (surnames[surNameIndex] + (s_rand.Next(emailFiller, emailFiller += 10)) + "@gmail.com");

            //randomizing expertise level
            int myComlexity = s_rand.Next(1, 6);
            DO.EngineerExperience randLevel = (DO.EngineerExperience)myComlexity;

            //randomizing phone number
            string randPhone = ("050" + s_rand.Next(1000000, 9999999).ToString());

            //creating and adding a new Engineer to the database
            Engineer myEngineer = new Engineer(randId, randLevel, randName, randEmail, randPhone, s_rand.Next(10,200));
            s_dal!.Engineer.Create(myEngineer);

        }
    }

    /// <summary>
    /// Initializes predecided amount of unique Dependencies
    /// </summary>
    private static void creatDependencies() 
    {
        List<Task?> listCopy = (List<Task?>)s_dal!.Task.ReadAll().ToList();
        int quarter = listCopy.Count / 4;
        int half = listCopy.Count / 2;
        for (int i = 0; i < (s_dependenciesAmount / 4); i++) //randomizing 40 Dependencies
        { 
            //randomly picking a dependent-ON-task from the task-list
            int randListIndex = s_rand.Next(0, quarter);
            Task depOnTask = listCopy.ElementAt(randListIndex)!;
            int depOnTaskId = depOnTask.Id;

            //randomly picking a dependent-task from the task-list
            randListIndex = s_rand.Next(quarter, half);
            Task depTask = listCopy.ElementAt(randListIndex)!;
            int depTaskId = depTask.Id;

            //creating and adding a new Dependency to the database
            Dependency myDependency = new Dependency(0, depTaskId, depOnTaskId);
            s_dal!.Dependency.Create(myDependency);
        }
        for (int i = s_dependenciesAmount / 4; i < (2 * (s_dependenciesAmount / 4)); i++) 
        {

            //randomly picking a dependent-ON-task from the task-list
            int randListIndex = s_rand.Next(quarter, half);
            Task depOnTask = listCopy.ElementAt(randListIndex)!;
            int depOnTaskId = depOnTask.Id;

            //randomly picking a dependent-task from the task-list
            randListIndex = s_rand.Next(half, 3 * quarter);
            Task depTask = listCopy.ElementAt(randListIndex)!;
            int depTaskId = depTask.Id;

            //creating and adding a new Dependency to the database
            Dependency myDependency = new Dependency(0, depTaskId, depOnTaskId);
            s_dal!.Dependency.Create(myDependency);

        }
        for (int i = s_dependenciesAmount / 2; i < s_dependenciesAmount; i++)
        {
            //randomly picking a dependent-ON-task from the task-list
            int randListIndex = s_rand.Next(half, half + quarter);
            Task depOnTask = listCopy.ElementAt(randListIndex)!;
            int depOnTaskId = depOnTask.Id;

            //randomly picking a dependent-task from the task-list
            randListIndex = s_rand.Next(half + quarter, listCopy.Count);
            Task depTask = listCopy.ElementAt(randListIndex)!;
            int depTaskId = depTask.Id;

            //creating and adding a new Dependency to the database
            Dependency myDependency = new Dependency(0, depTaskId, depOnTaskId);
            s_dal!.Dependency.Create(myDependency);

        }
    }

    /// <summary>
    /// Initializes predecided amount of unique tasks
    /// </summary>
    private static void creatTasks()
    {
        string[] tasks = new string[]
        {
            "Codebase Refactoring", "Bug Squashing", "Feature Implementation", "Unit Testing", "Integration Testing",
            "Deployment Orchestration", "Documentation Overhaul", "Performance Optimization", "Database Design",
            "API Creation", "Backend Development", "Frontend Development", "Service Integration", "CI Setup", "CD Setup",
            "Prototype Building", "Mockup Creation", "Script Writing", "Quality Assurance", "System Architecture Design",
            "Server Management", "System Monitoring", "Error Handling", "Codebase Optimization", "Middleware Development",
            "Component Creation", "Environment Setup", "Network Configuration", "Authentication Handling", "UI Design",
            "UI Development", "Code Review Conducting", "Security Issue Fixing", "Dependency Updating", "API Documentation",
            "Dependency Management", "Update Deployment", "Application Debugging", "Code Optimization", "Unit Test Writing",
            "Integration Test Writing", "E2E Test Writing", "Load Testing", "Database Management", "Query Optimization",
            "Exception Handling", "Dashboard Creation", "Caching Implementation", "Load Balancer Setup", "Load Time Optimization",
            "User Story Creation", "Pull Request Reviewing", "Technical Research", "User Flow Design", "Requirement Analysis",
            "User Guide Writing", "Sprint Planning", "Wireframe Creation", "Feature Testing", "Usability Testing",
            "Cloud Resource Management", "Cloud Service Deployment", "Cloud Service Monitoring", "Cloud Cost Optimization",
            "Microservices Implementation", "RESTful API Development", "GraphQL Implementation", "Webhook Creation",
            "Middleware Implementation", "OAuth Implementation", "Microservices Design", "Microservices Development",
            "Kubernetes Implementation", "Docker Setup", "Container Management", "State Management", "Mobile App Development",
            "Responsive Design Creation", "Mobile Performance Optimization", "Documentation Writing", "Architecture Review",
            "Schema Design", "Storage Optimization", "CLI Tool Development", "Shell Script Writing", "API Gateway Management",
            "SSO Implementation", "User Authentication Creation", "User Permissions Design", "Concurrency Handling",
            "Code Merging", "Build Time Optimization", "CI/CD Pipeline Management", "Serverless Function Implementation",
            "Serverless Application Monitoring", "Serverless Cost Optimization", "Deployment Script Creation", "Firewall Configuration",
            "Data Encryption Implementation", "Backup Script Writing", "Disaster Recovery Plan Design", "Code Refactoring",
            "Feature Toggle Implementation", "AB Testing", "Analytics Tool Development", "Data Processing Optimization",
            "Data Pipeline Creation", "Machine Learning Implementation", "AI Model Development", "Data Cleaning",
            "Data Visualization Implementation", "BI Report Creation", "BI Tool Development", "Data Storage Optimization",
            "Big Data Management", "Event Sourcing Implementation", "CQRS Development", "Data Sharding Handling",
            "Data Replication Implementation", "User Interface Creation", "User Interface Design", "User Experience Optimization",
            "User Research Conducting", "Prototype Creation", "Prototype Development", "Test Plan Writing",
            "Regression Testing", "Functional Testing", "System Testing", "UAT Conducting", "Feature Flag Implementation",
            "SDK Development", "Library Creation", "Web Security Implementation", "Cross-Site Scripting Prevention",
            "SQL Injection Prevention", "Web Server Configuration", "Web Server Monitoring", "Web Performance Optimization",
            "CSS Writing", "HTML Template Development", "Web Accessibility Handling", "Responsive Layout Creation",
            "Progressive Web App Development", "Service Worker Implementation", "Web Asset Optimization", "CDN Implementation",
            "Custom Theme Creation", "E-commerce Feature Development", "SEO Optimization", "Meta Tag Writing", "Web Animation Development",
            "Web Font Handling", "Web Form Creation", "Web Form Validation", "Payment System Integration", "Shopping Cart Development",
            "User Tracking Implementation", "Session Management", "Social Media Integration", "Chatbot Creation",
            "Push Notification Implementation", "Notification System Creation", "Push Notification Optimization",
            "Web Socket Implementation", "Real-Time Feature Development", "File Upload Handling", "Image Gallery Creation",
            "Video Player Development", "Audio Player Implementation", "Media Playback Optimization", "Slider Creation",
            "Carousel Development", "Lazy Loading Implementation", "Scroll Event Handling", "Custom Widget Creation",
            "Web API Development", "Web Dashboard Creation", "Web Dashboard Optimization", "API Rate Limiting",
            "Webhook Implementation", "Serverless API Creation", "Serverless Function Development", "Serverless Performance Optimization",
            "Serverless Function Monitoring", "Error Log Writing", "Logging System Creation", "Monitoring Tool Implementation",
            "Alert Handling", "Incident Response Plan Development", "Incident Response Optimization", "Incident Report Creation"
        };

        string[] descriptions = new string[]
        {
            "Tidying up the code like cleaning your room, but less vacuuming involved.",
            "Hunting down and eliminating bugs, like an exterminator with a keyboard.",
            "Adding shiny new features to make users go 'ooh' and 'ahh'.",
            "Ensuring every piece of code behaves, like a strict but fair schoolteacher.",
            "Making sure different parts of the code play nice together.",
            "Sending the latest and greatest code into the wild world of production.",
            "Turning chaos into clarity, one page of documentation at a time.",
            "Making the app faster than your morning coffee run.",
            "Organizing data in tables, because even databases appreciate a good table setting.",
            "Building bridges for software to talk to each other without shouting.",
            "Crafting the server-side magic behind the scenes of your favorite apps.",
            "Creating the shiny interface that users interact with, because looks do matter.",
            "Getting different systems to shake hands and be friends.",
            "Setting up continuous integration, so your codebase is always ready for the runway.",
            "Automating deployment because manual labor is so last century.",
            "Bringing abstract ideas to life, like a magician with code.",
            "Designing digital blueprints that are almost as pretty as they are functional.",
            "Writing those nifty little scripts that make everything run smoother.",
            "Ensuring that everything works perfectly, because almost doesn't count.",
            "Designing the big picture, because someone has to be the architect.",
            "Keeping servers happy and healthy, like a digital zookeeper.",
            "Watching over systems to catch problems before they become disasters.",
            "Catching and handling errors before they spiral out of control.",
            "Making the codebase faster, leaner, and more elegant.",
            "Creating the middle layer that holds everything together, like glue but cooler.",
            "Building reusable pieces of functionality, like Lego blocks for software.",
            "Setting up environments where code can live, grow, and thrive.",
            "Configuring networks so that data flows as smoothly as possible.",
            "Ensuring that only the right people get in, like a bouncer for your app.",
            "Crafting the visual design that users will love and remember.",
            "Building the interactive elements users will actually click on.",
            "Reviewing code changes like a meticulous editor with an eye for detail.",
            "Fixing security vulnerabilities before they become news headlines.",
            "Keeping the project up-to-date with the latest and greatest dependencies.",
            "Documenting APIs so others can understand your genius.",
            "Managing dependencies so your project doesn’t turn into a tangled mess.",
            "Rolling out new updates without breaking a sweat or anything else.",
            "Finding and fixing issues before users do.",
            "Making the code not just work, but work beautifully.",
            "Ensuring every function, method, and module behaves as expected.",
            "Testing how different parts of the application interact under pressure.",
            "Simulating user journeys from start to finish to catch hidden issues.",
            "Ensuring the app can handle a crowd without breaking a sweat.",
            "Maintaining databases so they run smoothly and efficiently.",
            "Speeding up database queries because nobody likes to wait.",
            "Gracefully managing the unexpected so your app doesn’t panic.",
            "Creating visual dashboards that make data look good and easy to understand.",
            "Storing frequently used data so it can be accessed quickly.",
            "Balancing the load across servers to keep everything running smoothly.",
            "Reducing load times because fast apps make happy users.",
            "Writing user stories that capture the essence of what users need.",
            "Reviewing pull requests like a digital detective, ensuring quality.",
            "Digging into new tech and trends to stay ahead of the curve.",
            "Designing seamless user flows that feel as natural as a morning coffee.",
            "Breaking down requirements into manageable tasks, like a pro organizer.",
            "Creating guides that even your grandma could understand.",
            "Planning sprints like a marathon runner mapping out the race.",
            "Sketching wireframes to visualize the product before it’s built.",
            "Testing new features to make sure they work as advertised.",
            "Getting real user feedback to make sure the app is intuitive.",
            "Managing cloud resources efficiently to avoid runaway bills.",
            "Deploying cloud services like setting up a high-tech picnic.",
            "Monitoring cloud services to ensure they’re running at peak performance.",
            "Cutting cloud costs like a savvy shopper during sales season.",
            "Breaking down monoliths into microservices, making them easier to handle.",
            "Building APIs that are as elegant as they are functional.",
            "Setting up GraphQL for more efficient data queries.",
            "Creating webhooks to automate notifications and updates.",
            "Writing middleware that keeps everything running smoothly in between.",
            "Implementing OAuth to keep user data secure and private.",
            "Designing microservices to keep the codebase modular and scalable.",
            "Developing microservices to handle specific tasks like a team of specialists.",
            "Orchestrating Kubernetes to keep containers in perfect harmony.",
            "Setting up Docker to containerize applications for easy deployment.",
            "Managing containers to ensure they run smoothly and efficiently.",
            "Handling state management so the app remembers everything it needs to.",
            "Developing mobile apps that fit in the palm of your hand.",
            "Creating designs that look great on any device.",
            "Making mobile apps faster and more efficient.",
            "Writing documentation that’s clear, concise, and sometimes even witty.",
            "Reviewing architecture to ensure everything is solid and scalable.",
            "Designing schemas to organize data effectively.",
            "Optimizing storage to make the most of available space.",
            "Building command-line tools for power users.",
            "Writing shell scripts to automate tedious tasks.",
            "Managing API gateways to control and monitor traffic.",
            "Setting up SSO for seamless, secure logins.",
            "Creating robust user authentication systems.",
            "Designing user permissions to control access effectively.",
            "Handling concurrency to avoid race conditions and data corruption.",
            "Merging code like a pro to keep the project moving forward.",
            "Speeding up build times so you can get back to coding.",
            "Managing CI/CD pipelines to automate the boring parts.",
            "Implementing serverless functions to streamline deployment.",
            "Keeping an eye on serverless applications to catch issues early.",
            "Optimizing serverless costs to save money without sacrificing performance.",
            "Writing scripts to automate deployments, saving time and effort.",
            "Configuring firewalls to keep the bad guys out.",
            "Encrypting data to keep it safe from prying eyes.",
            "Creating backup scripts to ensure no data is lost.",
            "Designing plans for when things go wrong, because they will.",
            "Refactoring code to make it cleaner, faster, and more maintainable.",
            "Using feature toggles to control new functionality rollout.",
            "Running A/B tests to see what works best for users.",
            "Building tools to track and analyze user interactions.",
            "Optimizing data processing for speed and efficiency.",
            "Creating pipelines to move data smoothly and reliably.",
            "Implementing machine learning to make software smarter.",
            "Developing AI models to predict and analyze.",
            "Cleaning data so it’s ready for analysis.",
            "Visualizing data to make complex information easy to understand.",
            "Creating reports that turn data into insights.",
            "Developing BI tools to help make better business decisions.",
            "Optimizing data storage to keep everything running smoothly.",
            "Managing big data to handle massive amounts of information.",
            "Implementing event sourcing to track changes efficiently.",
            "Developing CQRS to separate read and write operations.",
            "Handling data sharding to distribute data across databases.",
            "Implementing data replication to ensure data availability.",
            "Creating beautiful user interfaces that users will love.",
            "Designing user interfaces that are both functional and attractive.",
            "Making user experiences as smooth and enjoyable as possible.",
            "Conducting research to understand what users need and want.",
            "Creating prototypes to test ideas quickly.",
            "Developing prototypes to bring concepts to life.",
            "Writing detailed test plans to cover all scenarios.",
            "Conducting regression tests to ensure nothing broke.",
            "Performing functional tests to verify feature functionality.",
            "Conducting system tests to ensure everything works together.",
            "Carrying out user acceptance testing to get the green light.",
            "Using feature flags to manage feature rollouts effectively.",
            "Developing SDKs to make your tools easy for others to use.",
            "Creating libraries that others can rely on.",
            "Implementing web security to protect against threats.",
            "Preventing cross-site scripting to keep data safe.",
            "Thwarting SQL injection attempts to protect databases.",
            "Configuring web servers to run smoothly and securely.",
            "Monitoring web servers to catch issues before users do.",
            "Optimizing web performance to make sites lightning fast.",
            "Writing CSS to style web pages beautifully.",
            "Developing HTML templates for consistent layouts.",
            "Ensuring web accessibility so everyone can use your site.",
            "Creating responsive layouts that adapt to any screen size.",
            "Developing progressive web apps for a native app experience.",
            "Implementing service workers for offline functionality.",
            "Optimizing web assets to improve load times.",
            "Setting up CDNs to deliver content quickly.",
            "Creating custom themes to make sites unique.",
            "Developing e-commerce features to drive sales.",
            "Optimizing SEO to improve search rankings.",
            "Writing meta tags to help search engines understand your content.",
            "Creating web animations to make sites more dynamic.",
            "Handling web fonts for better typography.",
            "Creating web forms for user input, making data collection a breeze.",
            "Validating web forms to ensure users provide the right information.",
            "Integrating payment systems for seamless transactions.",
            "Developing shopping carts to enhance e-commerce experiences.",
            "Implementing user tracking to gather useful analytics.",
            "Managing session state to keep user data consistent.",
            "Integrating social media to increase engagement.",
            "Creating chatbots to provide instant support and interaction.",
            "Setting up push notifications to keep users informed.",
            "Building notification systems that are both helpful and non-intrusive.",
            "Optimizing push notifications for better user engagement.",
            "Implementing web sockets for real-time communication.",
            "Developing real-time features to keep data fresh and current.",
            "Handling file uploads securely and efficiently.",
            "Creating image galleries to showcase visual content.",
            "Developing video players for seamless media playback.",
            "Implementing audio players for great sound experiences.",
            "Optimizing media playback for smooth, uninterrupted viewing.",
            "Creating sliders to display content interactively.",
            "Building carousels to cycle through content attractively.",
            "Implementing lazy loading to improve page load times.",
            "Handling scroll events to create dynamic user experiences.",
            "Creating custom widgets to enhance functionality.",
            "Developing web APIs for smooth data exchanges.",
            "Creating web dashboards for data visualization.",
            "Optimizing web dashboards to be both informative and efficient.",
            "Implementing API rate limiting to prevent overloads.",
            "Creating webhooks to trigger actions automatically.",
            "Building serverless APIs for scalable and efficient performance.",
            "Developing serverless functions to handle background tasks.",
            "Optimizing serverless performance for cost efficiency.",
            "Monitoring serverless functions to catch and resolve issues.",
            "Writing error logs to keep track of application issues.",
            "Creating logging systems for better debugging and maintenance.",
            "Implementing monitoring tools to ensure everything runs smoothly.",
            "Handling alerts to stay on top of potential issues.",
            "Developing incident response plans for quick recovery.",
            "Optimizing incident response to minimize downtime.",
            "Creating incident reports to document and learn from issues."
        };

        string[] remarks = new string[]
        {
            "Remember, a tidy codebase is a happy codebase!",
            "Bug busting: more satisfying than bubble wrap.",
            "Bringing features to life, one line of code at a time.",
            "Tests: the unsung heroes of quality code.",
            "Because who doesn't love a good team player?",
            "Launch day is like sending your kid to college. Exciting and terrifying!",
            "Documentation: because mind reading isn’t a feature (yet).",
            "Making the app faster than your grandma’s dial-up.",
            "Designing databases with more love than a barista making latte art.",
            "APIs: the bridges to software utopia.",
            "Backend: where the real magic happens.",
            "Frontend: making sure users see the magic.",
            "Service integration: making the whole greater than the sum of its parts.",
            "CI: because broken builds are so last century.",
            "CD: delivering code faster than pizza.",
            "Prototyping: where ideas meet reality.",
            "Mockups: the appetizer before the main course.",
            "Scripts: automation’s best friend.",
            "QA: because perfection is a journey, not a destination.",
            "Architecting systems: more satisfying than Lego.",
            "Servers: because your app needs a home too.",
            "Monitoring: always watching, never judging.",
            "Errors are inevitable, but panic is optional.",
            "Making the codebase sleek and chic.",
            "Middleware: the secret sauce of seamless operations.",
            "Components: building blocks of your app’s future.",
            "Environments: the stage for your code’s debut.",
            "Networks: ensuring data flows smoother than a jazz solo.",
            "Authentication: your app’s bouncer, keeping the riffraff out.",
            "UI: where form meets function with style.",
            "Bringing the user interface to life, one pixel at a time.",
            "Code reviews: where constructive criticism meets your ego.",
            "Security fixes: because hackers don’t take holidays.",
            "Keeping dependencies fresher than your morning coffee.",
            "APIs are only as good as their docs.",
            "Managing dependencies so your project stays zen.",
            "Deploying updates: controlled chaos at its finest.",
            "Debugging: the detective work of programming.",
            "Optimizing code like a fine-tuned engine.",
            "Unit tests: the building blocks of trust.",
            "Integration tests: making sure everyone plays nice.",
            "End-to-end tests: simulating the real world, but without the traffic.",
            "Load testing: preparing for the unexpected crowd.",
            "Databases: the unsung heroes of data storage.",
            "Speeding up queries so you can get back to Netflix.",
            "Handling exceptions with the grace of a ballet dancer.",
            "Dashboards: making data dance on screen.",
            "Caching: the underappreciated hero of speed.",
            "Load balancers: the jugglers of the server world.",
            "Load times: the faster, the better!",
            "User stories: because every feature has a tale.",
            "Pull requests: the modern handshake of developers.",
            "Staying ahead of the curve, one research paper at a time.",
            "User flows: the river guiding users to their destination.",
            "Breaking down tasks like a pro project manager.",
            "User guides: the maps to your app’s treasure.",
            "Sprints: turning marathon ideas into quick wins.",
            "Wireframes: where dreams take shape.",
            "Testing: the last line of defense before launch.",
            "Usability tests: making sure users don’t rage quit.",
            "Cloud management: making sure we don’t blow the budget.",
            "Cloud deployments: as smooth as a silk scarf.",
            "Cloud monitoring: keeping an eye on those fluffy servers.",
            "Saving on cloud costs, so there’s more for coffee.",
            "Microservices: because small is the new big.",
            "RESTful APIs: making data flow like poetry.",
            "GraphQL: query efficiency on steroids.",
            "Webhooks: automating the little things.",
            "Middleware: the unsung hero of app architecture.",
            "OAuth: keeping user data under lock and key.",
            "Microservices: modularizing for the win.",
            "Microservices development: teamwork makes the dream work.",
            "Kubernetes: container orchestration without the headaches.",
            "Docker: shipping code like a pro.",
            "Containers: keeping apps tidy and portable.",
            "State management: making sure the app doesn’t lose its mind.",
            "Mobile apps: pocket-sized powerhouses.",
            "Responsive design: beauty that fits every screen.",
            "Mobile performance: because no one likes a sluggish app.",
            "Documentation: clarity, one page at a time.",
            "Architecture reviews: building castles in the code.",
            "Schemas: organizing data with precision and care.",
            "Storage optimization: more space for your digital knick-knacks.",
            "CLI tools: for when the mouse just isn’t fast enough.",
            "Shell scripts: automating like a boss.",
            "API gateways: the gatekeepers of your data.",
            "SSO: one login to rule them all.",
            "User authentication: security starts here.",
            "User permissions: granting access like a digital doorman.",
            "Concurrency: keeping the threads from tangling.",
            "Merging code: the ultimate test of teamwork.",
            "Speeding up builds: because nobody likes waiting.",
            "CI/CD: the automation dream team.",
            "Serverless functions: lightweight, fast, and ready to roll.",
            "Serverless monitoring: keeping an eye on the ethereal.",
            "Serverless cost-saving: budget-friendly and efficient.",
            "Deployment scripts: automating the mundane.",
            "Firewalls: your app’s first line of defense.",
            "Data encryption: because privacy matters.",
            "Backup scripts: because data loss is not an option.",
            "Disaster recovery: planning for the worst, hoping for the best.",
            "Code refactoring: turning spaghetti into gourmet code.",
            "Feature toggles: flipping features on and off like a switch.",
            "A/B testing: science for better user experiences.",
            "Analytics tools: turning data into decisions.",
            "Data processing: faster than a caffeinated coder.",
            "Data pipelines: moving data with grace and speed.",
            "Machine learning: teaching machines to be clever.",
            "AI models: because predicting the future is cool.",
            "Data cleaning: tidying up for better insights.",
            "Data visualization: making numbers look good.",
            "BI reports: turning data into storytelling.",
            "BI tools: helping businesses make smart moves.",
            "Data storage: because every byte matters.",
            "Big data: handling the heavy lifting of information.",
            "Event sourcing: tracking changes like a detective.",
            "CQRS: splitting the load for better performance.",
            "Data sharding: spreading the love (and data).",
            "Data replication: keeping copies safe and sound.",
            "UI creation: making screens that users love.",
            "UI design: blending form and function.",
            "User experience: smoother than a jazz tune.",
            "User research: digging deep to find the gold.",
            "Prototype creation: testing ideas in the real world.",
            "Prototype development: bringing concepts to life.",
            "Test plans: your roadmap to bug-free code.",
            "Regression testing: making sure old friends don’t become new enemies.",
            "Functional testing: ensuring features work like a charm.",
            "System testing: the final check before going live.",
            "User acceptance: getting the thumbs up from users.",
            "Feature flags: turning features on and off with ease.",
            "SDK development: tools for your developer friends.",
            "Library creation: reusable code at its finest.",
            "Web security: keeping the bad guys at bay.",
            "Cross-site scripting: blocking sneaky scripts.",
            "SQL injection: safeguarding your database from intruders.",
            "Web server setup: the backbone of the internet.",
            "Web server monitoring: keeping things running smoothly.",
            "Web performance: speed thrills, and we deliver.",
            "CSS writing: styling the web with flair.",
            "HTML templates: crafting the web’s skeleton.",
            "Web accessibility: inclusivity starts here.",
            "Responsive layouts: making websites fit every screen.",
            "Progressive web apps: the best of both worlds.",
            "Service workers: offline functionality, online convenience.",
            "Web asset optimization: faster sites, happier users.",
            "CDN setup: delivering content at lightning speed.",
            "Custom themes: giving websites a unique personality.",
            "E-commerce features: boosting online sales seamlessly.",
            "SEO optimization: climbing the search engine ranks.",
            "Meta tag writing: helping search engines understand your site.",
            "Web animations: adding a bit of magic.",
            "Web fonts: perfecting typography online.",
            "Web forms: collecting data with ease.",
            "Form validation: ensuring users fill out forms correctly.",
            "Payment integration: smooth and secure transactions.",
            "Shopping cart development: making online shopping a breeze.",
            "User tracking: gaining insights without being creepy.",
            "Session management: keeping user data consistent.",
            "Social media integration: increasing engagement with ease.",
            "Chatbot creation: providing instant support, 24/7.",
            "Push notification setup: keeping users in the loop.",
            "Notification systems: gentle reminders, not annoyances.",
            "Push notification optimization: engaging users the right way.",
            "Web socket setup: real-time communication made easy.",
            "Real-time feature development: keeping data fresh.",
            "File upload handling: secure and smooth uploads.",
            "Image gallery creation: showcasing visual delights, one click at a time.",
            "Video player development: bringing moving pictures to life.",
            "Audio player implementation: the sound of smooth functionality.",
            "Media playback optimization: because waiting is so last century.",
            "Slider creation: sliding into a better user experience.",
            "Carousel development: spinning up interactive content.",
            "Lazy loading: because who has time to wait?",
            "Scroll event handling: navigating the digital seas with ease.",
            "Custom widget creation: adding a touch of uniqueness.",
            "Web API development: where data meets the internet.",
            "Web dashboard creation: making data-driven decisions easier.",
            "Web dashboard optimization: because every pixel counts.",
            "API rate limiting: keeping the floodgates under control.",
            "Webhook implementation: automating with style.",
            "Serverless API creation: lightweight and powerful.",
            "Serverless function development: functions that pack a punch.",
            "Serverless performance optimization: making efficiency an art form.",
            "Serverless monitoring: keeping an eye on the ethereal.",
            "Error log writing: because every error has a story.",
            "Logging systems: keeping tabs on the code’s adventures.",
            "Monitoring tools: the watchful guardians of your app.",
            "Alert handling: notifying you before things go south.",
            "Incident response plans: because accidents happen.",
            "Incident response optimization: turning chaos into order.",
            "Incident report creation: learning from mistakes, one incident at a time."
            };

        string[] deliverables = new string[]
        {
            "A codebase shinier than a freshly polished diamond!",
            "Bugs squashed harder than a pancake under a steamroller!",
            "Features implemented smoother than a jazz saxophonist's solo!",
            "Unit tests written tighter than a toddler's grip on a cookie jar!",
            "Integration tests executed faster than a cheetah chasing its lunch!",
            "Deployments orchestrated more flawlessly than a symphony conductor!",
            "Documentation overhauled with more love than a grandma's secret recipe!",
            "Performance optimized to rival a cheetah on caffeine!",
            "Database design more solid than a rock concert in an earthquake!",
            "APIs created cooler than the flip side of the pillow!",
            "Backend developed stronger than a bodybuilder's biceps!",
            "Frontend developed smoother than a dolphin gliding through water!",
            "Services integrated tighter than a pair of skinny jeans!",
            "CI setup smoother than butter on a warm biscuit!",
            "CD setup tighter than a squirrel stashing its nuts for winter!",
            "Prototypes built faster than a kid's Lego masterpiece!",
            "Mockups created with more style than a fashion runway!",
            "Scripts written funnier than a stand-up comedian's jokes!",
            "Quality Assurance tighter than a grandma's hugs!",
            "System architecture designed more meticulously than a Swiss watch!",
            "Servers managed better than a restaurant with a 5-star chef!",
            "System monitoring sharper than a hawk's gaze!",
            "Errors handled smoother than a seasoned magician's tricks!",
            "Codebase optimized with more precision than a Swiss Army knife!",
            "Middleware developed cooler than the other side of the pillow!",
            "Components created sturdier than a tank!",
            "Environments setup quicker than a pizza delivery!",
            "Networks configured tighter than a spider's web!",
            "Authentication handled more securely than Fort Knox!",
            "UI designed prettier than a picture-perfect sunset!",
            "UI developed smoother than a baby's bottom!",
            "Code reviews conducted more cheerfully than a birthday party!",
            "Security issues fixed faster than a ninja's strike!",
            "Dependencies updated fresher than a farmer's market produce!",
            "APIs documented clearer than crystal!",
            "Dependencies managed tighter than a ship's cargo!",
            "Updates deployed smoother than a hot knife through butter!",
            "Application debugging more entertaining than a detective novel!",
            "Code optimization snappier than a racecar at the finish line!",
            "Unit tests written more creatively than a poet's verses!",
            "Integration tests written more harmoniously than a choir!",
            "E2E tests written more comprehensively than a best-selling novel!",
            "Load testing tougher than a boot camp!",
            "Database management smoother than a ballroom dance!",
            "Queries optimized faster than a Formula 1 pit stop!",
            "Exceptions handled more gracefully than a ballerina!",
            "Dashboards created fancier than a royal palace!",
            "Caching implemented more effectively than a squirrel hoarding nuts!",
            "Load balancer setup smoother than a silk scarf!",
            "Load time optimization quicker than a cheetah on caffeine!",
            "User stories created more engagingly than a blockbuster movie!",
            "Pull requests reviewed friendlier than a family reunion!",
            "Technical research conducted more passionately than a treasure hunt!",
            "User flow design smoother than a gentle breeze!",
            "Requirement analysis deeper than the ocean!",
            "User guide writing more engaging than a mystery novel!",
            "Sprint planning more exciting than planning a vacation!",
            "Wireframes created clearer than a cloudless sky!",
            "Feature testing more thrilling than a roller coaster ride!",
            "Usability testing more insightful than a Zen master!",
            "Cloud resource management more efficient than a Swiss watch!",
            "Cloud service deployment quicker than lightning!",
            "Cloud service monitoring sharper than a hawk's gaze!",
            "Cloud cost optimization more budget-friendly than a clearance sale!",
            "Microservices implementation smoother than a hot knife through butter!",
            "RESTful API development more streamlined than a bullet train!",
            "GraphQL implementation cooler than the other side of the pillow!",
            "Webhook creation snazzier than a jazz club!",
            "Middleware implementation smoother than a silk scarf!",
            "OAuth implementation more secure than a fortress!",
            "Microservices design cooler than the flip side of the pillow!",
            "Microservices development smoother than a baby's bottom!",
            "Kubernetes implementation quicker than a flash!",
            "Docker setup tighter than a drum!",
            "Container management smoother than a glass of fine wine!",
            "State management handled more gracefully than a ballerina!",
            "Mobile app development faster than a speeding bullet!",
            "Responsive design creation sleeker than a sports car!",
            "Mobile performance optimization quicker than a Formula 1 pit stop!",
            "Documentation writing more engaging than a mystery novel!",
            "Architecture review more thorough than an eagle-eyed detective!",
            "Schema design more solid than a brick house!",
            "Storage optimization more efficient than a Swiss watch!",
            "CLI tool development snappier than a whip crack!",
            "Shell script writing funnier than a stand-up comedy show!",
            "API gateway management smoother than a breeze!",
            "SSO implementation more secure than a bank vault!",
            "User authentication creation more reliable than a Swiss watch!",
            "User permissions design tighter than Fort Knox!",
            "Concurrency handling smoother than a well-oiled machine!",
            "Code merging more harmonious than a choir!",
            "Build time optimization quicker than a blink of an eye!",
            "CI/CD pipeline management smoother than a silk scarf!",
            "Serverless function implementation faster than a speeding bullet!",
            "Serverless application monitoring sharper than a hawk's gaze!",
            "Serverless cost optimization more budget-friendly than a clearance sale!",
            "Deployment script creation snazzier than a jazz club!",
            "Firewall configuration tighter than a drum!",
            "Data encryption implementation more secure than Fort Knox!",
            "Backup script writing funnier than a stand-up comedy show!",
            "Disaster recovery plan design more solid than a rock concert in an earthquake!",
            "Code refactoring smoother than a hot knife through butter!",
            "Feature toggle implementation more flexible than a gymnast!",
            "AB testing more intriguing than a mystery novel!",
            "Analytics tool development snappier than a whip crack!",
            "Data processing optimization quicker than a Formula 1 pit stop!",
            "Data pipeline creation smoother than a silk scarf!",
            "Machine learning implementation cooler than the flip side of the pillow!",
            "AI model development more intelligent than Einstein!",
            "Data cleaning more thorough than a spring cleaning!",
            "Data visualization implementation more beautiful than a sunset!",
            "BI report creation more insightful than a Zen master!",
            "BI tool development smoother than a glass of fine wine!",
            "Data storage optimization more efficient than a Swiss watch!",
            "Big data management bigger than the Grand Canyon!",
            "Event sourcing implementation more exciting than a roller coaster ride!",
            "CQRS development more efficient than a Swiss watch!",
            "Data sharding handling smoother than a baby's bottom!",
            "Data replication implementation tighter than a drum!",
            "User interface creation more user-friendly than a golden retriever!",
            "User interface design prettier than a picture-perfect sunset!",
            "User experience optimization smoother than a dolphin's glide!",
            "User research conducting deeper than the Mariana Trench!",
            "Prototype creation faster than a rocket launch!",
            "Prototype development sleeker than a sports car!",
            "Test plan writing more comprehensive than a bestselling novel!",
            "Regression testing more thorough than a detective's investigation!",
            "Functional testing more fun than a carnival!",
            "System testing more robust than a fortress!",
            "UAT conducting more exciting than a treasure hunt!",
            "Feature flag implementation more flexible than a contortionist!",
            "SDK development more helpful than a Swiss Army knife!",
            "Library creation more resourceful than a Swiss bank!",
            "Web security implementation tighter than a drum!",
            "Cross-site scripting prevention funnier than a stand-up comedy show!",
            "SQL injection prevention more secure than a bank vault!",
            "Web server configuration smoother than a silk scarf!",
            "Web server monitoring sharper than an eagle's eye!",
            "Web performance optimization quicker than a cheetah on caffeine!",
            "CSS writing snazzier than a jazz club!",
            "HTML template development cooler than the flip side of the pillow!",
            "Web accessibility handling more inclusive than a family reunion!",
            "Responsive layout creation sleeker than a panther!",
            "Progressive web app development trendier than the latest fashion!",
            "Service worker implementation quicker than lightning!",
            "Web asset optimization more valuable than buried treasure!",
            "CDN implementation faster than a speeding bullet!",
            "Custom theme creation more stylish than a Parisian fashion show!",
            "E-commerce feature development more profitable than a gold mine!",
            "SEO optimization more visible than the Northern Lights!",
            "Meta tag writing more captivating than a bestseller!",
            "Web animation development cooler than the other side of the pillow!",
            "Web font handling more elegant than a swan!",
            "Web form creation smoother than a baby's bottom!",
            "Web form validation more accurate than a sniper!",
            "Payment system integration more secure than Fort Knox!",
            "Shopping cart development more convenient than online shopping!",
            "User tracking implementation more insightful than a psychic!",
            "Session management tighter than a drum!",
            "Social media integration more engaging than a viral video!",
            "Chatbot creation more helpful than a personal assistant!",
            "Push notification implementation more attention-grabbing than a marching band!",
            "Notification system creation more informative than a news anchor!",
            "Push notification optimization more enticing than a siren's call!",
            "Web socket implementation faster than a bolt of lightning!",
            "Real-time feature development more dynamic than a tornado!",
            "File upload handling smoother than a baby's bottom!",
            "Image gallery creation more captivating than a museum exhibit!",
            "Video player development more entertaining than a blockbuster movie!",
            "Audio player implementation more melodic than a symphony!",
            "Media playback optimization quicker than a hummingbird's wings!",
            "Slider creation smoother than silk!",
            "Carousel development more captivating than a merry-go-round!",
            "Lazy loading implementation quicker than a catnap!",
            "Scroll event handling more fluid than a river!",
            "Custom widget creation more versatile than a Swiss Army knife!",
            "Web API development smoother than a silk scarf!",
            "Web dashboard creation more illuminating than a lighthouse!",
            "Web dashboard optimization quicker than a blink of an eye!",
            "API rate limiting tighter than a corset!",
            "Webhook implementation faster than a cheetah!",
            "Serverless API creation cooler than the other side of the pillow!",
            "Serverless function development smoother than a baby's bottom!",
            "Serverless performance optimization quicker than a heartbeat!",
            "Serverless function monitoring sharper than a hawk's gaze!",
            "Error log writing funnier than a sitcom!",
            "Logging system creation more meticulous than a detective!",
            "Monitoring tool implementation more comprehensive than an encyclopedia!",
            "Alert handling quicker than a reflex!",
            "Incident response plan development more solid than a brick!",
            "Incident response optimization smoother than butter!",
            "Incident report creation more informative than a news broadcast!"
        };


        int[] usedTasks = new int[tasks.Length]; // keeps track of which tasks are used
        
        for (int i = 0; i < s_tasksAmount; i++) //initialize 20 tasks
        {
            //randomizing task-Alias and task-description and remarks
            int randIndex;
            do
            {
                randIndex = s_rand.Next(tasks.Length);
            } while (usedTasks[randIndex] == 1);
            usedTasks[randIndex] = 1;
            string randAlias = tasks[randIndex];
            string randDescription = descriptions[randIndex] ?? "";
            string randRemarks = remarks[randIndex] ?? "";
            string randDeliverables = deliverables[randIndex] ?? "";

            //randomizing complexity
            int myComlexity = s_rand.Next(1, 6);
            DO.EngineerExperience randLevel = (DO.EngineerExperience)myComlexity;

            //randomizing date of creation
            DateTime start = new DateTime(2023, 1, 1);
            DateTime randDate = start.AddDays(s_rand.Next(300));

            //creating and adding a new task to the database
            Task newTask = new Task(0, randAlias, randDescription, randDate, new TimeSpan(s_rand.Next(1,15), s_rand.Next(24),0, 0), randLevel, false, null,null,null,null,randDeliverables,randRemarks);
            s_dal!.Task.Create(newTask);
        }
    }

    /// <summary>
    /// Calls the "creat" methods which initialize the database
    /// </summary>
    public static void Do() 
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2;
        s_dal = DalApi.Factory.Get; //stage 4
        creatTasks();
        creatEngineers();
        creatDependencies();
    }

}
