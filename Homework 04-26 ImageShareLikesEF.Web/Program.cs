namespace ImageShareLikes.Web
{
    public class Program
    {

        private static string CookieScheme = "Authentication";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //does this need to be fixed?
            builder.Services.AddAuthentication(CookieScheme)
            //builder.Services.ImageShareLikes(CookieScheme)
             .AddCookie(CookieScheme, options =>
             {
                 options.LoginPath = "/account/logIn";
             });

            builder.Services.AddSession();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}