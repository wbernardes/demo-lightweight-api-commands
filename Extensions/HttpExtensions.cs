using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using DemoLightweightApi.Commands;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DemoLightweightApi.Extensions
{
    public static class HttpExtensions
    {
        private static string COMMANDS_ASSEMBLY = Assembly.GetEntryAssembly().GetName().Name;
        private static string COMMANDS_NAMESPACE = $"{COMMANDS_ASSEMBLY}.Commands";

        private static readonly JsonSerializer _serializer = new JsonSerializer();
        public static Task WriteJson<T>(this HttpResponse response, T obj)
        {
            response.ContentType = "application/json";
            return response.WriteAsync(JsonConvert.SerializeObject(obj));
        }

        public static async Task<T> ReadFromJson<T>(this HttpContext httpContext)
        {
            using (var streamReader = new StreamReader(httpContext.Request.Body))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var obj = _serializer.Deserialize<T>(jsonTextReader);

                var results = new List<ValidationResult>();
                if (Validator.TryValidateObject(obj, new ValidationContext(obj), results))
                {
                    return obj;
                }

                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteJson(results);
                return default(T);
            }
        }
        public static Command ReadAsCommand(this HttpContext context, string action)
        {
            var pascalizedAction =  Regex.Replace(action, "(?:^|_)(.)", match => match.Groups[1].Value.ToUpper());
            var commandQualifiedName = $"{COMMANDS_NAMESPACE}.{pascalizedAction}, {COMMANDS_ASSEMBLY}";
            var commandType = Type.GetType(commandQualifiedName);
            
            if(commandType == null)
            {
                return null;
            }
            
            using (var sr = new StreamReader(context.Request.Body))
            using (var jr = new JsonTextReader(sr))
            {
                return _serializer.Deserialize(jr, commandType) as Command;
            }
        }
    }
}
