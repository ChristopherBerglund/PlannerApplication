
1. Download Heidi SQL.
2. Run both script files as queries in Heidi;
	2.1. Planner Application SQL script as new query = (ReadMe/Planner SQL Script.txt) (Identity)
	2.2. Planner SQL script as new query = (ReadMe/Planner SQL Script.txt) (Application)
	(This set some dummy data into the tables).

3. Paste your ConnectionString in appsettings.

	 "Defaultconnection": "server=[SERVER];user id=[USERNAME];password=[PASSWORD];database=planner;",
	 "IdentityConnection": "server=[SERVER];user id=[USERNAME];password=[PASSWORD];Database=PlannerApplication;"

4. Start application.
5. Register, allow the application to get your location. (pop-up)
6. Create a new activity.
	6.1. Enter a adress, click find adress.
	6.2. Enter your credentials for the activity.
	6.3. CLick Create button.
	Weei! You have created an event, that other users can book!

7. Filter or search for event on different activites.
8. Sort events on time and location. 
9. Book event, unbock event, delete event. 

