import scrapy
from scrapy_splash import SplashRequest, SplashFormRequest
from websocket import create_connection

# MSSQ
from pymssql import connect, Error
#If using Mysql
#from mysql.connector import connect, Error

import ssl
from scrapy.utils.reactor import install_reactor



class Conference(scrapy.Item):
    date = scrapy.Field()
    title = scrapy.Field()
    country = scrapy.Field()
    url = scrapy.Field()
    event_status = scrapy.Field()
    organizer = scrapy.Field()
    deadline = scrapy.Field()
    start_date = scrapy.Field()
    end_date = scrapy.Field()
    secretary = scrapy.Field()
    inquiry_email = scrapy.Field()
    registration_url = scrapy.Field()

class ConferencesCrawler(scrapy.Spider):
    isDevelopment = True

    install_reactor("twisted.internet.asyncioreactor.AsyncioSelectorReactor")
    name = "conferences"
    next_page = 1
    total_pages = 1
    max_conference = 50
    
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        self.conferences = []
        try:
            if self.isDevelopment:
                self.db = connect(
                    host="localhost:1433",
                    user="sa",
                    database="conferences",
                    password="Password123"
                )
            else:
                self.db = connect(
                    host="hotelprojectserver.database.windows.net:1433",
                    user="sqladmin",
                    database="HotelAPI",
                    password="Asddsaas1#"
            )
            print("Database connection established.")
        except Error as e:
            print("Error establishing database connection:")
            print(e)
    
    def start_requests(self):
        urls = [
            "https://www.allconferencealert.com/engineering-and-technology.html"
        ]
        for url in urls:
            yield SplashRequest(url=url, callback=self.parse, args={'wait': 2})
    
    def closed(self, reason):
        print("-------------------------Saving----------------------------")
        self.store_conference()
        print("-------------------------DONE----------------------------")
        
        if self.isDevelopment:
            ws = create_connection("wss://localhost:7150/ws",  sslopt={"cert_reqs": ssl.CERT_NONE})
        else:
            ws = create_connection("wss://crawlerproject-web.azurewebsites.net/ws",  sslopt={"cert_reqs": ssl.CERT_NONE})
        
        print("Sending")
        ws.send("reload")
        print("Sent")
        print("Receiving...")
        result =  ws.recv()
        print("Received '%s'" % result)
        ws.close()
        print("closed")
                
    def parse(self, response):
        print("In parse now....")
        conferences = response.css("tr.data1")
        print("length of conferences: " + str(len(conferences.xpath("./li"))))
        
        i = 0
        for conf in conferences:
            i+=1
            if i > self.max_conference:
                break
            conference = Conference()
            td0 = conf.xpath("./td")[0]
            conference['date'] = td0.xpath("./a/span/text()").get()
            # Get date of conf
            print("Conf date:" + td0.xpath("./a/span/text()").get())
            
            td1 = conf.xpath("./td")[1]
            conference['title'] = td1.xpath("./a/text()").get()
            # Get conf title
            print("Conf title:" + td1.xpath("./a/text()").get())
            
            td2 = conf.xpath("./td")[2]
            conference['country'] = td2.xpath("./strong/a/text()").get().strip()
            conference['url'] = td2.xpath("./strong/a/@href").get()
            # Get conf country
            print("Country:" + td2.xpath("./strong/a/text()").get().strip())
            # get conf link
            print("Conf URL:" + td2.xpath("./strong/a/@href").get())
            # print(conference)
            # print("---\n")
            yield response.follow(conference['url'], self.parse_conf, meta={'conference': conference})


        pagination = response.css("div.pagination ul")
        is_next_page_active = pagination.xpath("./li[10]").get("class")
        while 'active' in is_next_page_active and self.next_page < self.total_pages:
            self.next_page = self.next_page + 1
            yield SplashFormRequest(url="https://www.allconferencealert.com/cat_load_pagi_data.php?topic=Engineering%20and%20Technology&date=", callback=self.parse, args={'wait': 5}, formdata={'page': str(self.next_page)})
            
            

    def parse_conf(self, response):
        print("In parse_conf now....")
        conference = response.meta['conference']
        event_details = response.css("div.conference-detail ul")[0]

        num_event_details = len(event_details.xpath("./li"))

        conference['event_status'] = event_details.xpath("./li[2]/span/text()").get().strip()

        if num_event_details == 11:
            conference['organizer'] = event_details.xpath("./li[4]/span/text()").get().strip()
            conference['deadline'] = event_details.xpath("./li[6]/h3/following-sibling::text()").get().strip()
            conference['start_date'] = event_details.xpath("./li[7]/span/text()").get().strip()
            conference['end_date'] = event_details.xpath("./li[8]/span/text()").get().strip()
            conference['secretary'] = event_details.xpath("./li[9]/h3/following-sibling::text()").get().strip()
            conference['inquiry_email'] = event_details.xpath("./li[10]/h3/following-sibling::text()").get().strip()
            conference['registration_url'] = event_details.xpath("./li[11]/span//a/@href").get().strip()
        elif num_event_details == 10:
            conference['organizer'] = event_details.xpath("./li[3]/span/text()").get().strip()
            conference['deadline'] = event_details.xpath("./li[5]/h3/following-sibling::text()").get().strip()
            conference['start_date'] = event_details.xpath("./li[6]/span/text()").get().strip()
            conference['end_date'] = event_details.xpath("./li[7]/span/text()").get().strip()
            conference['secretary'] = event_details.xpath("./li[8]/h3/following-sibling::text()").get().strip()
            conference['inquiry_email'] = event_details.xpath("./li[9]/h3/following-sibling::text()").get().strip()
            conference['registration_url'] = event_details.xpath("./li[10]/span//a/@href").get().strip()

        self.conferences.append(conference)

        yield conference

    def store_conference(self):

        insert_conferences_query = """
            INSERT INTO conferences
            (date, title, country, url, event_status, organizer, deadline, start_date, end_date, secretary, inquiry_email, registration_url)
            VALUES 
            (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
        """
        try:
            cursor = self.db.cursor()
            
            cursor.execute("""
            DELETE FROM conferences
            """)
            print("Deleted Conferences")
            listcon = []
            for conference in self.conferences:
                values = (conference['date'], conference['title'], conference['country'], conference['url'], conference['event_status'], conference['organizer'], conference['deadline'], conference['start_date'], conference['end_date'], conference['secretary'], conference['inquiry_email'], conference['registration_url'])
                cursor.execute(insert_conferences_query, values)
            
            self.db.commit()
            print("Conferences data stored in the database.")
        except Error as e:
            print("Error inserting conferences into the table:")
            print(e)
        finally:
            self.db.close()