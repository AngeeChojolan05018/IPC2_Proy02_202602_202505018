var builder = WebApplication.CreateBuilder(args);

// Agregar servicios para Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configuración del manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Permitir archivos estáticos
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Mapear las páginas Razor
app.MapRazorPages();

app.Run();