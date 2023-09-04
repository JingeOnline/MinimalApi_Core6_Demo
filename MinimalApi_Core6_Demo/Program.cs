using MinimalApi_Core6_Demo.Models;

namespace MinimalApi_Core6_Demo
{
    public class Program
    {

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //��ӡ��ǰ���л����е�һЩ����
            Console.WriteLine($"Application Name: {builder.Environment.ApplicationName}");
            Console.WriteLine($"Environment Name: {builder.Environment.EnvironmentName}");
            Console.WriteLine($"ContentRoot Path: {builder.Environment.ContentRootPath}");
            Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");
            Console.WriteLine();
            //��appsettings.json�ļ��ж�ȡ������Ϣ
            Console.WriteLine(builder.Configuration["HelloMessage"]);
            Console.WriteLine();


            // Add services to the container.
            builder.Services.AddAuthorization();

            //����Swagger��Swagger�����ַΪ��http://localhost/swagger/index.html
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            //����м���������ļ��ŷ���Ĭ������£�ֻ���ŷ�wwwroot�ļ����µ��ļ���
            //���������ļ��ŷ������ݣ��ο��ٷ��̳�:https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-7.0
            app.UseStaticFiles();

            //�������л��������ô���ҳ��
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/oops");
            }
            app.MapGet("/oops", () => "Oops! An internal error happened.");

            //������־Logger��Ĭ�����������̨��
            //��־��appsettings.json�����á�
            app.Logger.LogInformation("The app started.");

            //��ʱ����Ҫ����Https�ض�����Ϊֻ֧��Http
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            //������Ŀ�Լ����ɵ�Action
            app.MapGet("/", (HttpContext httpContext) => GenerateRandomWeathers()).WithName("GetWeatherForecast");

            #region ·�ɷ�����Actions
            //GET-����
            app.MapGet("/test", () => "Test Get Response").WithName("GET-Basic");
            //GET-��·�ɲ���
            app.MapGet("/test/{userId}/books/{bookId}", (int userId, string bookId) => $"UserId={userId}, BookId={bookId}")
                .WithName("GET-With route parameters");
            //GET-��״̬��
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
            //GET-�첽
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
            //PUT��DELETEʡ��
            #endregion

            app.Run();
        }

        //��Ŀ�Լ����ɵ�ʾ��
        public static string[] summaries = new string[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        /// <summary>
        /// �������һ������Ԥ��������Ŀ�Լ����ɵģ�
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