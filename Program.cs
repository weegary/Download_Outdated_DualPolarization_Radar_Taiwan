// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Net;

Console.WriteLine(" === This program downloads dual polarization radar's echo image from cwb.gov.tw. === ");
Console.WriteLine();

# region SetParameter 
int selectedIndex = (int)Radar.Shulin;                              // Select the Radar you want to download.
DateTime startDT = new DateTime(2021, 06, 04, 13, 00, 00);
DateTime endDT = new DateTime(2021, 06, 04, 14, 00, 00);
#endregion

DateTime time = endDT;
string url = "";
string url_LY = @"https://tsafe.cwb.gov.tw/TAHOPE_2020/tahope_data/{0}/ops/Radar_Linyuan_Cband/{1}/ops.Radar_Linyuan_Cband.{2}.CV1_RCLY_3600.png";
string url_SL = @"https://tsafe.cwb.gov.tw/TAHOPE_2020/tahope_data/{0}/ops/Radar_Shulin_Cband/{1}/ops.Radar_Shulin_Cband.{2}.CV1_RCSL_3600.png";
string url_NT = @"https://tsafe.cwb.gov.tw/TAHOPE_2020/tahope_data/{0}/ops/Radar_Nantun_Cband/{1}/ops.Radar_Nantun_Cband.{2}.CV1_RCNT_3600.png";
string[] URLS = { url_SL, url_LY, url_NT };
string[] radar_name = { "RCSL", "RCLY", "RCNT" };

IWebDriver m_driver;
var options = new ChromeOptions();
options.AddArgument("headless");
m_driver = new ChromeDriver(options);
while (time > startDT)
{
    DateTime urlTime = time.AddHours(-8);
    url = String.Format(URLS[selectedIndex], urlTime.ToString("yyyy"),urlTime.ToString("yyyyMMdd"), urlTime.ToString("yyyyMMddHHmm"));
    m_driver.Url = url;
    List<IWebElement> elementList = new List<IWebElement>();
    elementList.AddRange(m_driver.FindElements(By.CssSelector("body > img")));

    if (elementList.Count > 0)
    {
        //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
        WebClient client = new WebClient();
        client.DownloadFile(url, String.Format("{0}\\CV1_{0}_3600_{1}.png", radar_name[selectedIndex], time.ToString("yyyyMMddHHmm")));
    }
    time = time.AddMinutes(-1);

}
m_driver.Close();

Console.WriteLine(" === Finish downloading. ===");
Console.WriteLine(" >> Bye << ");

enum Radar { Shulin, Linyuan, Nantun };