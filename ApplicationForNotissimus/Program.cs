using ApplicationForNotissimus;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Text;
using System.Xml;
using System.Xml.Linq;

//Getting connection string from file assembly
var cBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");
var configuration = cBuilder.Build();
var connectionString = Microsoft
   .Extensions
   .Configuration
   .ConfigurationExtensions
   .GetConnectionString(configuration, "NotisDB");
//Create a project
var builder = WebApplication.CreateBuilder();
var app = builder.Build();
//Setting url and Id for search
var url = "http://partner.market.yandex.ru/pages/help/YML.xml";
var requirementsId = "12344";
/// <summary>
/// The result is displayed by switching to /View
/// </summary>
app.Map("/View", async (appB) => {
    string result = null;
    using (var dbContext = new MyDbContext(connectionString))
    {
        if (dbContext.Offers.Where(s => s.Id == requirementsId).Count() == 1)
        {
            /*This variable will store the contents of the Bootstrap/Index.html file,
            and then padded with the resulting values. */
            StringBuilder html = new StringBuilder("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <title>DB offers</title>\r\n    <meta charset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css\">\r\n    <script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.6.3/jquery.min.js\"></script>\r\n    <script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js\"></script>\r\n</head>\r\n<body>\r\n\r\n    <div class=\"container\">\r\n        <h2>Basic Table</h2>\r\n        <table class=\"table\">\r\n            <thead>");
            html.Append("<tr>");
            foreach (var v in typeof(Offer).GetProperties())
            {
                html.Append($"<th>{v.Name}</th>");
            }
            html.Append("</tr>");
            html.Append("</thead>");
            html.Append("<tbody>");
            var offer = dbContext.Offers.Where(s => s.Id == requirementsId).Take(1).ToArray().First();
            html.Append("<tr>");
            foreach (var v in offer.GetType().GetProperties())
            {
                html.Append($"<td>{v.GetValue(offer)}</td>");
            }
            html.Append("</tr>");
            html.Append("</tbody>\r\n        </table>\r\n    </div>\r\n\r\n</body>\r\n</html>");
            result = html.ToString();
        }
        else
        {
            result = "Id not found";
        }
    }
    appB.Run(async context => {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(result); });
});

/// <summary>
/// The commented out part in the method was used to get all the unique
/// fields for all offers from the file at once, because you may need to store any of them.
/// </summary>
app.Run(async (context) => {
    //Changing the encoding is necessary because file uses windows-1251
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    XmlDocument xDoc = new XmlDocument();
    xDoc.Load(url);
    //Get all offers
    XmlElement? xOffers = xDoc.DocumentElement["shop"]["offers"];
    XmlElement req = null;
    var s = new List<string>();
    foreach (var v in xOffers)
    {
        if (((XmlElement)v).GetAttribute("id") == "12344")
        {
            req = (XmlElement)v;
            break;
        }
        
        /*foreach (var el in ((XmlElement)v).ChildNodes)
        {
            s.Add((el as XmlNode).Name);
        }*/
    }
    /*var r = s.Distinct().Order();
    foreach (var v in r)
    {
        Console.WriteLine(v);
    }*/
    var add = false;
    using (var dbContext = new MyDbContext(connectionString))
    {
        if (dbContext.Offers.Where(s => s.Id == requirementsId).Count() == 0)
        {
            add = true;
            var offer = new Offer()
            {
                Id = (req.GetAttribute("id"))
            };
            /*Filling an instance of the offer class with values ​​from the xml object, the required fields
            selected by reflection*/
            foreach (XmlNode v in req.ChildNodes)
            {
                var property = offer.GetType().GetProperties().Where(o => o.Name == v.Name).First();
                property.SetValue(offer, v.InnerText);
            }
            dbContext.Offers.Add(offer);
            dbContext.SaveChanges();
        }
    }
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(add == true?"Offer added to db":"Offer already exist in db");
});

app.Run();