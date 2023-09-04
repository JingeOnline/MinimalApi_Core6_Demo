using MinimalApi_Core6_Demo.Models;

namespace MinimalApi_Core6_Demo
{
    public class Program
    {

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //打印当前运行环境中的一些变量
            Console.WriteLine($"Application Name: {builder.Environment.ApplicationName}");
            Console.WriteLine($"Environment Name: {builder.Environment.EnvironmentName}");
            Console.WriteLine($"ContentRoot Path: {builder.Environment.ContentRootPath}");
            Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");
            Console.WriteLine();
            //从appsettings.json文件中读取配置信息
            Console.WriteLine(builder.Configuration["HelloMessage"]);
            Console.WriteLine();


            // Add services to the container.
            builder.Services.AddAuthorization();

            //配置Swagger，Swagger请求地址为：http://localhost/swagger/index.html
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            //添加中间件，启用文件伺服。默认情况下，只会伺服wwwroot文件夹下的文件。
            //其他关于文件伺服的内容，参考官方教程:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-7.0
            app.UseStaticFiles();

            //根据运行环境来配置错误页面
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/oops");
            }
            app.MapGet("/oops", () => "Oops! An internal error happened.");

            //配置日志Logger，默认输出到控制台。
            //日志在appsettings.json中配置。
            app.Logger.LogInformation("The app started.");

            //暂时不需要开启Https重定向，因为只支持Http
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            //这是项目自己生成的Action
            app.MapGet("/", (HttpContext httpContext) => GenerateRandomWeathers()).WithName("GetWeatherForecast");

            #region 路由方法，Actions
            //GET-基本
            app.MapGet("/test", () => "Test Get Response").WithName("GET-Basic");
            //GET-带路由参数
            app.MapGet("/test/{userId}/books/{bookId}", (int userId, string bookId) => $"UserId={userId}, BookId={bookId}")
                .WithName("GET-With route parameters");
            //GET-带状态码
            app.MapGet("/test/{userId}", (int userId) =>
            {
                if (userId > 0 && userId <= 10)
                {
                    return Results.Ok(new Student(userId, "Jackson", true, "This is a valid user."));
                }
                else
                {
                    return Results.NotFound("The user doesn't exit.");
                }
            }).WithName("GET-With response code");
            //GET-异步
            app.MapGet("test/async", async () =>
            {
                await Task.Delay(3000);
                return Results.Ok("Wait 3 seconds and return.");
            }).WithName("GET-Async");
            //POST
            app.MapPost("/test", (Student stu) =>
            {
                return Results.Ok(stu);
            });
            //PUT和DELETE省略
            #endregion

            app.Run();
        }

        //项目自己生成的示例
        public static string[] summaries = new string[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        /// <summary>
        /// 随机生成一组天气预报对象（项目自己生成的）
        /// </summary>
        /// <returns></returns>
        public static WeatherForecast[] GenerateRandomWeathers()
        {
            WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();
            return forecast;
        }
    }
}