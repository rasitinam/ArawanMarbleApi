using ArawanMarbleApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // T�m origin'lere izin ver
              .AllowAnyMethod()    // T�m HTTP metodlar�na izin ver (GET, POST, vb.)
              .AllowAnyHeader();   // T�m header'lara izin ver
    });
});
builder.Services.AddEndpointsApiExplorer();  // API'nin u� noktalar�n� ke�fetmek i�in
builder.Services.AddSwaggerGen();  // Swagger'� ekliyoruz
// Servisleri ekle
builder.Services.AddControllers();
builder.Services.AddDbContext<ArawanDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));  // SQLite ba�lant� dizesi
var app = builder.Build();

// Middleware'leri do�ru s�rayla ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseCors("AllowAll");  // CORS politikas�n� uygulamaya dahil et

// Middleware'leri ekleyelim
app.UseRouting();
app.UseHttpsRedirection();

// �NCE UseDefaultFiles, SONRA UseStaticFiles
app.UseDefaultFiles(); // index.html, default.html, vs. arar
app.UseStaticFiles(); // wwwroot i�indeki dosyalar� sunar
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Swagger'� kullanmaya ba�l�yoruz
    app.UseSwaggerUI();  // Swagger UI'yi aktif hale getiriyoruz
}

app.UseAuthorization();
app.MapControllers();

// SPA i�in gerekirse (t�m route'lar� index.html'e y�nlendirir)
app.MapFallbackToFile("index.html");

app.Run();