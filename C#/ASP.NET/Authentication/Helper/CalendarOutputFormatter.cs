using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace YourNamespace.Helper
{
    public class CalendarOutputFormatter : TextOutputFormatter
    {
        public CalendarOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var @event = (Event)context.Object; 
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("BEGIN:VCALENDAR");
            builder.AppendLine("VERSION:2.0");
            builder.AppendLine($"PRODID:-//A2//NONSGML v1.0//EN");
            builder.AppendLine("BEGIN:VEVENT");
            builder.AppendLine($"PRODID:{"hsir571"}");  
            builder.AppendLine($"UID:{@event.Id}");
            builder.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");  
            builder.AppendLine($"DTSTART:{@event.Start}");
            builder.AppendLine($"DTEND:{@event.End}");
            builder.AppendLine($"SUMMARY:{@event.Summary}");
            builder.AppendLine($"DESCRIPTION:{@event.Description}");
            builder.AppendLine($"LOCATION:{@event.Location}");
            builder.AppendLine("END:VEVENT");
            builder.AppendLine("END:VCALENDAR");

            string outString = builder.ToString();
            byte[] outBytes = selectedEncoding.GetBytes(outString);
            var response = context.HttpContext.Response.Body;

            return response.WriteAsync(outBytes, 0, outBytes.Length);
        }
    }
}
