from it_conference.it_conference.spiders.conferences_crawler import ConferencesCrawler
from scrapy.crawler import CrawlerProcess
from scrapy.utils.project import get_project_settings
import os
from apscheduler.schedulers.twisted import TwistedScheduler


class Scraper:
    def __init__(self):
        settings_file_path = 'it_conference.it_conference.settings' # The path seen from root, ie. from main.py
        os.environ.setdefault('SCRAPY_SETTINGS_MODULE', settings_file_path)
        self.process = CrawlerProcess(get_project_settings())
        self.spider = ConferencesCrawler # The spider you want to crawl
        self.scheduler = TwistedScheduler()

    def run_spiders(self):
        self.scheduler.add_job(self.process.crawl, 'interval', args=[self.spider], seconds=60)
        self.scheduler.start()
        self.process.start(False)