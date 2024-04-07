## Prerequisites
### Required:
1. Visual Studio 2022
2. .NET 8 runtime
3. Python 3.7 and all module in requirements.txt
### Not Required:
- MSSQL (or MySql, but I've not tried it yet) from [here](https://github.com/tyh7121/DANMCNPM/tree/feature/firstCrawler)

---
## Steps to run the code:
1. Clone the source code (or download and extract it).
2. Open the file **WebApplication1.sln**.
3. In **CrawlerProject_API**:
   ![image](https://github.com/thuanan7/CrawlerProject/assets/47140156/fb8fcf02-e01b-42a9-92e7-25af98e6145f)
   - **(If you have your own DB)**: Open **appsettings.json** then edit **"ConnectionStrings":"CrawlerProjectDb"** with the configuration of your DB.
     (Example: "Server={ServerName};Database={Database};User ID=sa;Password={YourPassword};Trusted_Connection=false;TrustServerCertificate=True;MultipleActiveResultSets=True;")
   - **(If you don't have a DB)**: Open **appsettings.Production.json**, copy all then replace **appsettings.json** with the copy (this is my sample database).

4. **(If you want to use a Python virtual environment)**: In **CrawlerApplication**, right-click Python Environment then create or add an existing one. For more information: [Managing Python Environments in Visual Studio](https://learn.microsoft.com/en-us/visualstudio/python/managing-python-environments-in-visual-studio?view=vs-2022)

5. In **(CrawlerApplication->it_conference->it_conferences->spiders->conferences_crawler.py)**: Edit database information in the **constructor(def __init__())** of **class ConferencesCrawler** (Same as step 3)
   ![image](https://github.com/thuanan7/CrawlerProject/assets/47140156/a2d3bbe2-6ae1-4755-9da5-295abb68a160)

6. Right-click Solution -> Configure Startup Projects -> check Multiple startup projects -> change every project's Action to "start" (Make sure API starts first).

7. Start projects using HTTPS.
