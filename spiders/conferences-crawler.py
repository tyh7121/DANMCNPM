import scrapy
from scrapy_splash import SplashRequest
from mysql.connector import connect, Error

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
    def __init__(self, **kwargs):
        super().__init__(**kwargs)
        try:
            self.db = connect(
                host="localhost",
                user="root",
                database="conferences",
                password=""
            )
            print("Database connection established.")
        except Error as e:
            print("Error establishing database connection:")
            print(e)

    name = "conferences"

    def start_requests(self):
        urls = [
            "https://www.allconferencealert.com/engineering-and-technology.html"
        ]
        for url in urls:
            yield SplashRequest(url=url, callback=self.parse, args={'wait': 2})

    def parse(self, response):
        conferences = response.css("tr.data1")
        # i = 0
        for conf in conferences:
            # if i > 0:
            #     break

            conference = Conference()

            # i = i + 1

            td0 = conf.xpath("./td")[0]
            conference['date'] = td0.xpath("./a/span/text()").get()
            # Get date of conf
            # print("Conf date:" + td0.xpath("./a/span/text()").get())
            
            td1 = conf.xpath("./td")[1]
            conference['title'] = td1.xpath("./a/text()").get()
            # Get conf title
            # print("Conf title:" + td1.xpath("./a/text()").get())
            
            td2 = conf.xpath("./td")[2]
            conference['country'] = td2.xpath("./strong/a/text()").get().strip()
            conference['url'] = td2.xpath("./strong/a/@href").get()
            # Get conf country
            # print("Country:" + td2.xpath("./strong/a/text()").get().strip())
            # get conf link
            # print("Conf URL:" + td2.xpath("./strong/a/@href").get())

            # print(conference)
            # print("---\n")

            yield response.follow(conference['url'], self.parse_conf, meta={'conference': conference})

    def parse_conf(self, response):
        conference = response.meta['conference']
        event_details = response.css("div.conference-detail ul")[0]

        num_event_details = len(event_details.xpath("./li"))

        # print("Event Status: " + event_details.xpath("./li[2]/span/text()").get().strip())
        conference['event_status'] = event_details.xpath("./li[2]/span/text()").get().strip()

        if num_event_details == 11:
            # print("Organizer: " + event_details.xpath("./li[4]/span/text()").get().strip())
            conference['organizer'] = event_details.xpath("./li[4]/span/text()").get().strip()

            # print("Deadline: " + event_details.xpath("./li[6]/h3/following-sibling::text()").get().strip())
            conference['deadline'] = event_details.xpath("./li[6]/h3/following-sibling::text()").get().strip()

            # print("Start Date: " + event_details.xpath("./li[7]/span/text()").get().strip())
            conference['start_date'] = event_details.xpath("./li[7]/span/text()").get().strip()

            # print("End Date: " + event_details.xpath("./li[8]/span/text()").get().strip())
            conference['end_date'] = event_details.xpath("./li[8]/span/text()").get().strip()

            # print("Secretary: " + event_details.xpath("./li[9]/h3/following-sibling::text()").get().strip())
            conference['secretary'] = event_details.xpath("./li[9]/h3/following-sibling::text()").get().strip()

            # print("Inquiry Email: " + event_details.xpath("./li[10]/h3/following-sibling::text()").get().strip())
            conference['inquiry_email'] = event_details.xpath("./li[10]/h3/following-sibling::text()").get().strip()

            # print("Registration URL: " + event_details.xpath("./li[11]/span//a/@href").get().strip())
            conference['registration_url'] = event_details.xpath("./li[11]/span//a/@href").get().strip()
        elif num_event_details == 10:
            # print("Organizer: " + event_details.xpath("./li[3]/span/text()").get().strip())
            conference['organizer'] = event_details.xpath("./li[3]/span/text()").get().strip()

            # print("Deadline: " + event_details.xpath("./li[5]/h3/following-sibling::text()").get().strip())
            conference['deadline'] = event_details.xpath("./li[5]/h3/following-sibling::text()").get().strip()

            # print("Start Date: " + event_details.xpath("./li[6]/span/text()").get().strip())
            conference['start_date'] = event_details.xpath("./li[6]/span/text()").get().strip()

            # print("End Date: " + event_details.xpath("./li[7]/span/text()").get().strip())
            conference['end_date'] = event_details.xpath("./li[7]/span/text()").get().strip()

            # print("Secretary: " + event_details.xpath("./li[8]/h3/following-sibling::text()").get().strip())
            conference['secretary'] = event_details.xpath("./li[8]/h3/following-sibling::text()").get().strip()

            # print("Inquiry Email: " + event_details.xpath("./li[9]/h3/following-sibling::text()").get().strip())
            conference['inquiry_email'] = event_details.xpath("./li[9]/h3/following-sibling::text()").get().strip()

            # print("Registration URL: " + event_details.xpath("./li[10]/span//a/@href").get().strip())
            conference['registration_url'] = event_details.xpath("./li[10]/span//a/@href").get().strip()

        # print(conference.__str__())

        self.store_conference(conference)

        yield conference.__str__()

    def store_conference(self, conference):
        insert_conferences_query = """
            INSERT INTO conferences
            (date, title, country, url, event_status, organizer, deadline, start_date, end_date, secretary, inquiry_email, registration_url)
            VALUES 
            (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
        """
        values = (conference['date'], conference['title'], conference['country'], conference['url'], conference['event_status'], conference['organizer'], conference['deadline'], conference['start_date'], conference['end_date'], conference['secretary'], conference['inquiry_email'], conference['registration_url'])
        try:
            cursor = self.db.cursor()
            cursor.execute(insert_conferences_query, values)
            self.db.commit()
            print(cursor.rowcount, " row(s) were inserted.")
        except Error as e:
            print("Error inserting a conference into the table:")
            print(e)
