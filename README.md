# Pre-requsities

Please install if you do not have any of the following:

- Python 3.7
- Docker Desktop
- MySQL
- Code Editor of your choice (PyCharm preferred for simplicity)

Steps to run the code:

1. Clone the code from Github
2. Go into the code directory via PyCharm
3. Allow PyCharm to create a Virtual Environment
4. Make sure PyCharm has installed the required libraries from requirements.txt file
5. Run the following commands to run [Scrapy Flash](https://github.com/scrapy-plugins/scrapy-splash) from your terminal

- `docker pull scrapinghub/splash`
- `docker run -p 8050:8050 scrapinghub/splash`

6. Run your MySQL database, it is not already running
7. Connect to your MYSQL database. Run the following commands:

- `CREATE DATABASE conferences;`
- `CREATE TABLE conferences(date varchar(10), title varchar(250), country varchar(100), url varchar(512), event_status varchar(10), organizer varchar(100), deadline varchar(10), start_date varchar(10), end_date varchar(10), secretary varchar(100), inquiry_email varchar(100), registration_url varchar(512));`

8. In the PyCharm terminal, activate your Virtual Environment that is crated by PyCharm

- `source <your_virual_env_path>/bin/activate` (if you're on MacOS). Please find the same for Windows.

9. Finally, run `scrapy crawl conferences` to run the crawler
