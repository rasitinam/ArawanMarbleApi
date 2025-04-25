using ArawanMarbleApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Tüm origin'lere izin ver
              .AllowAnyMethod()    // Tüm HTTP metodlarýna izin ver (GET, POST, vb.)
              .AllowAnyHeader();   // Tüm header'lara izin ver
    });
});
builder.Services.AddEndpointsApiExplorer();  // API'nin uç noktalarýný keþfetmek için
builder.Services.AddSwaggerGen();  // Swagger'ý ekliyoruz
// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddDbContext<ArawanDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));  // SQLite baðlantý dizesi
var app = builder.Build();

// Middleware'leri doðru sýrayla ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowAll");  // CORS politikasýný uygulamaya dahil et

// Middleware'leri ekleyelim
app.UseRouting();
app.UseHttpsRedirection();

// ÖNCE UseDefaultFiles, SONRA UseStaticFiles
app.UseDefaultFiles(); // index.html, default.html, vs. arar
app.UseStaticFiles(); // wwwroot içindeki dosyalarý sunar
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Swagger'ý kullanmaya baþlýyoruz
    app.UseSwaggerUI();  // Swagger UI'yi aktif hale getiriyoruz
}

app.UseAuthorization();
app.MapControllers();

// SPA için gerekirse (tüm route'larý index.html'e yönlendirir)
app.MapFallbackToFile("index.html");

app.Run();