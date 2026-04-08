using Microsoft.Extensions.Primitives;

namespace api_gateway.Configuration;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Tenta pegar o token do cabeçalho Authorization da requisição
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        // Verifique se o token está presente
        if (!string.IsNullOrEmpty(token))
        {
            // Verifica se o cabeçalho Authorization já existe
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                // Se o cabeçalho já contém o "Bearer", não adicionamos o token novamente
                if (!context.Request.Headers["Authorization"].ToString().Contains("Bearer"))
                {
                    // Se o cabeçalho não contém "Bearer", então adicionamos
                    context.Request.Headers["Authorization"] = new StringValues("Bearer " + token);
                }
            }
            else
            {
                // Se o cabeçalho Authorization não existe, adicione o token como "Bearer"
                context.Request.Headers["Authorization"] = new StringValues("Bearer " + token);
            }
        }

        // Passa para o próximo middleware
        await _next(context);
    }

}
